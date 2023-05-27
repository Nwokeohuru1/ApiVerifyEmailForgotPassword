using ApiVerifyEmailForgotPassword.Data;
using ApiVerifyEmailForgotPassword.Helpers;
using ApiVerifyEmailForgotPassword.Interfaces;
using ApiVerifyEmailForgotPassword.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiVerifyEmailForgotPassword.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly DataContext _dataContext;
        private readonly PasswordHash _passwordHash;

        public UserController(IUserServices userServices, DataContext dataContext, PasswordHash passwordHash)
        {
            _userServices = userServices;
            _dataContext = dataContext;
            _passwordHash = passwordHash;
        }
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            
        
            if (_dataContext.users.Any(o => o.Email == request.Email))
            {
                return BadRequest("User already exists");
            }
            var user = await _userServices.Register(request);
            return Ok("User successfully created");
           
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }

            if (!_passwordHash.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("password is incorrect");
            }
            if (user.VerifiedAt == null)
            {
                return BadRequest("User not verified");
            }

            return Ok($"Welcome back {user.Email}");

        }


        [HttpPost]
        [Route("VerifyUser")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return BadRequest("invalid token");
            }
            user.VerifiedAt = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return Ok($"{user.Email} is verified");
        }

    }
}
