using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Ethereum.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AssetRequest
    {
        [FromRoute]
        public string AssetId { get; set; }
    }
}
