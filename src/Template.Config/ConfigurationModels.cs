namespace Template.Config;

public class JwtConfig
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}

public class RedisConfig
{
    public string ConnectionString { get; set; } = string.Empty;
}

public class ElasticConfig
{
    public string Uri { get; set; } = string.Empty;
} 