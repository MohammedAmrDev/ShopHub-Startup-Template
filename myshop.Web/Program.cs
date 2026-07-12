using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.BLL;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.DAL.Repositories;
using myshop.Models.IdentityEntities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    )) ;

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4);
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.FallbackPolicy = options.GetPolicy("AdminOnly");
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.ConfiguringBusinessLogicLayer();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddHttpContextAccessor();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
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


app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();

