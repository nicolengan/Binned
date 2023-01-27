using Microsoft.AspNetCore.Identity;
using Binned.Model;
using Stripe;
using Binned.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddScoped<OrderService>();

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

StripeConfiguration.ApiKey = "sk_test_51MLgtkDRJAN9sJBkdSsIFHtxbiKKFYuz7IySriyQNfLhtMGIvdyUUWCTefqlDn1J0csXdyh2S697Nim90nEx9F6l00D1RtZIs8";

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapRazorPages();

app.Run();
