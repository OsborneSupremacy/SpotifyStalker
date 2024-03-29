﻿namespace SpotifyStalker.ConsoleUi;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class UserPromptService
{
    private readonly ArtistQueryService _artistQueryService;

    private readonly TrackQueryService _trackQueryService;

    private readonly Dictionary<int, (string name, Func<Task> operation)> _operations;

    public UserPromptService(
        ArtistQueryService artistQueryService,
        TrackQueryService trackQueryService
    )
    {
        _artistQueryService = artistQueryService ?? throw new ArgumentNullException(nameof(artistQueryService));
        _trackQueryService = trackQueryService ?? throw new ArgumentNullException(nameof(trackQueryService));
        _operations =
            new()
            {
                { 1, new("Query Artists", _artistQueryService.ExecuteAsync) },
                { 2, new("Query Tracks", _trackQueryService.ExecuteAsync) },
                { 0, ("Exit", null) }
            };
    }

    public async Task<bool> PromptUserAsync()
    {
        var (isValid, name, operation) = SelectOperation();
        if (!isValid || operation == null) return false;

        Console.WriteLine($"Selected operation: {name}");
        Console.WriteLine();

        await operation.Invoke();

        return true; // since we're returning true, the host will run this again
    }

    public (bool isValid, string name, Func<Task> operation) SelectOperation()
    {
        StringBuilder s = new();
        s.AppendLine();
        s.AppendLine("SELECT AN OPERATION:");

        foreach (var op in _operations)
        {
            if (op.Key == 0)
            {
                s.AppendLine($"0. (Or Any Other Key) {op.Value.name}");
                continue;
            }
            s.AppendLine($"{op.Key}. {op.Value.name}");
        }

        Console.Write(s.ToString());

        var i = Console.ReadKey();
        Console.WriteLine();

        i.KeyChar.ToString();

        if (!int.TryParse(i.KeyChar.ToString(), out var input)) return inValid();

        if (_operations.TryGetValue(input, out var foundOp))
            return (true, foundOp.name, foundOp.operation);

        return inValid();

        static (bool, string, Func<Task>) inValid() => (false, null, null);
    }
}
