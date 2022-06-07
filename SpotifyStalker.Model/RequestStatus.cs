
using System;

namespace SpotifyStalker.Model;

public enum RequestStatus
{
    [Obsolete]
    Default,
    [Obsolete]
    Success,
    NotFound,
    Retry,
    Failed
}
