using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Models;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class OperationRequestValidator : AbstractValidator<TransactionRequest>
    {
        public OperationRequestValidator()
        {
            RuleFor(x => x.TransactionId)
                .TransactionIdMustBeNonEmptyGuid();
        }
    }
}
