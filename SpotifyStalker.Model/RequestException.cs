using System;

namespace SpotifyStalker.Model;

public class RequestException : Exception
{
    public RequestStatus RequestStatus { get; set; }

    public bool Retry { get; set; }

    public double? WaitMs { get; set; }

    public RequestException(RequestStatus requestStatus)
    {
        RequestStatus = requestStatus;
        Retry = false;
        WaitMs = null;
    }

    public RequestException(RequestStatus requestStatus, bool retry, double waitMs)
    {
        RequestStatus = requestStatus;
        Retry = retry;
        WaitMs = waitMs;
    }
}
