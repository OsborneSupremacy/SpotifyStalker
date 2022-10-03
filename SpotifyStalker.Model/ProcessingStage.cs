namespace SpotifyStalker.Model;

public record ProcessingStage
{
    public ProcessingStage(bool inProcess, string stage)
    {
        InProcess = inProcess;
        Stage = stage;
    }

    public bool InProcess { get; }

    public string Stage { get; }
}
