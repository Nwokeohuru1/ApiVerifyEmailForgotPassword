using ApiVerifyEmailForgotPassword.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiVerifyEmailForgotPassword.Interfaces
{
    public interface IUserServices
    {
    Task<bool> Register(UserRegisterRequest request);
    Task<List<User>> GetUsers();
    Task<User> GetUser(string email);
    Task<bool> Verify(string token);
    Task<User> GetUserByVerificationToken(string token);
    Task<User> GetUserByPasswordResetToken(string token);
    Task<bool> ForgotPassword(string email);
    Task<bool> ResetPassword(ResetPasswordRequest request);





    }
}
