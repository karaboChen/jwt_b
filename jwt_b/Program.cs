using jwt_b;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Serilog;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddHostedService<Repeat>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    //取出私鑰
//                    var serectByte = Encoding.UTF8.GetBytes(builder.Configuration["AuthKey:Sercet"]);
//                    options.TokenValidationParameters = new TokenValidationParameters()
//                    {
//                        //驗證發布者
//                        ValidateIssuer = true,
//                        ValidIssuer= builder.Configuration["AuthKey:Issuer"],

//                        //驗證接收者
//                        ValidateAudience = true,
//                        ValidAudience = builder.Configuration["AuthKey:Audience"],

//                        //驗證是否過期
//                        ValidateLifetime = true,

//                        //驗證私鑰
//                        IssuerSigningKey = new SymmetricSecurityKey(serectByte),

//                         ClockSkew = TimeSpan.Zero // 到期直接掛

//                    };
//                });


//自訂義過濾
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizationMiddleware(builder.Configuration));
});





//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie(b =>
//{
//    //b.LoginPath = "/gg";
//    b.Cookie.Name = "w53";
//    b.Cookie.Path = "/";
//    b.Cookie.HttpOnly = true;
//    b.ExpireTimeSpan = TimeSpan.FromSeconds(15);

//});

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration).CreateLogger();

//builder.Host.UseSerilog();
//.MinimumLevel.Warning()
//.MinimumLevel.Debug()
//.WriteTo.File("logs/tt-.txt", rollingInterval:RollingInterval.Day)
//.CreateLogger();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();

//  app.UseSerilogRequestLogging();

app.UseDefaultFiles();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "no-store";
        ctx.Context.Response.Headers["Cache-Control"] = "private,max-age=0";
    }
});



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



