using AdminPanel.Models.Service;
using System.ComponentModel.DataAnnotations;
namespace AdminPanel.Models.Products
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public ICollection<ServiceEntity>? Services { get; set; }
        //public ICollection<ProductModel>? ProductModels { get; set; }
        
    }
}
