using SpotifyStalker.Interface;
using System;

namespace SpotifyStalker.Service
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
