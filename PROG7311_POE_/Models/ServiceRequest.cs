using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE_.Models
{
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestID { get; set; }
        public int ContractID { get; set; }
        public Contract Contract { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }
}
