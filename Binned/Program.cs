using Microsoft.AspNetCore.Identity;
using Binned.Model;
using Stripe;
using Binned.Services;
using Binned.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("AuthConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthConnectionString' not found.");


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<Binned.Services.ProductService>();


/*builder.Services.AddDefaultIdentity<BinnedUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>();*/


builder.Services.AddIdentity<BinnedUser, IdentityRole>(
options => {
    options.Stores.MaxLengthForKeys = 128;
})
.AddEntityFrameworkStores<MyDbContext>()
.AddRoles<IdentityRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();


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


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MyDbContext>();

    context.Database.Migrate();

    var userMgr = services.GetRequiredService<UserManager<BinnedUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    RolesManagement.Initialize(context, userMgr, roleMgr).Wait();
}

app.Run();
