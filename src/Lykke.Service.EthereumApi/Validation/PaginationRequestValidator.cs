﻿using System;
using Common;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Validation
{
    [UsedImplicitly]
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.Take)
                .Must(take => take > 0)
                .WithMessage(x => "Take parameter must be greater than zero.");

            RuleFor(x => x.Continuation)
                .Must(ValidateContinuationToken)
                .WithMessage(x => "Continuation token is not valid.");
        }

        private static bool ValidateContinuationToken(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var decodedToken = token.StringToHex();

                    JsonConvert.DeserializeObject<TableContinuationToken>(decodedToken);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
