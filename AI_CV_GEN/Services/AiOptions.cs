namespace CV_Generator.Services;

public sealed class AiOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string Model { get; set; } = "openai/gpt-4o-mini";
    public string SiteUrl { get; set; } = "http://localhost:5000";
    public string SiteName { get; set; } = "CV Generator AI";
}
