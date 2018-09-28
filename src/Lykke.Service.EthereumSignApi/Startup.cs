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
        public void Configure(IApplicationBuilder app)
        {   
            app.UseLykkeConfiguration();
        }

        public IServiceProvider ConfigureServices(
            IServiceCollection services)
        {
            return services.BuildEthereumServiceProvider<AppSettings>
            (
                serviceName: "SignApi",
                
                #if ENABLE_SENSITIVE_LOGGING
                
                enableLogging: true,
                
                #else

                enableLogging: false,

                #endif
                
                logsConnString: settings => settings.SignApiService.Db.LogsConnString
            );
        }
    }
}
