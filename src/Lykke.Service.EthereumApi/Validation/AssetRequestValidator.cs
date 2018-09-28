using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Models;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class AssetRequestValidator : AbstractValidator<AssetRequest>
    {
        public AssetRequestValidator()
        {
            RuleFor(x => x.AssetId)
                .AssetMustBeSupported();
        }
    }
}
