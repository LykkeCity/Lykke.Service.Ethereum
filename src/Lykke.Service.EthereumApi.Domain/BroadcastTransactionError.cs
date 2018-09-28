namespace Lykke.Service.Ethereum.Domain
{
    public enum BroadcastTransactionError
    {
        AmountIsTooSmall,
        BalanceIsNotEnough,
        TransactionHasBeenBroadcasted,
        TransactionHasBeenDeleted,
        TransactionShouldBeRebuilt,
        OperationHasNotBeenFound
    }
}
