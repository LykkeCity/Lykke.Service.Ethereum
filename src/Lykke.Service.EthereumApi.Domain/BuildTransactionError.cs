namespace Lykke.Service.Ethereum.Domain
{
    public enum BuildTransactionError
    {
        AmountIsTooSmall,
        BalanceIsNotEnough,
        TransactionHasBeenBroadcasted,
        TransactionHasBeenDeleted,
    }
}
