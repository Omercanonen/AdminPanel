using AdminPanel.Models.Customer;
using AdminPanel.Models.Employees;
using AdminPanel.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace AdminPanel.Models.Service
{
    public class ServiceEntity
    {
        [Key]
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string Model { get; set; }
        public int EmployeeId { get; set; }
        public string SeriNo { get; set; }
        public bool Warranty { get; set; }
        public string Complaint {  get; set; }
        public string PerformedActions { get; set; }
        public int PartsCost { get; set; }
        public int ServiceCost { get; set; }
        public int TotalCost { get; set; }
        public string Description { get; set; }
        public bool PaymentStatus { get; set; }
        public bool DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        public CustomerEntity? Customer { get; set; }
        
        public Product? Product { get; set; }
        //public ProductModel? ProductModel { get; set; }
        
        public Employee? Employee { get; set; }
    }
}
