﻿using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(
            [NotNull] Transaction transaction);

        [ItemCanBeNull]
        Task<Transaction> TryGetAsync(
            Guid transactionId);

        Task UpdateAsync(
            [NotNull] Transaction transaction);
    }
}
