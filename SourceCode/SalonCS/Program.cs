using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using SalonCS.Data;
using SalonCS.Filters;
using SalonCS.IServices;
using SalonCS.Services;
using Serilog;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//Password service should be registered before Db register since its refering the password service
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IAsymmetricEncryption, AsymmetricEncryption>();
builder.Services.AddSingleton<IRSAService, RSAService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<IUtilityService,UtilityService>();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews(Options => Options.Filters.Add<ErrorHandlingFilterAttribute>());

builder.Services.AddTransient<IAuthenticationService,AuthenticationService>();

var _logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(_logger);
builder.Services.AddEndpointsApiExplorer();

var tokenKey = builder.Configuration.GetSection("LoginSecret:Token").Value;

if (String.IsNullOrEmpty(tokenKey)) throw new Exception("Invalid token found");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

CreateKeyPair();

app.Run();

void CreateKeyPair() 
{
    RSA rsa = RSA.Create(512);
    
    var publicKey = rsa.ToXmlString(false);
    var privateKey = rsa.ToXmlString(true);

    Environment.SetEnvironmentVariable("PublicKey", publicKey);
    Environment.SetEnvironmentVariable("PrivateKey", privateKey);
}