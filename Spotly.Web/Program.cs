using Microsoft.AspNetCore.Identity;
using Spotly.Core.Entities;
using Spotly.Infrastructure;
using Spotly.Infrastructure.Data;
using Spotly.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/access-denied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(
        services.GetRequiredService<RoleManager<IdentityRole<int>>>(),
        services.GetRequiredService<UserManager<AppUser>>(),
        app.Configuration["Seed:AdminEmail"] ?? throw new InvalidOperationException("Seed:AdminEmail is missing."),
        app.Configuration["Seed:AdminPassword"] ?? throw new InvalidOperationException("Seed:AdminPassword is missing."));
    
    var db = services.GetRequiredService<AppDbContext>();
    await CategorySeeder.SeedAsync(db);

    if (app.Environment.IsDevelopment())
        await DemoDataSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();