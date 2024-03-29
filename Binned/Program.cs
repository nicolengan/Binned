using Microsoft.AspNetCore.Identity;
using Binned.Model;
using Stripe;
using Binned.Services;
using Binned.Areas.Identity.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using static Binned.Pages.RegisterModel;
using FluentAssertions.Common;
using Binned.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("AuthConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthConnectionString' not found.");

builder.Services.AddRazorPages();
var emailConfig = builder.Configuration.GetSection("email").Get<EmailSettings>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CodeService>();
builder.Services.AddScoped<Binned.Services.ProductService>();
builder.Services.AddScoped<Binned.Services.AccountService>();


/*builder.Services.AddDefaultIdentity<BinnedUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>();*/

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

builder.Services.AddIdentity<BinnedUser, IdentityRole>(
options =>
{
    options.Stores.MaxLengthForKeys = 128;
})
.AddEntityFrameworkStores<MyDbContext>()
//.AddRoles<IdentityRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
options =>
{
    // Cookie settings (WORKS, logs user out immediately)
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/errors/403";
    options.SlidingExpiration = true;
    options.Cookie.Name = "AnotherCookie";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/errors");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

StripeConfiguration.ApiKey = "sk_test_51MLgtkDRJAN9sJBkdSsIFHtxbiKKFYuz7IySriyQNfLhtMGIvdyUUWCTefqlDn1J0csXdyh2S697Nim90nEx9F6l00D1RtZIs8";

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


// error pages 404
app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MyDbContext>();

    var userMgr = services.GetRequiredService<UserManager<BinnedUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

//    RolesManagement.Initialize(context, userMgr, roleMgr).Wait();
}

app.Run();
