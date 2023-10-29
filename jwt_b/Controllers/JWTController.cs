using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace jwt_b.Controllers
{
    [Authorize]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public JWTController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
        [HttpGet("/gg")]
        public IActionResult kk()
        {
            return Ok("成功");
        }


        [HttpGet("/AA")]
        public IActionResult AA()
        {
            return Ok("AA");
        }

        [AllowAnonymous]
        [HttpGet("/get")]
        public IActionResult Get()
        {
            //生成jwt  選擇header 簽名算法
            var singnature = SecurityAlgorithms.HmacSha256;

            //payload,存放用戶訊息,範例我存放一個userid
            var claims = new[]
            {
                    //new Claim(ClaimTypes.Name, "JohnDoe"), // 使用者名稱
                    //new Claim(ClaimTypes.Role, "Admin"),   // 使用者角色
                    //new Claim(ClaimTypes.Email, "johndoe@example.com"), // 使用者電子郵件

                new Claim(JwtRegisteredClaimNames.Sub, "管理者"),
                new Claim(JwtRegisteredClaimNames.Name, "林冠名"),
                new Claim(ClaimTypes.Role, "林冠"),   // 使用者角色
                new Claim(JwtRegisteredClaimNames.Email, "johndoe@example.com")
            };




            //signature  把私要拿出來  先轉乘UTF8
            var serectByte = Encoding.UTF8.GetBytes(_configuration["AuthKey:Sercet"]);
            // 使用非對稱 對私鑰加密
            var singkey = new SymmetricSecurityKey(serectByte);

            //使用Hsm256 來驗證加密後key 數字
            var signingCredentials = new SigningCredentials(singkey, singnature);

            var token = new JwtSecurityToken(

                issuer: _configuration["AuthKey:Issuer"],
                audience: _configuration["AuthKey:Audience"],
                claims: claims,   //存放個人訊息
                notBefore: DateTime.UtcNow,  //發布時間
                expires: DateTime.UtcNow.AddSeconds(30), // 有效時間
                signingCredentials
                
             );


            //生成字串福的token
            var Token_key = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(Token_key);
        }





    }
}
