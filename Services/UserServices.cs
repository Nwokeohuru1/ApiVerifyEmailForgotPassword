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

        public Task<List<User>> GetUsers()
        {
            var user = _dataContext.users.ToListAsync();
            return user;
        }

        public async Task<User> GetUser(string email)
        {
            var user_ = await _dataContext.users.FirstOrDefaultAsync(u => u.Email == email);
            return user_;
           
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
