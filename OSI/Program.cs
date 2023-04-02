using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OSI.Identitet;
using OSI.Areas.Identity.Data;
using OSI;
using OSI.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("OSIdentitetContextConnection") ?? throw new InvalidOperationException("Connection string 'OSIdentitetContextConnection' not found.");
//var connectionStringOsi = builder.Configuration.GetConnectionString("osi") ?? throw new InvalidOperationException("Connection string 'osi' not found.");

builder.Services.AddDbContext<OSIdentitetContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<OSIContext>(options =>
//   options.UseSqlServer(connectionStringOsi));

builder.Services.AddDefaultIdentity<OSIKorisnik>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<OSIdentitetContext>();

// Add services to the container.

builder.Services.AddControllersWithViews(options =>
{

});
builder.Services.AddRazorPages();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();
