using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SignApiSettings
    {
        public DbSettings Db { get; set; }
    }
}
