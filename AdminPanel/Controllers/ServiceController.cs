using AdminPanel.Models;
using AdminPanel.Models.Employees;
using AdminPanel.Models.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ServicePage(string sortOrder, string serviceSearchString)
        {
            ViewData["ServiceIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "ServiceId_desc" : "";
            ViewData["CustomerIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "CustomerId_desc" : "";
            ViewData["EmployeeIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "EmployeeId_desc" : "";
            ViewData["ProductIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "ProductId_desc" : "";
            ViewData["ModelSortParam"] = sortOrder == "Model" ? "Model_desc" : "Model";
            ViewData["SeriNoSortParam"] = sortOrder == "SeriNo" ? "SeriNo_desc" : "SeriNo";
            ViewData["PartsCostSortParam"] = sortOrder == "PartsCost" ? "PartsCost_desc" : "PartsCost";
            ViewData["ServiceCostSortParam"] = sortOrder == "ServiceCost" ? "ServiceCost_desc" : "ServiceCost";
            ViewData["TotalCostSortParam"] = sortOrder == "TotalCost" ? "TotalCost_desc" : "TotalCost";

            var services = _context.Services
                .Include(s => s.Customer)
                .Include(s => s.Product)
                .Include(s => s.Employee)
                .Where(s => s.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(serviceSearchString))
            {
                services = services.Where(s =>
                    s.Customer.CustomerName.Contains(serviceSearchString) ||
                    s.Employee.EmpName.Contains(serviceSearchString) ||
                    s.Product.ProductName.Contains(serviceSearchString) ||
                    s.Model.Contains(serviceSearchString));
            }



            switch (sortOrder)
            {
                case "ServiceId_desc":
                    services = services.OrderByDescending(s => s.ServiceId);
                    break;
                case "CustomerId_desc":
                    services = services.OrderByDescending(s => s.Customer.CustomerName);
                    break;
                case "EmployeeId_desc":
                    services = services.OrderByDescending(s => s.Employee.EmpName);
                    break;
                case "ProductId_desc":
                    services = services.OrderByDescending(s => s.Product.ProductName);
                    break;
                case "Model":
                    services = services.OrderBy(s => s.Model);
                    break;
                case "Model_desc":
                    services = services.OrderByDescending(s => s.Model);
                    break;
                case "SeriNo":
                    services = services.OrderBy(s => s.SeriNo);
                    break;
                case "SeriNo_desc":
                    services = services.OrderByDescending(s => s.SeriNo);
                    break;
                case "PartsCost":
                    services = services.OrderBy(s => s.PartsCost);
                    break;
                case "PartsCost_desc":
                    services = services.OrderByDescending(s => s.PartsCost);
                    break;
                case "ServiceCost":
                    services = services.OrderBy(s => s.ServiceCost);
                    break;
                case "ServiceCost_desc":
                    services = services.OrderByDescending(s => s.ServiceCost);
                    break;
                case "TotalCost":
                    services = services.OrderBy(s => s.TotalCost);
                    break;
                case "TotalCost_desc":
                    services = services.OrderByDescending(s => s.TotalCost);
                    break;
                default:
                    services = services.OrderBy(s => s.ServiceId);
                    break;
            }

            return View(await services.ToListAsync());
        }

        public async Task<IActionResult> ServiceDelete(int Id)
        {
            var service = await _context.Services.FindAsync(Id);
            if (service == null)
            {
                return NotFound();
            }

            service.IsActive = false;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            return RedirectToAction("ServicePage");
        }

        [HttpGet]
        public async Task<IActionResult> ServiceCreate()
        {
            var customers = await _context.Customers.ToListAsync();
            var products = await _context.Products.ToListAsync();
            //var models = await _context.ProductModels.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            
            ViewBag.CustomerList = new SelectList(customers, "CustomerId", "CustomerName");
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
            //ViewBag.ProductModelList = new SelectList(models, "ModelId", "ModelName");
            ViewBag.EmployeeList = new SelectList(employees, "EmployeeId", "EmpName");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCreate([Bind("ServiceId,CustomerId,ProductId,Model,EmployeeId,SeriNo,Warranty,Complaint,PerformedActions,PartsCost,ServiceCost,TotalCost,Description,PaymentStatus,DeliveryStatus,DeliveryDate")] ServiceEntity service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Services.Add(service);
                    TempData["SuccessMessage"] = "Kayıt Başarıyla Oluşturuldu.";
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ServiceCreate");
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

            var customers = await _context.Customers.ToListAsync();
            var products = await _context.Products.ToListAsync();
            //var models = await _context.ProductModels.ToListAsync();
            var employees = await _context.Employees.ToListAsync();



            ViewBag.CustomerList = new SelectList(customers, "CustomerId", "CustomerName");
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
            //ViewBag.ProductModelList = new SelectList(models, "ModelId", "ModelName");
            ViewBag.EmployeeList = new SelectList(employees, "EmployeeId", "EmpName");

            return View(service);
        }

        [HttpGet]
        public async Task<IActionResult> ServiceEdit(int id)
        {
            var service = _context.Services.Include(s => s.Customer)
                                       .Include(s => s.Product)
                                       .Include(s => s.Employee)
                                       .FirstOrDefault(s => s.ServiceId == id);

            if (service == null)
            {
                return NotFound();
            }

            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName", service.CustomerId);
            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName", service.ProductId);
            ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "EmpName", service.EmployeeId);

            return View(service);

        }



        [HttpPost]
        public async Task<IActionResult> ServiceEdit(ServiceEntity viewModel)
        { 
            if (ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(viewModel.ServiceId); 
                if (service == null)
                {
                    return NotFound();
                }

                
                service.CustomerId = viewModel.CustomerId;
                service.ProductId = viewModel.ProductId;
                service.EmployeeId = viewModel.EmployeeId;
                service.Model = viewModel.Model;
                service.SeriNo = viewModel.SeriNo;
                service.Warranty = viewModel.Warranty;
                service.Complaint = viewModel.Complaint;
                service.PerformedActions = viewModel.PerformedActions;
                service.PartsCost = viewModel.PartsCost;
                service.ServiceCost = viewModel.ServiceCost;
                service.TotalCost = viewModel.TotalCost;
                service.Description = viewModel.Description;
                service.PaymentStatus = viewModel.PaymentStatus;
                service.DeliveryDate = viewModel.DeliveryDate;
                try
                {
                    await _context.SaveChangesAsync();


                    TempData["SuccessMessage"] = "İçerik başarıyla güncellendi.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme işlemi başarısız oldu: {ex.Message}");
                }
                //await _context.SaveChangesAsync(); // Asenkron kaydetme

                return RedirectToAction("ServiceEdit", new { id = service.ServiceId }); 
            }

            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName", viewModel.CustomerId);
            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName", viewModel.ProductId);
            ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "EmpName", viewModel.EmployeeId);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ServiceDetails(int serviceId)
        {
            var service = await _context.Services
                .Include(s => s.Customer)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

            if (service == null)
            {
                return NotFound();
            }

            var serviceDetails = new
            {
                service.ServiceId,
                CustomerName = service.Customer.CustomerName,
                ProductName = service.Product.ProductName,
                service.Model,
                service.SeriNo,
                service.Complaint,
                service.PerformedActions,
                service.PartsCost,
                service.ServiceCost,
                service.TotalCost,
                service.Description,
                service.Warranty,
                service.PaymentStatus,
                service.DeliveryStatus,
                DeliveryDate = service.DeliveryDate?.ToString("dd/MM/yyyy")
            };

            return PartialView("ServiceDetails", serviceDetails);


        }
    }
}
