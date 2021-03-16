using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager; // UserManager From ASP Identity Packet
        private readonly SignInManager<User> _signInManager; // SignIN from Identity Pack
        private readonly IMapper _mapper;


        private IConfiguration Configuration { get; } // So i can get strings from appsettings.json
       
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            Configuration = configuration; // for appsettings.json 
        }

        public class RegistrationResponseDto // sends errors in list
        {
            public bool IsSuccs { get; set; }
            public IEnumerable<string> Errors { get; set; }

            public string Email { get; set; }
            public string Token { get; set; }
            public string TokenExpires { get; set; }
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
                string secretkey = Configuration.GetValue<string>("JWTSettings:mySecret");
                if (result.Succeeded)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
                    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    List<Claim> myclaims = new List<Claim>();
                    myclaims.Add(new Claim(ClaimTypes.Email, user.Email));
                    myclaims.Add(new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(60).ToString()));
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:3000/",
                        audience: "https://localhost:5000/",
                        claims: myclaims,
                        expires: DateTime.Now.AddMinutes(60),
                         signingCredentials: signingCredentials);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new RegistrationResponseDto { IsSuccs = true,Email=user.Email, Token=tokenString , TokenExpires = DateTime.Now.AddMinutes(1).ToString() });
                }
                else {
                    IEnumerable<string> error = new List<string> { "Invalid Username or Password" };

                    return BadRequest(new RegistrationResponseDto { IsSuccs = false, Errors = error });
                }
                
               
               
            }
   
            return Ok();
        }





        [Authorize]
        [HttpGet]
        // Checks users token and then looks inside the token and gets the name and other information.
        public async Task<IActionResult> GetUserByToken()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var expiration = claimsIdentity.FindFirst(ClaimTypes.Expiration)?.Value;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new RegistrationResponseDto { IsSuccs = true, Email = userId, TokenExpires = expiration });
        }

        [HttpPost]
        // Checks users token and then looks inside the token and gets the name and other information.
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            //
            var user = await _userManager.FindByNameAsync(email);
            string kur = user.Id;
            /*if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok(new RegistrationResponseDto { IsSuccs = true, Email = email })
            }*/
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string apiKey = Configuration.GetValue<string>("SendGrid:mySecret");
            /*var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");*/ // Other possible way to use
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("djerimixer2260@abv.bg", "Helsinki Guide App");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, "Password reset - Helsinki Guide App");
            var plainTextContent = "Click on the button and after that add your new password";
            var htmlContent = "<h1>Click on the button and after that add your new password</h1>" +
                "<br /><button style='background-color:#0093e9;border:none;color:white;width:200px;padding:25px;cursor:Pointer;'>" +
                "Click Here</button> " +
                $"{token}";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return Ok(new RegistrationResponseDto { IsSuccs = true, Email = kur });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Checks users token and then looks inside the token and gets the name and other information.
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            //check if valid model
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            //Do this more elegant way to response the error array!
            if (!resetPassResult.Succeeded)
            {
                List<string> kur = new List<string>();
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    kur.Add(error.Description);
                }
                return Ok(kur);
            }
            else { return Ok(new RegistrationResponseDto { IsSuccs = true, Email = user.Email, }); }
        }



        }
}
