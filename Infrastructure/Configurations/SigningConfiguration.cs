using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Configurations
{
    public class SigningConfiguration
    {
        public SecurityKey SecurityKey { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfiguration(string key)
        {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            SecurityKey = new SymmetricSecurityKey(keyBytes);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}