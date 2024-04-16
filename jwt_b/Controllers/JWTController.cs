using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt_b.Controllers
{

    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public JWTController(IConfiguration configuration)
        {
            _configuration = configuration;

        }






 

        [AllowAnonymous]
        [HttpGet("/gww")]
        public  async  Task<IActionResult> test()
        {
            // 原始字串
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "http://localhost:5164/api/Read";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    // 處理回應內容
                    Console.WriteLine(content);
                }
            }

            // 將字串轉換成Base64編碼
            string base64EncodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration["AuthKey:Sercet"]));

            return Ok(base64EncodedString);
        }

        [AuthorizationMiddleware2(Roles = "林冠名")]
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
        public async Task<IActionResult> Get()
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
                expires: DateTime.UtcNow.AddHours(3), // 有效時間
                signingCredentials               
             );


            //生成字串福的token
            var Token_key = new JwtSecurityTokenHandler().WriteToken(token);


            var claims2 = new List<Claim>
               {
                    //new Claim(ClaimTypes.Name, "JohnDoe"), // 使用者名稱
                    //new Claim(ClaimTypes.Role, "Admin"),   // 使用者角色
                    //new Claim(ClaimTypes.Email, "johndoe@example.com"), // 使用者電子郵件
                new Claim(ClaimTypes.Name, "林冠名"),
                new Claim(ClaimTypes.Role, "林冠"),   // 使用者角色
   
            };

            var idenity = new ClaimsIdentity(claims2, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal  = new ClaimsPrincipal(idenity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
            Log.Warning("我不知道他是誰的 {Token_key}", Token_key);
            return Ok(Token_key);
        }
    }
}
