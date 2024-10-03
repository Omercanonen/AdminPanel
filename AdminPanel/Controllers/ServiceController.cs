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
        public async Task<IActionResult> ServicePage(string sortOrder)
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
        public async Task<IActionResult> ServiceEdit(int ServiceId)
        {
            //var service = await _context.Customers.FindAsync(ServiceId);

            //return View(service);
            //ServiceEntity'den ilgili kaydı bul
            //var service = await _context.Services.FindAsync(ServiceId);

            //if (service == null)
            //{
            //    //Eğer kayıt bulunamazsa, hata mesajı ekleyip geri döner
            //    ModelState.AddModelError(string.Empty, "Servis bulunamadı.");
            //    return RedirectToAction("ServicePage"); // Örneğin listeleme sayfasına geri dönebilirsiniz.
            //}

            ////DropDown listeleri doldur
            //ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            //ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName");
            //ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");

            ////Bulunan servisi view'a gönder
            //return View(service);
            // ServiceEntity'den ilgili kaydı bul
            var service = await _context.Services.FindAsync(ServiceId);

            if (service == null)
            {
                // Eğer kayıt bulunamazsa, hata mesajı ekleyip geri döner
                ModelState.AddModelError(string.Empty, "Servis bulunamadı.");
                return RedirectToAction("ServicePage"); // Listeleme sayfasına geri dönebilirsiniz.
            }

            // DropDown listeleri doldur
            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");

            // Bulunan servisi view'a gönder
            return View(service);
        }



        [HttpPost]
        public async Task<IActionResult> ServiceEdit(ServiceEntity viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Mevcut servis kaydını bul
            var service = await _context.Services.FindAsync(viewModel.ServiceId);

            if (service == null)
            {
                ModelState.AddModelError(string.Empty, "Servis bulunamadı.");
                return View(viewModel);
            }

            // Mevcut müşteri bilgilerini güncelle
            service.CustomerId = viewModel.CustomerId;
            service.ProductId = viewModel.ProductId;
            service.Model = viewModel.Model;
            service.EmployeeId = viewModel.EmployeeId;
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
                // Veritabanında değişiklikleri kaydet
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Servis başarıyla güncellendi.";
                return RedirectToAction("ServiceEdit", new { Id = viewModel.ServiceId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Güncelleme işlemi başarısız oldu: {ex.Message}");

            }
            return View(viewModel);


        }
    }
}
