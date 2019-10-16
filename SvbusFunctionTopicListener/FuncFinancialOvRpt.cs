using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SvbusDomain.Model;

namespace SvbusFunctionTopicListener
{
    public static class FuncFinancialOvRpt
    {
        [FunctionName("FuncFinancialOvRpt")]
        public static void Run([ServiceBusTrigger("taa_sec_rpt_notification_sbus_topic", 
            "taa_sbus_subscription_rpt_finanical_overview", Connection = "SBusConnection")]Message sbusMsg, ILogger log)        
        {
            if (sbusMsg != null && sbusMsg.Body != null)
            {
                var messageBody = Encoding.UTF8.GetString(sbusMsg.Body);

                if (!string.IsNullOrEmpty(messageBody))
                {
                    var notification = JsonConvert.DeserializeObject<ReportNotification>(messageBody);

                    log.LogInformation($"C# ServiceBus topic trigger function processed message for Airline: {notification.AirlineICAOCode}");
                }
            }
        }
    }
}
