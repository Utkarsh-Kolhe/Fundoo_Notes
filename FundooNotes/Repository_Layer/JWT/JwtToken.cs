using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.Entity;

namespace Repository_Layer.JWT
{
    public class JwtToken
    {
        private readonly IConfiguration _config;
        public JwtToken(IConfiguration config)

        {
            _config = config;

        }

        public string GenerateToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Initializing an array of Claim type objects
            var claims = new[]
            {
               new Claim("Email",user.Email),                   //Creating Claim object that would get stored in Jwt payload
               new Claim("UserId",user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
