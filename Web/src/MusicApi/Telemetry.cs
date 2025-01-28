using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MusicApi;

public static class Telemetry
{
    public static readonly ActivitySource ActivitySource = new("MusicApi");

    public static void AddTags(this Activity? activity,
        params ReadOnlySpan<(string, object)> tags)
    {
        if (activity is null) return;
        foreach (var (key, value) in tags)
        {
            activity.AddTag(key, value);
        }
    }
}