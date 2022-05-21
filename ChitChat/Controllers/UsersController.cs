using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChitChat.Data;
using ChitChat.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ChitChat.Controllers
{
    public class UsersController : Controller
    {

        private readonly ChitChatContext _context;

        public UsersController(ChitChatContext context)
        {
            _context = context;
        }
        /*
         * 
         *         private async void Signin(User account)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.Username),
                        new Claim(ClaimTypes.Role, account.Type.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10);
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                }

         */
        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] User user)
        {
            var q = from u in _context.User
                    where u.Username == user.Username && u.Password == user.Password
                    select u;

            if (!q.Any())
            {
                ViewData["Error"] = "Wrong Password or Username";

            }
            else
            {
                //Signin(q.First());
                HttpContext.Session.SetString("username", q.First().Username);
                return RedirectToAction(nameof(Index), "Chats");
            }
            return View(user);
        }

        public void Logout()
        {
            HttpContext.SignOutAsync();
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Username == user.Username
                        select u;

                if (q.Any())
                {
                    ViewData["Error"] = "Already have a user with this username, Please pick a different Username";

                }
                else
                {
                    //Signin(user);
                    HttpContext.Session.SetString("username", user.Username);
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }
    }
}
