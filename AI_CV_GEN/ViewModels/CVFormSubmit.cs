using CV_Generator.Models;

namespace CV_Generator.ViewModels
{
    
        public class CVFormSubmit
        {
            // ── Personal Information ──
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string LinkedIn { get; set; } = string.Empty;

            // ── ATS Job Description ──
            public string DescriptionForATS { get; set; } = string.Empty;

            // ── Education (single object, bound directly from form fields) ──
            public Education Education { get; set; } = new();

            // ── Dynamic lists — serialized as JSON by JS, deserialized in controller ──
            public string SkillsJson { get; set; } = string.Empty;  // → List<string>
            public string ExperienceJson { get; set; } = string.Empty;  // → List<Experience>
            public string ProjectsJson { get; set; } = string.Empty;  // → List<Project>

            // ── Populated by controller after deserialization ──
            public List<string> Skills { get; set; } = new();
            public List<Experience> Experience { get; set; } = new();
            public List<Project> Projects { get; set; } = new();

            // ── Result returned from the server back to the view ──
            public string? GeneratedCV { get; set; }
        }

    }
