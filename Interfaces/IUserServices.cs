using ApiVerifyEmailForgotPassword.Models;

namespace ApiVerifyEmailForgotPassword.Interfaces
{
    public interface IUserServices
    {
    Task<bool> Register(UserRegisterRequest request);
    

    
    }
}
