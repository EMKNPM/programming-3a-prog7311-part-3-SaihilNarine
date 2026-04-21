using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE_.Models
{
    public class Contract
    {
        [Key]
        public int ContractID { get; set; }
        public int ClientID { get; set; }
        public Client? Client { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        
        [Required]
        public string? Status { get; set; }

        [Required]
        public string? ServiceLevel { get; set; }
        public string? AgreementFile { get; set; }
        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}
