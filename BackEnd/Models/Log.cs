using System;

namespace BackEnd.Models
{
    public class Log
    {
        public long Id { get; set; }
        public string IPAddress { get; set; }
        public DateTime LogDate { get; set; }
        public string LogMessage { get; set; }
    }
}