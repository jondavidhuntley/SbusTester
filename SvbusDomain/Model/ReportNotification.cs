using System;

namespace SvbusDomain.Model
{
    public class ReportNotification
    {
        /// <summary>
        /// Constructs a Report Notification
        /// </summary>
        public ReportNotification()
        {
            Created = DateTime.Now;
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Airline ICAO Code
        /// </summary>
        public string AirlineICAOCode { get; set; }

        /// <summary>
        /// Reporting Period (Annual, Q1, Q2, Q3, Q4, H1, H2)
        /// </summary>
        public string ReportingPeriod { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year
        {
            get
            {
                return ReportDateUTC.Year;
            }
        }

        /// <summary>
        /// Previous year
        /// </summary>
        public int PreviousYear
        {
            get
            {
                return Year - 1;
            }
        }

        /// <summary>
        /// Currency Code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Report Type
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// Report Date
        /// </summary>
        public DateTime ReportDateUTC { get; set; }

        /// <summary>
        /// Created Data
        /// </summary>
        public DateTime Created { get; set; }
    }
}