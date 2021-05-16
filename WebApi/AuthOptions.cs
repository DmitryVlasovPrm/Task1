using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi
{
    public class AuthOptions
    {
        // Издатель
        public const string ISSUER = "AuthServer";
        // Потребитель
        public const string AUDIENCE = "AuthClient";
        // Ключ шифрования
        public const string KEY = "32892A11-99C7-4C78-8827-D00B031D52C1";
        // Время жизни токена
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}