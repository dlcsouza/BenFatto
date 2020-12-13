using Microsoft.AspNetCore.Http;
using System;
using System.Collections;

namespace BackEnd.ViewModels
{
    public class LogViewModel
    {
        public long Id { get; set; }
        public string IPAddress { get; set; }
        public string LogDate { get; set; }
        public string LogMessage { get; set; }
        public IFormFile[] files{get;set;}
    }
}