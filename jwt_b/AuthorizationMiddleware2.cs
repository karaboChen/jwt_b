using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Data;

namespace jwt_b
{
    public class AuthorizationMiddleware2 : Attribute, IAuthorizationFilter

    {
        public string Roles = "";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
          if (Roles == context.HttpContext.User.Identity.Name)
            {
                //context.Result = new JsonResult(new ReturnJson()
                //{
                //    Data = Roles
                //});
            }
            else
            {
                context.Result = new JsonResult(new ReturnJson()
                {
                    Data = "錯誤"
                });
            }
        
        }
    }
}
