using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PaginationRequest
    {
        public PaginationRequest()
        {
            Continuation = string.Empty;
        }

        public string Continuation { get; set; }

        public int Take { get; set; }
    }
}
