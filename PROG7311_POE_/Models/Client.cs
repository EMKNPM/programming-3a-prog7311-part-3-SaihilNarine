using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace PROG7311_POE_.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
