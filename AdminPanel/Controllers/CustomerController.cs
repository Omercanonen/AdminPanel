using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AdminPanel.Models;
using AdminPanel.Models.Customer;
using AdminPanel.Models.Employees;

namespace AdminPanel.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CustomerPage(string sortOrder)
        {
            ViewData["CustomerIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "CustomerId_desc" : "";
            ViewData["CustomerNameSortParam"] = sortOrder == "CustomerName" ? "CustomerName_desc" : "CustomerName";
            ViewData["CustomerSurnameSortParam"] = sortOrder == "CustomerSurname" ? "CustomerSurname_desc" : "CustomerSurname";
            ViewData["CustPhoneNumberSortParam"] = sortOrder == "CustPhoneNumber" ? "CustPhoneNumber_desc" : "CustPhoneNumber";
            ViewData["CustEmailSortParam"] = sortOrder == "CustEmail" ? "CustEmail_desc" : "CustEmail";

            var customers = from c in _context.Customers select c;

            switch(sortOrder)
            {
                case "CustomerId_desc":
                    customers = customers.OrderByDescending(c => c.CustomerId);
                    break;
                case "CustomerName":
                    customers = customers.OrderBy(c => c.CustomerName);
                    break;
                case "CustomerName_desc":
                    customers = customers.OrderByDescending(c => c.CustomerName);
                    break;
                case "CustomerSurname":
                    customers = customers.OrderBy(c => c.CustomerSurname);
                    break;
                case "CustomerSurname_desc":
                    customers = customers.OrderByDescending(c => c.CustomerSurname);
                    break;
                case "CustPhoneNumber":
                    customers = customers.OrderBy(c => c.CustPhoneNumber);
                    break;
                case "CustPhoneNumber_desc":
                    customers = customers.OrderByDescending(c => c.CustPhoneNumber);
                    break;
                case "CustEmail":
                    customers = customers.OrderBy(c => c.CustEmail);
                    break;
                case "CustEmail_desc":
                    customers = customers.OrderByDescending(c => c.CustEmail);
                    break;

            }

            return View(await customers.ToListAsync()); 
        }
        [HttpGet]
        public async Task<IActionResult> CustomerEdit(int Id)
        {
            var customer = await _context.Customers.FindAsync(Id);

            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> CustomerEdit(CustomerEntity viewModel)
        {
            var customer = await _context.Customers.FindAsync(viewModel.CustomerId);

            if (customer != null)
            {
                customer.CustomerId = viewModel.CustomerId;
                customer.CustomerName = viewModel.CustomerName;
                customer.CustomerSurname = viewModel.CustomerSurname;
                customer.CustPhoneNumber = viewModel.CustPhoneNumber;
                customer.CustEmail = viewModel.CustEmail;
                customer.CustAddress = viewModel.CustAddress;
                customer.CustTaxNo = viewModel.CustTaxNo;
                customer.CustTaxOffice = viewModel.CustTaxOffice;
                customer.CustTitle = viewModel.CustTitle;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            return View(viewModel);
        }
        public async Task<IActionResult> CustomerDelete(int Id)
        {
            var customer = await _context.Customers.FindAsync(Id);
            _context.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("CustomerPage");
        }

        [HttpGet]
        public async Task<IActionResult> CustomerCreate()
        {
            var customers = await _context.Customers.ToListAsync();
            ViewBag.Employees = new SelectList(customers, "CustomerId", "CustomerName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerCreate([Bind("CustomerId,CustomerName,CustomerSurname,CustPhoneNumber,CustEmail,CustAddress,CustTaxNo,CustTaxOffice,CustTitle")] CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kayıt işlemi başarıyla tamamlandı.";
                    return RedirectToAction("CustomerCreate");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Bir hata oluştu, lütfen tekrar deneyin.");
                }
            }

            var errors = ModelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(customer);

        }
    }
}
