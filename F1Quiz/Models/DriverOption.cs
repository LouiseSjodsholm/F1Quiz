using Microsoft.EntityFrameworkCore;

namespace F1Quiz.Models
{
    [Owned]
    public class DriverOption
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
