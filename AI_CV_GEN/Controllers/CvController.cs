using System.Text.Json;
using CV_Generator.Services.Implementation;
using CV_Generator.Services.Interfaces;
using CV_Generator.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CV_Generator.Controllers;

public class CvController : Controller
{
    private readonly IAiService _aiService;
    private readonly CVBuilder _cvBuilder;
    private readonly ILogger<CvController> _logger;

    public CvController(
        IAiService aiService,
        CVBuilder cvBuilder,
        ILogger<CvController> logger)
    {
        _aiService = aiService;
        _cvBuilder = cvBuilder;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(
            "Index",
            new CVFormSubmit());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(
        CVFormSubmit model,
        CancellationToken cancellationToken)
    {
        DeserializeLists(model);

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        try
        {
            var generatedCv =
                await _aiService.GenerateCvAsync(
                    model,
                    cancellationToken);

            var result =
                _cvBuilder.Build(
                    model,
                    generatedCv);

            return View(
                "CvPreview",
                result);
        }
        catch (Exception ex)
            when (ex is HttpRequestException
                or InvalidOperationException
                or JsonException)
        {
            _logger.LogError(
                ex,
                "CV generation failed.");

            ModelState.AddModelError(
                string.Empty,
                "The AI service could not generate your CV. " +
                "Please verify the API settings and try again.");

            return View(
                "Index",
                model);
        }
    }

    private static void DeserializeLists(
        CVFormSubmit model)
    {
        var options =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        model.Skills =
            Deserialize(
                model.SkillsJson,
                new List<string>(),
                options);

        model.Experience =
            Deserialize(
                model.ExperienceJson,
                new List<Models.Experience>(),
                options);

        model.Projects =
            Deserialize(
                model.ProjectsJson,
                new List<Models.Project>(),
                options);
    }

    private static T Deserialize<T>(
        string? json,
        T fallback,
        JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return fallback;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(
                       json,
                       options)
                   ?? fallback;
        }
        catch (JsonException)
        {
            return fallback;
        }
    }
}