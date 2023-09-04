using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Establishment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
    }
}
