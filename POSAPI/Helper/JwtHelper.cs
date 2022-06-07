using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace POSAPI.Helper
{
    public class JwtHelper
    {
        private string key_token = "zurcher inovation program key";

        public string Generate(int id)
        {
            var symmetric_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key_token));
            var credentiel = new SigningCredentials(symmetric_key, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentiel);
            var paylod = new JwtPayload(id.ToString(), audience:null, claims:null,notBefore: null,expires: DateTime.Today.AddDays(1));
            var securitytoken = new JwtSecurityToken(header, paylod);

            return new JwtSecurityTokenHandler().WriteToken(securitytoken);
        }
        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(key_token);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);
            return (JwtSecurityToken)validatedToken;
        }
    }
}
