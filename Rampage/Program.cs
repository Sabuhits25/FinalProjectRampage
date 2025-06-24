using Rampage.BLL;
using Rampage.DAL;
using Rampage.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services
    .AddDataAccess(builder.Configuration)
    .AddBusiness();



builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Events.OnRedirectToLogin = context =>
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var pathSegments = path.Trim('/').Split('/');

        string lang = "en";
        if (pathSegments.Length > 0 && (pathSegments[0] == "az" || pathSegments[0] == "en"))
        {
            lang = pathSegments[0];
        }

        context.Response.Redirect($"/{lang}/Account/Login?ReturnUrl={Uri.EscapeDataString(context.Request.Path)}");
        return Task.CompletedTask;
    };
    opt.Events.OnRedirectToAccessDenied = context =>
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var pathSegments = path.Trim('/').Split('/');

        string lang = "en";
        if (pathSegments.Length > 0 && (pathSegments[0] == "az" || pathSegments[0] == "en"))
        {
            lang = pathSegments[0];
        }

        context.Response.Redirect($"/{lang}/Home/AccessDenied?ReturnUrl={Uri.EscapeDataString(context.Request.Path)}");
        return Task.CompletedTask;
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/en/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/en");
        return;
    }
    await next();
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;
    var pathSegments = path.Trim('/').Split('/');

    if (pathSegments.Length > 0 && (pathSegments[0].ToLower() == "en" || pathSegments[0].ToLower() == "az"))
    {
        context.Items["lang"] = pathSegments[0].ToLower();
    }
    else
    {
        context.Items["lang"] = "en";
    }

    await next();
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "localized",
    pattern: "{lang}/{controller}/{action}/{id?}",
    defaults: new { controller = "Home", action = "Index" },
    constraints: new { lang = "en|az" }
);

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.Run();
