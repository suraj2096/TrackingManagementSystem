using DataTrackingSystem.Service.IService;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataTrackingSystem.Service
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _config;
        public TokenHandler(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(string userId)
        {
            string? secretkey = _config.GetValue<string>("JwtToken:SecretKey");
            int tokenValidity = _config.GetValue<int>("JwtToken:TokenValidityInMinutes");
             // this is used to creating and validating the jwt token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userId) 
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenValidity),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            return tokenHandler.WriteToken(token);
            
        }
        public string? GetUserIdFromToken(string userToken)
        {
            if (userToken == "Bearer") return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var data = tokenHandler.ReadJwtToken(userToken);
            var userName = data.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            return userName;
        }


    }
}
