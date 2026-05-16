using CoopControlWeb.Components;
using CoopControlWeb.Components.Middleware;
using CoopControlWeb.Modelos;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CoopControlWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<UserService>();
builder.Services.AddControllers(); // Añadir esto
builder.Services.AddScoped<PoliticasCreditoService>();



builder.Services.AddDbContextFactory<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapControllers(); // Añadir esto antes de app.Run()

app.UseAuthentication();
app.UseMiddleware<AuthMiddleware>();
app.UseAuthorization();

app.UseAntiforgery();

// Endpoint para manejar logout
app.MapPost("/logout-handler", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();