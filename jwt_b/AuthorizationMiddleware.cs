using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt_b
{
    public class AuthorizationMiddleware : Attribute, IAuthorizationFilter

    {
        private readonly IConfiguration _configuration;

        public AuthorizationMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //自行定義的
            bool tokenFlag = context.HttpContext.Request.Headers.TryGetValue("sda", out StringValues values);


            //var ignore = (from a in context.ActionDescriptor.EndpointMetadata
            //              where a.GetType() == typeof(AllowAnonymousAttribute)
            //              select a).FirstOrDefault();

            var ignore = context.ActionDescriptor.EndpointMetadata
                            .OfType<AllowAnonymousAttribute>()
                             .FirstOrDefault();


            if (ignore == null)
            {

                if (tokenFlag)
                {

                    // 將Base64編碼的字串轉換回原始字串
                    //byte[] base64Bytes = Convert.FromBase64String(values);
                    //string decodedString = Encoding.UTF8.GetString(base64Bytes);


                    if (!確定動作(values))
                    {
                        context.Result = new JsonResult(new ReturnJson()
                        {
                            Data = "測試1",
                            HttpCode = 401,
                            ErrorMessage = "沒有登入"
                        });
                    }

                }
                else
                {
                    context.Result = new JsonResult(new ReturnJson()
                    {
                        Data = "測試2",
                        HttpCode = 401,
                        ErrorMessage = "沒有登入"
                    });
                }

            }
        }

        //解密動作
        public dynamic 確定動作(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                // 設定 JWT 的發行者和接收者
                ValidIssuer = _configuration["AuthKey:Issuer"],
                ValidAudience = _configuration["AuthKey:Audience"],
                // 設定驗證 JWT 的簽名金鑰和加密算法
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthKey:Sercet"])),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };


            try
            {
                // 驗證 JWT
                var jwtHandler = new JwtSecurityTokenHandler();

                var principal = jwtHandler.ValidateToken(token, validationParameters, out var validatedToken);


                //foreach (var  e  in principal.Claims)
                //{
                
                //}

                var jwtToken = validatedToken as JwtSecurityToken;

                // 取得使用者名稱和 JWTID
                var username = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                var username1 = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;

                // var jwtId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (username1 == "林冠名") return true;

                return false;
                // 在這裡可以執行額外的驗證，例如檢查使用者是否存在於資料庫中，以及檢查使用者是否有權限存取資源等等
            }
            catch (Exception)
            {
                 return false;
            }
  


        }

    }
}
