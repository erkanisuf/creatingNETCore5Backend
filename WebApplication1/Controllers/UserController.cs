using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            public bool IsSuccs { get; set; }
            public IEnumerable<string> Errors { get; set; }

            public string Email { get; set; }
            public string Token { get; set; }
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


                return BadRequest(new RegistrationResponseDto { Errors = errors, IsSuccs = false });

            }
            else {
                return Ok(new RegistrationResponseDto { Email = newuser.Email, IsSuccs = true });
            }
           
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
                    
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qsecurityKey@3333"));
                    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(issuer: "https://localhost:5000/",
                        audience: "https://localhost:5000/",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    /*return Ok(new RegistrationResponseDto { IsSuccs=true ,Email = user.Email,Token=tokenString });*/
                    return Ok(new { Token = tokenString });
                }
                else {
                    IEnumerable<string> error = new List<string> { "Invalid Username or Password" };

                    return BadRequest(new RegistrationResponseDto { IsSuccs = false, Errors = error });
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> testva()
        {
            var usera = await _userManager.GetUserAsync(HttpContext.User);
            if (usera != null)
            {

                return Ok(new RegistrationResponseDto { Email = usera.Email, IsSuccs = true });
            }
            else
            {
                return Ok(new RegistrationResponseDto { Email = "nope", IsSuccs = true });
            }
            
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
