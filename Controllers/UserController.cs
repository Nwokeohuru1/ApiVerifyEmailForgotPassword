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
        private readonly PasswordHash _passwordHash;
        private readonly RandomToken _randomToken;

        public UserController(IUserServices userServices, PasswordHash passwordHash, RandomToken randomToken)
        {
            _userServices = userServices;
            _passwordHash = passwordHash;
            _randomToken = randomToken;
        }
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            var users = await _userServices.GetUsers();
            if (users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email Exists");
            }
            var RegUser = await _userServices.Register(request);
            return Ok("User successfully created");
           
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _userServices.GetUser(request.Email);
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
            var user = await _userServices.GetUserByToken(token);
            if (user == null)
            {
                return BadRequest("Invalid Token");
            }
            await _userServices.Verify(token);
            return Ok("verified!, You can now Login");
        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userServices.GetUser(email);
            if (user == null)
            {
                return BadRequest($"{email} not found");
            }
            await _userServices.ForgotPassword(email);
            return Ok("You may now reset your password");

        }

        //[HttpPost("Reset-Password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        //{
        //    if (user == null || user.ResetTokenExpires < DateTime.Now)
        //    {
        //        return BadRequest("Oops!! Invalid token");
        //    }
        //    return Ok("Password reset is done");

        //}

    }
}
