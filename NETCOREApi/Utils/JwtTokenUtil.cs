using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCOREApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NETCOREApi.Utils
{
    public class JwtTokenUtil
    {
        private readonly IConfiguration _configuration;

        public JwtTokenUtil(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //根据用户信息返回token
        public string GetToken(User user)
        {
            var claims = new[]
                {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim("userid",user.Id.ToString(),ClaimValueTypes.Integer64)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["issuer"],//Issuer
                audience: _configuration["audience"],//Audience
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //解析Token
        public long DecodeToken(String token)
        {
            token = token.Replace("Bearer ", string.Empty);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,//是否验证Issuer
                ValidateAudience = true,//是否验证Audience
                ValidateLifetime = true,//是否验证失效时间
                ValidateIssuerSigningKey = true,//是否验证SecurityKey
                ValidAudience = _configuration["audience"],//Audience
                ValidIssuer = _configuration["issuer"],//Issuer

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]))//拿到SecurityKey
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var a = jwtTokenHandler.ReadJwtToken(token);

            ClaimsPrincipal parse = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validated);
            var id = parse.Claims.FirstOrDefault(m => m.Type == "userid").Value;
            return long.Parse(id);
            //Console.WriteLine(a.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Name).Value);
        }



    }
}
