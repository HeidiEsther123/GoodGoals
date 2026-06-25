using GoodGoals.Data;
using GoodGoals.Models;
using GoodGoals.Repositories;
using GoodGoals.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- Base de datos (Capa de Datos) ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ---- Identity (Autenticación) ----
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// ---- Repositorios (Capa de Acceso a Datos) ----
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ---- Servicios (Capa de Lógica de Negocio) ----
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IReminderService, ReminderService>();

// ---- MVC + Razor Pages (Identity UI) + API ----
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ---- Swagger (documentación de la API) ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Good Goals API",
        Version = "v1",
        Description = "API REST para el sistema Good Goals (Metas, Tareas, Notas, Recordatorios)."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Good Goals API v1"));
   
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
