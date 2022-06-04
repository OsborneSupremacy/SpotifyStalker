using System;
using SpotifyStalker.Interface;

namespace SpotifyStalker.Service;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentDateTime() => DateTime.Now;
}
