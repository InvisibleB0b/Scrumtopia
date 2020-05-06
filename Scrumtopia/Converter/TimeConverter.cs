using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrumtopia.Converter
{
    class TimeConverter
    { 
        
        /// <summary>
        /// Converts a day and time to a datetíme
        /// </summary>
        /// <param name="date"> Must be a type of date time offset</param>
        /// <param name="time">must be of type timespan</param>
        /// <returns></returns>
        public static DateTime ConverterToDateTime(DateTimeOffset date, TimeSpan time) => new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);// Bruges til oprettelse af nyt event, så databasen kan bruge værdierne


        public static DateTimeOffset ConvertToDate(DateTime dateTime) => new DateTimeOffset(dateTime);
        public static TimeSpan ConvertToTime(DateTime dateTime) => new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}
