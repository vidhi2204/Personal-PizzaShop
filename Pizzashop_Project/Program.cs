using BLL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using DAL.Models;
using BLL.Interfaces;
using BLL.Interfaces;
using BLL.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var conn = builder.Configuration.GetConnectionString("PizzaDbConnection");
builder.Services.AddDbContext<DAL.Models.PizzashopDbContext>(q=>q.UseNpgsql(conn));

builder.Services.AddScoped<IUserLoginService,UserLoginService>();
builder.Services.AddScoped<IJWTTokenService,JWTTokenService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddScoped<IRolesPermission,RolesPermissionService>();
builder.Services.AddScoped<IMenuService,MenuService>();

builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(x=>{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],  // The issuer of the token (e.g., your app's URL)
            ValidAudience = builder.Configuration["JwtConfig:Audience"], // The audience for the token (e.g., your API)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]?? "")), // The key to validate the JWT's signature
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name 
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Check for the token in cookies
                var token = context.Request.Cookies["AuthToken"]; // Change "AuthToken" to your cookie name if it's different
                // if (!string.IsNullOrEmpty(token))
                // {
                //     context.Request.Headers["Authorization"] = "Bearer " + token;
                // }
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // Redirect to login page when unauthorized 
                context.HandleResponse();
                context.Response.Redirect("/UserLogin/VerifyPassword");
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                // Redirect to login when access is forbidden (403)
                context.Response.Redirect("/UserLogin/VerifyPassword");
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddAuthorization();

// app.Use(async (context, next) =>
// {
//     context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
//     context.Response.Headers.Add("Pragma", "no-cache");
//     context.Response.Headers.Add("Expires", "0");

//     await next();
// });


builder.Services.AddSession(
    options => {
        options.IdleTimeout = TimeSpan.FromSeconds(10);
    }
);
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserLogin}/{action=VerifyPassword}/{id?}");


app.Run();