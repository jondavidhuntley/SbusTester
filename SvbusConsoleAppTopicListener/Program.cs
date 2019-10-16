using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;
using Newtonsoft.Json;
using SvbusDomain.Model;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvbusConsoleAppTopicListener
{
    public class Program
    {
        private static ISubscriptionClient _subscriptionClient;

        public static void Main(string[] args)
        {
            Console.WriteLine("Listening....");
            ServiceInitializer.Initialize();

            string connectionString = Environment.GetEnvironmentVariable("SbusConnString");
            string topic = Environment.GetEnvironmentVariable("Topic");
            string subscription = Environment.GetEnvironmentVariable("Subscription");

            var builder = new ServiceBusConnectionStringBuilder(connectionString);

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                builder.SasKeyName,
                builder.SasKey);

            try
            {
                _subscriptionClient = new SubscriptionClient(builder.Endpoint, topic, subscription, tokenProvider, TransportType.AmqpWebSockets, ReceiveMode.PeekLock);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };

                _subscriptionClient.RegisterMessageHandler(ReceiveMessagesAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();

                _subscriptionClient.CloseAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            var payload = Encoding.UTF8.GetString(message.Body);

            if (!string.IsNullOrEmpty(payload))
            {
                var notification = JsonConvert.DeserializeObject<ReportNotification>(payload);

                if (notification != null)
                {
                    Console.WriteLine($"Subscribed message for Airline: {notification.AirlineICAOCode} Message: {notification.Message} Created: {notification.Created}");
                }
            }

            _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionReceivedEventArgs"></param>
        /// <returns></returns>
        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }        
    }
}