using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Projectify.Database;
using Projectify.Models;

namespace Projectify.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _singInManager;
        private readonly IConfiguration _configuration;
        private RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationContext _context;


        public UserController(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _singInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            _configuration = configuration;
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _httpContextAccessor = httpContextAccessor;
            _context = serviceProvider.GetRequiredService<ApplicationContext>();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim("UserID",user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue<string>("SigningSecret"))), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var Token = tokenHandler.WriteToken(securityToken);
                return Ok(new
                {
                    token = Token,
                });
            }
            else
                return BadRequest(new
                {
                    userMessage = "Username or password is incorrect.",
                    errorCode = "Wrong credentials",
                });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return BadRequest(new
                {
                    userMessage = "User Already Exists",
                    errorCode = "Email already used",
                });
            }
            else
            {
                ApplicationUser NewUser = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.FirstName + model.LastName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    PhoneNumber = model.phoneNumber,
                    DateOfBirth = model.dateOfBirth
                };

                _userManager.CreateAsync(NewUser, model.Password).Wait();
                return Ok(
                );



            }
            

        }

        [Authorize]
        [HttpGet]
        [Route("getprofile")]
        public IActionResult GetCurrentUser()
        {
            string userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            ApplicationUser user = _context.Users.SingleOrDefault(u => u.Id == userID);
            return Ok(new
            {
                result = user
            });
        }
    }

}

