using GoodGoals.Data;
using GoodGoals.Models;
using GoodGoals.Patterns.Decorator;
using GoodGoals.Patterns.Factory;
using GoodGoals.Patterns.Observer;
using GoodGoals.Repositories;
using GoodGoals.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IReminderService, ReminderService>();

builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<IGoalService>(provider =>
{
    var inner = provider.GetRequiredService<GoalService>();
    var logger = provider.GetRequiredService<ILogger<LoggingGoalService>>();
    return new LoggingGoalService(inner, logger);
});

builder.Services.AddScoped<IReminderFactory, ReminderFactory>();
builder.Services.AddScoped<IGoalSubject, GoalEventManager>();
builder.Services.AddScoped<TaskCompletionObserver>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Good Goals API",
        Version = "v1",
        Description = "API REST para el sistema Good Goals."
    });
});

var app = builder.Build();

// ---- Aplicar migraciones automáticamente al iniciar ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

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
//djdjjdjG