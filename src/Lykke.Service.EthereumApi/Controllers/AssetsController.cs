using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract;
using Lykke.Service.BlockchainApi.Contract.Assets;
using Lykke.Service.Ethereum.Domain;
using Lykke.Service.Ethereum.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Ethereum.Controllers
{
    [PublicAPI, Route("/api/assets")]
    public class AssetsController : Controller
    {
        private static readonly AssetResponse AssetResponse;
        private static readonly PaginationResponse<AssetContract> AssetsResponse;

        static AssetsController()
        {
            AssetResponse = new AssetResponse
            {
                Accuracy = Constants.AssetAccuracy,
                AssetId = Constants.AssetId,
                Name = Constants.AssetId
            };
            
            AssetsResponse = new PaginationResponse<AssetContract>
            {
                Continuation = null,
                Items = new [] { AssetResponse }
            };
        }
        
        [HttpGet("{assetId}")]
        public ActionResult<AssetResponse> GetAsset(
            AssetRequest request)
        {
            if (request.AssetId == AssetResponse.AssetId)
            {
                return AssetResponse;
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpGet]
        public ActionResult<PaginationResponse<AssetContract>> GetAssetList(
            PaginationRequest request)
        {
            return Ok(AssetsResponse);
        }
    }
}
