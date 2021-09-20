using FatHead.Enums;
using FatHead.Loggers.Interfaces;
using System;

namespace FatHead.Loggers
{
    public class Log : ILog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorCode">FatHead.Enums.ErrorCode</param>
        /// <param name="logTime">System.DateTime</param>
        /// <param name="message">System.String</param>
        public Log(ErrorCode errorCode, DateTime logTime, string message)
        {
            ErrorCode = errorCode;
            LogTime = logTime;
            Message = message;
        }

        public ErrorCode ErrorCode { get; set; }
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
    }
}
