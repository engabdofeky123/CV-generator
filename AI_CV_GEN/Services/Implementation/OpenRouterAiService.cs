using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CV_Generator.Services.Interfaces;
using CV_Generator.ViewModels;
using Microsoft.Extensions.Options;

namespace CV_Generator.Services.Implementation;

public sealed class OpenRouterAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;
    private readonly IGeneratePromptService _promptService;
    private readonly ILogger<OpenRouterAiService> _logger;

    public OpenRouterAiService(
        HttpClient httpClient,
        IOptions<AiOptions> options,
        IGeneratePromptService promptService,
        ILogger<OpenRouterAiService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _promptService = promptService;
        _logger = logger;
    }

    public async Task<OptimizedCv> GenerateCvAsync(
        CVFormSubmit cv,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException(
                "AI API key is not configured. " +
                "Set Ai:ApiKey or OPENROUTER_API_KEY.");
        }

        var prompt = _promptService.GeneratePrompt(cv);

        var payload = new
        {
            model = _options.Model,
            temperature = 0.2,

            response_format = new
            {
                type = "json_object"
            },

            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        """
                        You are a professional CV writer.

                        Your task is to rewrite and optimize the candidate's
                        CV while preserving ALL provided information.

                        You MUST return ONLY valid JSON.

                        NEVER remove information that exists in the candidate data.

                        If the candidate provides experience, projects, education,
                        skills, or certifications, preserve them in the corresponding
                        JSON arrays.

                        Do not invent information.
                        """
                },

                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "chat/completions");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _options.ApiKey);

        if (!string.IsNullOrWhiteSpace(_options.SiteUrl))
        {
            request.Headers.Add(
                "HTTP-Referer",
                _options.SiteUrl);
        }

        if (!string.IsNullOrWhiteSpace(_options.SiteName))
        {
            request.Headers.Add(
                "X-Title",
                _options.SiteName);
        }

        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "OpenRouter request failed with {StatusCode}: {Body}",
                response.StatusCode,
                body);

            throw new HttpRequestException(
                $"OpenRouter returned {(int)response.StatusCode}: {body}");
        }

        using var document = JsonDocument.Parse(body);

        var content =
            document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(
                "OpenRouter returned an empty response.");
        }

        content = CleanJson(content);

        var result = JsonSerializer.Deserialize<OptimizedCv>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


        if (result == null)
        {
            throw new InvalidOperationException(
                "OpenRouter returned an invalid CV response.");
        }

        return result;
    }

    private static string CleanJson(string content)
    {
        content = content.Trim();

        var start = content.IndexOf('{');
        var end = content.LastIndexOf('}');

        if (start >= 0 && end > start)
        {
            return content[start..(end + 1)];
        }

        return content;
    }
}