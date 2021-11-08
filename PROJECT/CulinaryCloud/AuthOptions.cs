using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CulinaryCloud
{
    public class AuthOptions
    {
        public const string ISSUER = "CulinaryCloud";
        public const string AUDIENCE = "CulinaryCalculator";
        private const string KEY = "MySecretKey100500";
        public const int LIFE_TIME_MINUTES = 100;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
