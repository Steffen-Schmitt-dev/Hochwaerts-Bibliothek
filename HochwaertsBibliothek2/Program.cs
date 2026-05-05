using HochwaertsBibliothek2.Data;
using HochwaertsBibliothek2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BibliothekDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CSBibliothek")));

builder.Services.AddHostedService<MahnUndStatusService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Start/Fehler");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "standard",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();