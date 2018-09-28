using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Models;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class TransactionHistoryRequestValidator : AbstractValidator<TransactionHistoryRequest>
    {
        public TransactionHistoryRequestValidator()
        {
            RuleFor(x => x.Address)
                .AddressMustBeValid();

            RuleFor(x => x.Take)
                .GreaterThan(1);
        }
    }
}
