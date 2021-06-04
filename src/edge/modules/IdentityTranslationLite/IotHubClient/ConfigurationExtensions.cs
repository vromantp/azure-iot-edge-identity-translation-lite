using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityTranslationLite.IotHubClient
{
    public static class ConfigurationExtensions
    {
        public static ServiceCollection AddModuleClient(this ServiceCollection serviceCollection, ITransportSettings transportSettings)
        {
            serviceCollection.AddSingleton<IModuleClient>(sp => {
                ITransportSettings[] settings = { transportSettings };

                var ioTHubModuleClient = Microsoft.Azure.Devices.Client.ModuleClient.CreateFromEnvironmentAsync(settings).GetAwaiter().GetResult();
                return new ModuleClientAdapter(ioTHubModuleClient);
            });

            return serviceCollection;
        }
    }

}