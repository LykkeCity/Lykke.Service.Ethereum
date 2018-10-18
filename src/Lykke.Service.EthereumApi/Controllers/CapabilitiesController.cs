using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EthereumApi.Controllers
{
    [PublicAPI, Route("api/capabilities")]
    public class CapabilitiesController : Controller
    {
        private static readonly CapabilitiesResponse CapabilitiesResponse =
            new CapabilitiesResponse
            {
                IsTransactionsRebuildingSupported = false,
                AreManyInputsSupported = false,
                AreManyOutputsSupported = false,
                IsTestingTransfersSupported = false,
                IsPublicAddressExtensionRequired = false,
                IsReceiveTransactionRequired = false,
                CanReturnExplorerUrl = false,
                IsAddressMappingRequired = false,
                IsExclusiveWithdrawalsRequired = false
            };

        
        [HttpGet]
        public ActionResult<CapabilitiesResponse> Get()
        {
            return CapabilitiesResponse;
        }
    }
}
