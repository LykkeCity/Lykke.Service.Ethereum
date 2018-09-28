using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Transactions;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class BuildSingleTransactionRequestValidator : AbstractValidator<BuildSingleTransactionRequest>
    {
        public BuildSingleTransactionRequestValidator()
        {
            RuleFor(x => x.Amount)
                .AmountMustBeValid();

            RuleFor(x => x.FromAddress)
                .AddressMustBeValid();
            
            RuleFor(x => x.OperationId)
                .TransactionIdMustBeNonEmptyGuid();

            RuleFor(x => x.ToAddress)
                .AddressMustBeValid();
            
            RuleFor(x => x.AssetId)
                .AssetMustBeSupported();
        }
    }
}
