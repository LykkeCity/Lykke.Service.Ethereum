using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Transactions;
using Lykke.Service.Ethereum.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Ethereum.Controllers
{
    [PublicAPI, Route("api/sign")]
    public class SignController : Controller
    {
        private readonly ISignService _signService;

        public SignController(
            ISignService signService)
        {
            _signService = signService;
        }
        

        [HttpPost]
        public ActionResult<SignedTransactionResponse> SignAsync(
            [FromBody] SignTransactionRequest signRequest)
        {
            var signedTransaction = _signService.SignTransaction
            (
                transactionContext: signRequest.TransactionContext,
                privateKey: signRequest.PrivateKeys[0]
            );

            return new SignedTransactionResponse
            {
                SignedTransaction = signedTransaction
            };
        }
    }
}
