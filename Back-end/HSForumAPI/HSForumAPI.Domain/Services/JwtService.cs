using HSForumAPI.Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HSForumAPI.Domain.Services
{
    public class JwtService : IJwtService
    {
        private string _secretKey;
        public JwtService(IConfiguration conf)
        {
            _secretKey = conf.GetValue<string>("ApiSettings:Secret");
        }
        /// <summary>
        /// Creates Jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns>Token</returns>
        public async Task<string> GetJwtToken(int userId, string[] roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claimList = new List<Claim>() { new Claim(ClaimTypes.Name, userId.ToString()) };
            claimList.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());
            
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
