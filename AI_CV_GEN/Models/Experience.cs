namespace CV_Generator.Models
{
    public class Experience
    {
        public string Role { get; set; }

        public string Company { get; set; }

        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; } 
        public string Description { get; set; }
    }
}
 