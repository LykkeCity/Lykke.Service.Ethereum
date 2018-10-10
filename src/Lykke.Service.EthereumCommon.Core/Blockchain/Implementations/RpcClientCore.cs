using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Lykke.Service.Ethereum.Core.Blockchain.Exceptions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class RpcClientCore : IRpcClientCore
    {
        private readonly Uri _apiUrl;
        private readonly BlockchainType _blockchainType;
        private readonly TimeSpan _connectionTimeout;
        private readonly bool _disableTelemetry;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelemetryClient _telemetryClient;

        
        protected RpcClientCore(
            Uri apiUrl,
            BlockchainType blockchainType,
            TimeSpan connectionTimeout,
            bool disableTelemetry,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _blockchainType = blockchainType;
            _connectionTimeout = connectionTimeout;
            _disableTelemetry = disableTelemetry;
            _httpClientFactory = httpClientFactory;

            if (_disableTelemetry)
            {
                _telemetryClient = new TelemetryClient();
            }
        }

        /// <inheritdoc />
        public virtual async Task<RpcResponse> SendRpcRequestAsync(
            RpcRequest request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var cts = new CancellationTokenSource();

                cts.CancelAfter(_connectionTimeout);
                
                
                var requestJson = JsonConvert.SerializeObject(request);  
                var httpRequest = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync(_apiUrl, httpRequest, cts.Token);

                httpResponse.EnsureSuccessStatusCode();

                var responseJson = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RpcResponse>(responseJson);

                
                return response;
            }
            catch (TaskCanceledException e)
            {
                throw new RpcClientTimeoutException
                (
                    _connectionTimeout,
                    request, 
                    e
                );
            }
            catch (Exception e)
            {
                throw new RpcClientException
                (
                    "Error occurred while trying to send rpc request.",
                    request,
                    e
                );
            }
        }
        
        /// <inheritdoc />
        public virtual async Task<RpcResponse> SendRpcRequestWithTelemetryAsync(
            RpcRequest request)
        {
            if (!_disableTelemetry)
            {
                var operationHolder = StartOperation(request);

                try
                {
                    var response = await SendRpcRequestAsync(request); 
                    
                    operationHolder.Telemetry.Success = true;
                    
                    return response;
                }
                catch (Exception)
                {
                    operationHolder.Telemetry.Success = false;

                    throw;
                }
                finally
                {
                    _telemetryClient.StopOperation(operationHolder);
                }
            }
            else
            {
                return await SendRpcRequestAsync(request);
            }
        }

        protected virtual IOperationHolder<DependencyTelemetry> StartOperation(
            RpcRequest request)
        {
            var operationHolder = _telemetryClient.StartOperation<DependencyTelemetry>(request.Method);

            operationHolder.Telemetry.Data = JsonConvert.SerializeObject(request);
            operationHolder.Telemetry.Target = _apiUrl.ToString();
            operationHolder.Telemetry.Type = _blockchainType.ToString();

            return operationHolder;
        }
    }
}
