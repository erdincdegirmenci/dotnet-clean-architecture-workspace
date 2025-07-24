namespace Template.Config;

public class ProjectFeatureFlags
{
    public bool UseKafka { get; set; }
    public bool UseRedis { get; set; }
    public bool UseElastic { get; set; }
    public bool UseHangfire { get; set; }
    public bool UseQuartz { get; set; }
} 