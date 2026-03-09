using Microsoft.EntityFrameworkCore;
using CoopControlWeb.Modelos;

var builder = WebApplication.CreateBuilder(args);

// Servicios Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Registrar el DbContext (ajusta la cadena de conexión en appsettings.json)
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar UserService para inyección de dependencias
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Middleware y rutas (mantén lo que ya tengas)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();