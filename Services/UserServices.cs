using ApiVerifyEmailForgotPassword.Data;
using ApiVerifyEmailForgotPassword.Helpers;
using ApiVerifyEmailForgotPassword.Interfaces;
using ApiVerifyEmailForgotPassword.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ApiVerifyEmailForgotPassword.Services
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _dataContext;
        private readonly PasswordHash _passwordHash;
        private readonly RandomToken _randomToken;

        public UserServices(DataContext dataContext, PasswordHash passwordHash, RandomToken randomToken)
        {
            _dataContext = dataContext;
            _passwordHash = passwordHash;
            _randomToken = randomToken;

        }

        public async Task<bool> Register(UserRegisterRequest request)
        {
         _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = _randomToken.CreateRandomToken()

            }; 

          _dataContext.Add(user);
          await _dataContext.SaveChangesAsync();
          return true;
        }

        
      


      
    }




}
