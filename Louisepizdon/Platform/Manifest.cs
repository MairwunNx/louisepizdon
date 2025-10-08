namespace Louisepizdon.Platform;

public static class Manifest
{
    public static string Version { get; private set; } = "0.0.1";
    public static string BuildTime { get; private set; } = "1970-01-01";
    public static DateTime StartTime { get; private set; } = DateTime.UtcNow;

    public static void SetManifest(string version, string buildTime)
    {
        Version = version;
        BuildTime = buildTime;
        StartTime = DateTime.UtcNow;
    }
}