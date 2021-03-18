namespace Common
{
    using System;

    public static class DateTimeHelper
    {
        /// <summary>
		/// Method to convert input datetime in format "yyyy-MM-dd HH:mm:ss"
		/// </summary>
		/// <param name="dateTime">Input datetime to be converted</param>
		/// <returns>Formatted (yyyy-MM-dd HH:mm:ss) string</returns>
		public static string To_yyyy_MM_dd_HH_mm_ss(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm:ss}";
        }

        /// <summary>
        /// Method to get date timestamp
        /// </summary>
        /// <param name="dateTime">Input datetime to be converted</param>
        /// <returns>Formatted timestamp string</returns>
        public static string GetTimeStamp(this DateTime dateTime)
        {
            return $"{dateTime:yyyyMMddHHmmssffff}";
        }

        /// <summary>
        /// Method to convert input datetime in format "yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">Input datetime to be converted</param>
        /// <returns>Formatted (yyyy-MM-dd) string</returns>
        public static string To_yyyy_MM_dd(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd}";
        }
    }
}
