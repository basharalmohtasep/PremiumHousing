using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PremiumHousing.Data;
using System.Text;
using Microsoft.Extensions.FileProviders;
using PremiumHousing.Helper;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
// Get the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


//#if !DEBUG
//// Configure DbContext with SQL Server
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//#else
// Configure DbContext with SQL Lite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlite(connectionString));
//#endif
builder.Services.AddScoped<AdminUserSeeder>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<TokenHelper>();

builder.Services.AddControllersWithViews();


var secretKey = builder.Configuration.GetSection("JwtSettings")["Secret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new InvalidOperationException("JWT Secret key not found."))),
    };
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var adminSeeder = scope.ServiceProvider.GetRequiredService<AdminUserSeeder>();
    await adminSeeder.SeedAdminUserAsync();
}
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

//Run on every request
app.Use(async (context, next) =>
{
    //Add JWToken to all incoming HTTP Request Header
    var JWToken = context.Request.Cookies["Token"];
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Append("Authorization", "Bearer " + JWToken);
    }
    //Redirect to a not-found page if Status Code 404
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
