using System;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Ethereum.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Ethereum
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        public void Configure(
            IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();
        }

        public IServiceProvider ConfigureServices(
            IServiceCollection services)
        {
            return services.BuildEthereumServiceProvider<AppSettings>
            (
                serviceName: "Api",
                enableLogging: true,
                logsConnString: settings => settings.ApiService.Db.LogsConnString
            );
        }
    }
}
