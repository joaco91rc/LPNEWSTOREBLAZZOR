
using Blazored.Toast;
using DataLayer;
using Entities;
using LPNEWSTORE.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Services;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Agregar DbContext con cadena de conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadena_conexion")));

builder.Services.AddSingleton<DatabaseSettings>();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SesionNegocioService>();
builder.Services.AddScoped<IModalLauncher, ModalLauncher>();


// Agregar Identity con soporte para roles
builder.Services.AddIdentity<Usuario, Rol>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager(); // opcional, ya está incluido normalmente

// Configurar la cookie y el path de login así:
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login"; // tu página de login Razor Page
});


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
// Agrego los servicios por DI
builder.Services.AddApplicationServices();

builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()); // para test
});


builder.Services.AddBlazoredToast();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}





//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // <--- Mueve esto acá, antes de MapControllers
app.UseAuthorization();

app.MapControllers();      // Los controladores deben mapearse después de authentication/authorization

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();