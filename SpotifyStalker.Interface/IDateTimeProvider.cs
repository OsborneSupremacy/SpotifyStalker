using System;

namespace SpotifyStalker.Interface;

public interface IDateTimeProvider
{
    DateTime GetCurrentDateTime();
}
