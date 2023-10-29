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
                    //���X�p�_
                    var serectByte = Encoding.UTF8.GetBytes(builder.Configuration["AuthKey:Sercet"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //���ҵo����
                        ValidateIssuer = true,
                        ValidIssuer= builder.Configuration["AuthKey:Issuer"],

                        //���ұ�����
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AuthKey:Audience"],

                        //���ҬO�_�L��
                        ValidateLifetime = true,

                        //���Ҩp�_
                        IssuerSigningKey = new SymmetricSecurityKey(serectByte),

                         ClockSkew = TimeSpan.Zero // ���������




                    };
                });



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
