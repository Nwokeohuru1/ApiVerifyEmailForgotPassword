using ApiVerifyEmailForgotPassword.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiVerifyEmailForgotPassword.Interfaces
{
    public interface IUserServices
    {
    Task<bool> Register(UserRegisterRequest request);
    Task<List<User>> GetUsers();
    Task<User> GetUser(string email);





    }
}
