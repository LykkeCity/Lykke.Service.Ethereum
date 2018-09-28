﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;

namespace Lykke.Service.Ethereum.QueueConsumers
{
    public abstract class QueueConsumerBase<T> : IStartable, IStopable
    {
        private readonly int _emptyQueueCheckInterval;
        private readonly SemaphoreSlim _throttler;
        
        
        private CancellationTokenSource _cts;   
        private Task _executingTask;


        protected QueueConsumerBase(
            int emptyQueueCheckInterval = 1000,
            int maxDegreeOfParallelism  = 1)
        {
            _emptyQueueCheckInterval = emptyQueueCheckInterval;
            _throttler = new SemaphoreSlim(maxDegreeOfParallelism);
        }

        private void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                Stop();
                
                _throttler.Dispose();
                
                _cts?.Dispose();
                _executingTask?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            
            GC.SuppressFinalize(this);
        }
        
        public virtual void Start()
        {
            if (_executingTask == null)
            {
                _cts = new CancellationTokenSource();
                _executingTask = RunAsync(_cts.Token);
            }
        }

        public virtual void Stop()
        {
            if (_executingTask != null)
            {
                _cts.Cancel(false);
            
                _executingTask.Wait();

                _executingTask.Dispose();

                _executingTask = null;
            }
        }

        
        protected abstract Task<(bool, T)> TryGetNextTaskAsync();

        protected abstract Task ProcessTaskAsync(
            T task);


        private async Task ProcessTaskAndReleaseThrottlerAsync(
            T task)
        {
            try
            {
                await ProcessTaskAsync(task);
            }
            finally
            {
                _throttler.Release();
            }
        }
        
        private async Task RunAsync(
            CancellationToken cancellationToken)
        {
            var scheduledTasks = new List<Task>();
            
            while (true)
            {
                await _throttler.WaitAsync(cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    scheduledTasks.RemoveAll(x => x.IsCompleted);

                    var (nextTaskRetrieved, nextTask) = await TryGetNextTaskAsync();

                    if (nextTaskRetrieved)
                    {
                        scheduledTasks.Add
                        (
                            ProcessTaskAndReleaseThrottlerAsync(nextTask)
                        );
                    }
                    else
                    {
                        await Task.Delay(_emptyQueueCheckInterval, cancellationToken);
                        
                        _throttler.Release();
                    }
                }
                else
                {
                    break;
                }
            }

            await Task.WhenAll(scheduledTasks);
        }
    }
}
