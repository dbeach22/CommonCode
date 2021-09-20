using FatHead.Enums;
using System;

namespace FatHead.Loggers.Interfaces
{
    public interface ILog
    {
        ErrorCode ErrorCode { get; set; }

        DateTime LogTime { get; set; }

        string Message { get; set; }
    }
}
