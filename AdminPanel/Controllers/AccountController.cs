using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AdminPanel.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(AccountDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account modelAccount)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Username == modelAccount.Username && a.Password == modelAccount.Password);

                if (account != null) 
                {
                    var user = new IdentityUser { UserName = modelAccount.Username };
                    await _signInManager.SignInAsync(user,isPersistent:false);
                    return Redirect("/Home/Index");  //modelAccount.ReturnUrl ??
                }
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            }

                                                                        
            return View();
        }

        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(ReturnUrl);
        }
    }
}
