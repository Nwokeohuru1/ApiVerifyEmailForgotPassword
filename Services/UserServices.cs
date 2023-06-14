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

        public async Task<bool> Verify(string token)
        {

            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            user.VerifiedAt = DateTime.UtcNow; 
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByVerificationToken(string token)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            return user;
        }
        public async Task<User> GetUserByPasswordResetToken(string token)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);

            return user;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.Email == email);
            user.PasswordResetToken = _randomToken.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _dataContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token /*&& u.Email==request.Email*/);
            _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await _dataContext.SaveChangesAsync();

            return true;
        }

    }




}
