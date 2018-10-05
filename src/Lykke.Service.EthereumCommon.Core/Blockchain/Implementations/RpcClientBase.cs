using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.Exceptions;
using Lykke.Service.Ethereum.Core.Blockchain.Models;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public abstract class RpcClientBase
    {
        private readonly Uri _apiUrl;
        private readonly TimeSpan _connectionTimeout;
        private readonly IHttpClientFactory _httpClientFactory;

        protected RpcClientBase(
            Uri apiUrl,
            TimeSpan connectionTimeout,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _connectionTimeout = connectionTimeout;
            _httpClientFactory = httpClientFactory;
        }

        protected virtual async Task<RpcResponse> SendRpcRequestAsync(
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
        
        protected virtual Task<RpcResponse> SendRpcRequestWithTelemetryAsync(
            RpcRequest request)
        {
            
        }
    }
}
