using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpaceZD.BusinessLayer.Configuration
{
    public class AuthOptions
    {
        public const string Issuer = "SpaceZdApi"; // издатель токена
        public const string Audience = "Frontovik"; // потребитель токена
        const string Key = "SpaceKeyZDMandarin228";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
