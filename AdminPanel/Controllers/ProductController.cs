using AdminPanel.Models;
using AdminPanel.Models.Employees;
using AdminPanel.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class ProductController : Controller      
    {
        
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> ProductPage(string sortOrder,string productSearchString)
        {
            ViewData["ProductIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "ProductId_desc" : "";
            ViewData["ProductNameSortParam"] = sortOrder == "ProductName" ?  "ProductName_desc" : "EmpName";

            var products = _context.Products.Where(p => p.IsActive);

            if (!String.IsNullOrEmpty(productSearchString))
            {
                products = products.Where(c => c.ProductName.Contains(productSearchString));
            }


            switch (sortOrder)
            {
                case "ProductId_desc":
                    products = products.OrderByDescending(p => p.ProductId);
                    break;
                case "ProductName":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "ProductName_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
            }

            return View(await products.ToListAsync());
        }
        public async Task<IActionResult> ProductDelete(int Id)
        {   
            var product = await _context.Products.FindAsync(Id);

            if (product == null)
            {
                return NotFound();
            }
            product.IsActive = false;
           _context.Products.Update(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product successfully deleted!";
            return RedirectToAction("ProductPage");
        }
        [Authorize]
        public async Task<IActionResult> ProductEdit(Product viewModel)
        {
            var product = await _context.Products.FindAsync(viewModel.ProductId);

            if (product != null)
            {
                product.ProductId = viewModel.ProductId;
                product.ProductName = viewModel.ProductName;
               

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Güncelleme işlemi başarıyla tamamlandı.";
            }
            ModelState.AddModelError("", "Model Bulunamadı.");

            return View(viewModel);
        }
        [HttpGet]
        [Authorize]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ProductCreate([Bind("ProductName")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductCreate");
            }
            return View(product);
        }

    }
}
