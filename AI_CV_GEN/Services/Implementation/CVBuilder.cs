using CV_Generator.Models;
using CV_Generator.ViewModels;

namespace CV_Generator.Services.Implementation;

public sealed class CVBuilder
{
    public AiCvResult Build( CVFormSubmit source, OptimizedCv? generated)
    {
        generated ??= new OptimizedCv();

        var result = new AiCvResult
        {
            OptimizedCV = generated
        };

        // =========================
        // CONTACT
        // =========================

        generated.FullName =
            First(generated.FullName, source.Name);

        generated.Contact ??= new ContactDetails();

        generated.Contact.Email =
            First(generated.Contact.Email, source.Email);

        generated.Contact.Phone =
            First(generated.Contact.Phone, source.Phone);

        generated.Contact.Address =
            First(generated.Contact.Address, source.Address);

        generated.Contact.LinkedIn =
            First(generated.Contact.LinkedIn, source.LinkedIn);


        // =========================
        // SKILLS
        // =========================

        if (generated.Skills == null ||
            generated.Skills.Count == 0)
        {
            generated.Skills = source.Skills ?? new List<string>();
        }


        // =========================
        // EXPERIENCE
        // =========================

        if (generated.Experience == null ||
            generated.Experience.Count == 0)
        {
            generated.Experience =
                source.Experience?
                    .Select(MapExperience)
                    .ToList()
                ?? new List<OptimizedExperience>();
        }


        // =========================
        // PROJECTS
        // =========================

        if (generated.Projects == null ||
            generated.Projects.Count == 0)
        {
            generated.Projects =
                source.Projects?
                    .Select(MapProject)
                    .ToList()
                ?? new List<OptimizedProject>();
        }


        // =========================
        // EDUCATION
        // =========================

        if (generated.Education == null ||
            generated.Education.Count == 0)
        {
            generated.Education =
                BuildEducation(source);
        }


        // =========================
        // CERTIFICATIONS
        // =========================

        if (generated.Certifications == null)
        {
            generated.Certifications =
                new List<string>();
        }


        return result;
    }


    // =========================
    // EXPERIENCE MAPPING
    // =========================

    private static OptimizedExperience MapExperience(
        Experience experience)
    {
        return new OptimizedExperience
        {
            JobTitle = experience.Role,
            Company = experience.Company,

            Duration = FormatDates(
                experience.StartDate,
                experience.EndDate),

            Responsibilities =
                SplitDescription(
                    experience.Description)
        };
    }


    // =========================
    // PROJECT MAPPING
    // =========================

    private static OptimizedProject MapProject(
        Project project)
    {
        return new OptimizedProject
        {
            Name = project.Name,

            Description = project.Description,

            Url = project.GitHubLink,

            Technologies =
                (project.Technologies ?? string.Empty)
                    .Split(
                        ',',
                        StringSplitOptions.TrimEntries |
                        StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
        };
    }


    // =========================
    // EDUCATION
    // =========================

    private static List<OptimizedEducation> BuildEducation(
        CVFormSubmit source)
    {
        if (source.Education == null)
        {
            return new List<OptimizedEducation>();
        }

        if (string.IsNullOrWhiteSpace(source.Education.Degree) &&
            string.IsNullOrWhiteSpace(source.Education.University) &&
            string.IsNullOrWhiteSpace(source.Education.College) &&
            string.IsNullOrWhiteSpace(source.Education.GraduationYear))
        {
            return new List<OptimizedEducation>();
        }

        return new List<OptimizedEducation>
        {
            new OptimizedEducation
            {
                Degree = source.Education.Degree,

                Institution =
                    First(
                        source.Education.University,
                        source.Education.College),

                Year =
                    source.Education.GraduationYear
            }
        };
    }


    // =========================
    // HELPERS
    // =========================

    private static List<string> SplitDescription(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new List<string>();
        }

        return value
            .Split(
                new[] { '\n', '•' },
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }


    private static string FormatDates( DateOnly start, DateOnly end)
    {
        if (start == default)
        {
            return string.Empty;
        }

        return
            $"{start:MMM yyyy} – " +
            $"{(end == default
                ? "Present"
                : end.ToString("MMM yyyy"))}";
    }


    private static string First( string? value,string? fallback)
    {
        return string.IsNullOrWhiteSpace(value)
            ? fallback ?? string.Empty
            : value;
    }
}