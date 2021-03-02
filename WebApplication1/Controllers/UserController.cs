using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager; // UserManager From ASP Identity Packet
        private readonly SignInManager<User> _signInManager; // SignIN from Identity Pack
        private readonly IMapper _mapper;
        
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public class RegistrationResponseDto // sends errors in list
        {
            public bool IsSuccessfulRegistration { get; set; }
            public IEnumerable<string> Errors { get; set; }
        }
        // GET: UserController

        public ActionResult Index()
        {
            return View();
        }

        // GET: http://localhost:5000/api/user/register - Register Route
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel newuser)
        {
            if (newuser == null || !ModelState.IsValid) {
                return BadRequest();
            }
                
            var user = _mapper.Map<User>(newuser);
            var result = await _userManager.CreateAsync(user, newuser.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);


                return BadRequest(new RegistrationResponseDto { Errors = errors });
               
            }
            return Ok("register");
        }


        // Login Route
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {

                    return Ok("loggedin");
                }
                else {

                   
                    return BadRequest("Invalid username or password");
                }
                
                
               /* ModelState.AddModelError(string.Empty, "Invalid Login Attempt");*/
               
            }
            /* var usera = _userManager.GetUserAsync(HttpContext.User);
             return Ok(usera);*/
            return Ok();
        }



        // THIS gets current logged user with session
        [HttpGet]
        public async Task<IActionResult> currentUser()
        {
            var usera = await _userManager.GetUserAsync(HttpContext.User);
            if (usera != null)
            {

                return Ok(usera.Email);
            }
            else {
                return BadRequest("Not logged in");
            }
            
        }

        // Logout Route
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
