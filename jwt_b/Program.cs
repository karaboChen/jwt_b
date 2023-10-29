using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //取出私鑰
                    var serectByte = Encoding.UTF8.GetBytes(builder.Configuration["AuthKey:Sercet"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //驗證發布者
                        ValidateIssuer = true,
                        ValidIssuer= builder.Configuration["AuthKey:Issuer"],

                        //驗證接收者
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AuthKey:Audience"],

                        //驗證是否過期
                        ValidateLifetime = true,

                        //驗證私鑰
                        IssuerSigningKey = new SymmetricSecurityKey(serectByte),

                         ClockSkew = TimeSpan.Zero // 到期直接掛




                    };
                });



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
