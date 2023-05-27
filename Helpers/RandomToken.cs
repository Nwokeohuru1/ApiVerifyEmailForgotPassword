using System.Security.Cryptography;

namespace ApiVerifyEmailForgotPassword.Helpers
{
    public class RandomToken
    {
        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
