using WebAppWiki.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppWiki.DataAccessLayer;
using WebAppWiki.Authorize;
using WebAppWiki.Helpers;
using Mapster;
using WebAppWiki.Mapping;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DbContextWikiConnection") ?? throw new InvalidOperationException("Connection string 'DbContextWikiConnection' not found.");
//builder.Services.AddDbContext<DbContextWiki>(options => options.UseSqlServer(connectionString));

builder.SetupRepository();

builder.Services.AddDefaultIdentity<AppUser>(o => o.SignIn.RequireConfirmedEmail = false).AddRoles<IdentityRole>()
    .AddDefaultUI().AddEntityFrameworkStores<DbContextWiki>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<DataSeeder>();
builder.Services.AddTransient<ServiceFilter>();
builder.Services.AddTransient<ServiceAdmin>();
builder.Services.AddTransient<ServiceGame>();

RegisterMapper.RegisterSettings();
builder.Services.AddMapster();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromSeconds(60);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.DbSeedWithScope();
}

app.Run();
