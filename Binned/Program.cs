using Microsoft.AspNetCore.Identity;
using Account.Model;
using Microsoft.EntityFrameworkCore;
using Binned.Data;
using Binned.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BinnedContextConnection") ?? throw new InvalidOperationException("Connection string 'BinnedContextConnection' not found.");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

builder.Services.AddDefaultIdentity<BinnedUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BinnedContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddDbContext<BinnedContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BinnedUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BinnedContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapRazorPages();

app.Run();
