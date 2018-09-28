using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Models;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class AddressRequestValidator : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidator()
        {
            RuleFor(x => x.Address)
                .AddressMustBeValid();
        }
    }
}
