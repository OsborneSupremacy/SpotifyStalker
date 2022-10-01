namespace SpotifyStalker.Model;

public class CategoryViewModel<T> where T : ISpotifyStandardObject
{
    public ConcurrentDictionary<string, T> Items;

    public CategoryViewModel()
    {
        Items = new ConcurrentDictionary<string, T>();
        InProcess = false;
        Processed = 0;
    }

    public List<T> GetItems() =>
        Items.Values.ToList();

    public bool InProcess { get; set; }

    public int Total => Items.Count();

    public bool Display => Items.Any();

    public string ProgressDisplay => $"{(Processed == 0 ? 0.0 : (Processed / (double)Total) * 100.0)}%";

    public int Processed { get; set; }

    public bool TryAdd(string id, T value) => Items.TryAdd(id, value);

}
