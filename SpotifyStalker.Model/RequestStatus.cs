namespace SpotifyStalker.Model;

public enum RequestStatus
{
    [Obsolete]
    Default,
    Success,
    NotFound,
    Retry,
    Failed
}
