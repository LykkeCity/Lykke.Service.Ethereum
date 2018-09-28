﻿using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Ethereum.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TransactionRequest
    {
        [FromRoute]
        public Guid TransactionId { get; set; }
    }
}
