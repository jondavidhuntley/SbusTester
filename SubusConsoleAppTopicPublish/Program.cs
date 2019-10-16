using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using SvbusDomain.Model;
using System;
using System.Text;

namespace SubusConsoleAppTopicPublish
{
    public class Program
    {       
        public static void Main(string[] args)
        {
            ServiceInitializer.Initialize();

            string connectionString = Environment.GetEnvironmentVariable("SbusConnString");
            string topic = Environment.GetEnvironmentVariable("Topic");
            string payload = string.Empty;

            var builder = new ServiceBusConnectionStringBuilder(connectionString);

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                builder.SasKeyName,
                builder.SasKey);

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Publish Notification");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("1. AFR - 2017");
            Console.WriteLine("2. AFR - 2018");
            Console.WriteLine("3. AFR - 2019");
            Console.WriteLine("4. TST - 2017");
            Console.WriteLine("5. TST - 2018");
            Console.WriteLine("6. TST - 2019");
            Console.WriteLine("-------------------------------------------------------");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        payload = GetSecondaryReportNotificationPayload("AFR", 2017);
                        break;
                    case "2":
                        payload = GetSecondaryReportNotificationPayload("AFR", 2018);
                        break;
                    case "3":
                        payload = GetSecondaryReportNotificationPayload("AFR", 2019);
                        break;
                    case "4":
                        payload = GetSecondaryReportNotificationPayload("TST", 2017);
                        break;
                    case "5":
                        payload = GetSecondaryReportNotificationPayload("TST", 2018);
                        break;
                    case "6":
                        payload = GetSecondaryReportNotificationPayload("TST", 2019);
                        break;
                    default:
                        break;
                }

                var message = new Message(Encoding.UTF8.GetBytes(payload));
                Console.WriteLine($"Message Published: {payload}");

                var sender = new MessageSender(
                    builder.Endpoint,
                    topic,
                    tokenProvider,
                    TransportType.AmqpWebSockets);

                sender.SendAsync(message).GetAwaiter().GetResult();
            }                      
        }

        private static string GetSecondaryReportNotificationPayload(string airline, int year, string content = "")
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(content))
            {
                content = "Secondary Report Generation Notification!";
            }

            var notification = new ReportNotification
            {
                AirlineICAOCode = airline,
                Currency = "USD",
                ReportDateUTC = new DateTime(year, 1, 1),
                ReportingPeriod = "Annual",
                Message = content
            };
            
            message = Newtonsoft.Json.JsonConvert.SerializeObject(notification);

            return message;
        }
    }
}