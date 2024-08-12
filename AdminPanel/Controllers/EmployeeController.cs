using AdminPanel.Models;
using AdminPanel.Models.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AdminPanel.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            this._context = context;
        }
        [Authorize]
        public async Task<IActionResult> EmployeePage(string sortOrder)
        {

            ViewData["EmployeeIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "EmployeeId_desc" : "";
            ViewData["EmpNameSortParam"] = sortOrder == "EmpName" ? "EmpName_desc" : "EmpName";
            ViewData["EmpSurnameSortParam"] = sortOrder == "EmpSurname" ? "EmpSurname_desc" : "EmpSurname";
            ViewData["EmpPhoneNumberSortParam"] = sortOrder == "EmpPhoneNumber" ? "EmpPhoneNumber_desc" : "EmpPhoneNumber";
            ViewData["EmpTitleSortParam"] = sortOrder == "EmpTitle" ? "EmpTitle_desc" : "EmpTitle";

            var employees = from e in _context.Employees select e;

            switch (sortOrder)
            {
                case "EmployeeId_desc":
                    employees = employees.OrderByDescending(e => e.EmployeeId);
                    break;
                case "EmpName":
                    employees = employees.OrderBy(e => e.EmpName);
                    break;
                case "EmpName_desc":
                    employees = employees.OrderByDescending(e => e.EmpName);
                    break;
                case "EmpSurname":
                    employees = employees.OrderBy(e => e.EmpSurname);
                    break;
                case "EmpSurname_desc":
                    employees = employees.OrderByDescending(e => e.EmpSurname);
                    break;
                case "EmpPhoneNumber":
                    employees = employees.OrderBy(e => e.EmpPhoneNumber);
                    break;
                case "EmpPhoneNumber_desc":
                    employees = employees.OrderByDescending(e => e.EmpPhoneNumber);
                    break;
                case "EmpTitle":
                    employees = employees.OrderBy(e => e.EmpTitle);
                    break;
                case "EmpTitle_desc":
                    employees = employees.OrderByDescending(e => e.EmpTitle);
                    break;

            }

            return View(await employees.ToListAsync());

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EmployeeEdit(int Id)
        {
            var employee = await _context.Employees.FindAsync(Id);

            if(employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EmployeeEdit(Employee viewModel)
        {
            var employee = await _context.Employees.FindAsync(viewModel.EmployeeId);

            if (employee != null)
            {
                employee.EmployeeId = viewModel.EmployeeId;
                employee.EmpName = viewModel.EmpName;
                employee.EmpSurname = viewModel.EmpSurname;
                employee.EmpPhoneNumber = viewModel.EmpPhoneNumber;
                employee.EmpTitle = viewModel.EmpTitle;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Güncelleme işlemi başarıyla tamamlandı.";
            }
            ModelState.AddModelError("", "Çalışan bulunamadı.");

            return View(viewModel);
            //if (ModelState.IsValid)
            //{
            //    var employee = await _context.Employees.FindAsync(viewModel.EmployeeId);

            //    if (employee != null)
            //    {
            //        employee.EmpName = viewModel.EmpName;
            //        employee.EmpSurname = viewModel.EmpSurname;
            //        employee.EmpPhoneNumber = viewModel.EmpPhoneNumber;
            //        employee.EmpTitle = viewModel.EmpTitle;

            //        _context.Employees.Update(employee);
            //        await _context.SaveChangesAsync();

            //        TempData["SuccessMessage"] = "Güncelleme işlemi başarıyla tamamlandı.";
            //        return RedirectToAction("EmployeePage");
            //    }
            //    ModelState.AddModelError("", "Çalışan bulunamadı.");
            //}
            //return View(viewModel);

        }
        public async Task<IActionResult> EmployeeDelete(int Id)
        {
            var employee = await _context.Employees.FindAsync(Id);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeePage");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EmployeeCreate()
        {
            var employees = await _context.Employees.ToListAsync();
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "EmpName");
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EmployeeCreate([Bind("EmpName,EmpSurname,EmpPhoneNumber,EmpTitle")] Employee employee)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Employees.Add(employee);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction("EmployeeCreate");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        ModelState.AddModelError("", "Bir hata oluştu, lütfen tekrar deneyin.");
            //    }
            //}

            //var errors = ModelState.Values.SelectMany(e => e.Errors);
            //foreach (var error in errors)
            //{
            //    Console.WriteLine(error.ErrorMessage);
            //}

            //return View(employee);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Kayıt işlemi başarıyla tamamlandı.";
                    return RedirectToAction("EmployeeCreate");
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

            return View(employee);
        }
    }
}
