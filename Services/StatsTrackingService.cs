namespace SimpBot.Services;

public class StatsTrackingService
{
    public IDictionary<string, int> Stats { get; }

    private readonly object _lock = new();

    public StatsTrackingService()
    {
        Stats = new Dictionary<string, int>();
    }

    public void TrackUsage(string feature, int usages = 1)
    {
        lock (_lock)
        {
            if (!Stats.ContainsKey(feature))
            {
                Stats[feature] = 0;
            }
            
            Stats[feature] += usages;
        }
    }
}