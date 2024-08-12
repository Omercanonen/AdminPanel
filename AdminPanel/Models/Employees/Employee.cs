using AdminPanel.Models.Service;
using System.ComponentModel.DataAnnotations;
namespace AdminPanel.Models.Employees
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string EmpSurname { get; set; }
        public string EmpPhoneNumber { get; set; }
        public string EmpTitle { get; set; }

        public ICollection<ServiceEntity>? Services { get; set; }

    }
}
