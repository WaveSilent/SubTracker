using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru-RU");
    options.SupportedCultures = new[] { new System.Globalization.CultureInfo("ru-RU") };
    options.SupportedUICultures = new[] { new System.Globalization.CultureInfo("ru-RU") };
});

var cultureInfo = new System.Globalization.CultureInfo("ru-RU");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Ïðèìåíåíèå ìèãðàöèé ê áàçå äàííûõ...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Ìèãðàöèè óñïåøíî ïðèìåíåíû!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Îøèáêà ïðè ïðèìåíåíèè ìèãðàöèé");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
