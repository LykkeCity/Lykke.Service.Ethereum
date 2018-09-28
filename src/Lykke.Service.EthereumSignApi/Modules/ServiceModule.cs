using Autofac;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Domain.Services;
using Lykke.Service.Ethereum.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Ethereum.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        // ReSharper disable once NotAccessedField.Local : Field reserved for future use
        private readonly IReloadingManager<AppSettings> _appSettings;

        
        public ServiceModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .RegisterType<SignService>()
                .As<ISignService>()
                .SingleInstance();

            builder
                .RegisterType<WalletService>()
                .As<IWalletService>()
                .SingleInstance();
        }
    }
}
