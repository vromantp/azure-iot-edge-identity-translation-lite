// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using IdentityTranslationLite.IotHubClient;

namespace IdentityTranslationLite
{
    using System;
    using System.Runtime.Loader;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Azure.Devices.Client;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            // Build the our IServiceProvider and set our static reference to it
            IServiceProvider sp = serviceCollection.BuildServiceProvider();
            // Initialize module
            sp.GetRequiredService<IdentityTranslationLiteModule>()
                .Init()
                .GetAwaiter()
                .GetResult();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }
        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddModuleClient(new AmqpTransportSettings(TransportType.Amqp_Tcp_Only));
            serviceCollection.AddSingleton<IDeviceRepository>(new MemoryDeviceRepository());
            serviceCollection.AddSingleton<IdentityTranslationLiteModule>();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}
