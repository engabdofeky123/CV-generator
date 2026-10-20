namespace CV_Generator.Models
{
    public class CV
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Location { get; set; }

        public string LinkedIn { get; set; }

        public string GitHub { get; set; }

        // AI Generated Summary
        public string ProfessionalSummary { get; set; }

        public List<string> Skills { get; set; }

        public List<string> Tools { get; set; }

        public List<Experience> Experiences { get; set; }

        public List<Project> Projects { get; set; }

        public List<Education> Educations { get; set; }
    }
}
