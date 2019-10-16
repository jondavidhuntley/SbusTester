using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SubusConsoleAppTopicPublish
{
    public static class ServiceInitializer
    {        
        private static IConfiguration _configuration;
        
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        public static IServiceCollection Services { get; private set; }

        /// <summary>
        /// Initialize Service Collection
        /// </summary>
        public static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            Services = new ServiceCollection()
                .AddLogging()
                .AddLogging(bdr =>
                {
                    bdr.SetMinimumLevel(LogLevel.Trace);
                    bdr.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                });            
        }
    }
}