using AdminPanel.Models.Service;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.Customer
{
    public class CustomerEntity
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustPhoneNumber { get; set; }
        public string CustEmail { get; set; }
        public string CustAddress { get; set; }
        public string CustTaxNo { get; set; }
        public string CustTaxOffice { get; set; }
        public string CustTitle { get; set; }
        public ICollection<ServiceEntity>? Services { get; set; }

    }
}
