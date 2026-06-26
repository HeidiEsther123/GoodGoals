### Universidad: Tecnologico de Software
### Materia: Arquitectura de Software
### Maestro: Jorge Javier Pedroza Romero
### Alumno: Heidi Esther Peña Betanzos
### Grado: 3B

Aplicación web de productividad personal construida con **ASP.NET Core MVC + Web API + Entity Framework Core + SQL Server**, siguiendo una Arquitectura en Capas formal e incorporando patrones de diseño GOF.
---

## ¿Qué es Good Goals?

Good Goals es una herramienta diseñada para personas que desean mejorar su productividad y fortalecer su disciplina personal. Está especialmente pensada para ser accesible y amigable con quienes presentan déficit de atención. Reúne en un solo lugar calendario, agenda, seguimiento de objetivos, notas y recordatorios:

- 🎯 **Metas** — define objetivos con fecha límite y seguimiento de avance
- ✅ **Tareas** — organiza actividades del día a día, asociadas opcionalmente a una meta
- 🗒️ **Notas** — espacio libre para ideas, apuntes y reflexiones
- ⏰ **Recordatorios** — avisos programados para no olvidar nada importante

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server o SQL Server LocalDB (incluido con Visual Studio)
- Visual Studio 2022 o VS Code

---

## Cómo correrlo

1. Clona el repositorio:
   ```bash
   git clone https://github.com/HeidiEsther123/GoodGoals.git
   cd GoodGoals
   ```

2. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
   ```

3. Ajusta la cadena de conexión en `appsettings.json` si no usas LocalDB.

4. Crea la base de datos:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Corre el proyecto:
   ```bash
   dotnet run
   ```

6. Abre el navegador:
   - `/` → página de inicio
   - `/Identity/Account/Register` → crear cuenta
   - `/Goals`, `/Tasks`, `/Notes`, `/Reminders` → módulos (requieren login)
   - `/swagger` → documentación interactiva de la API REST

---

## Estructura del proyecto

```
GoodGoals/
├── Models/              # Entidades del dominio (Goal, TaskItem, Note, Reminder, ApplicationUser)
├── Data/                # AppDbContext (EF Core + Identity)
├── Repositories/        # Repositorio genérico (acceso a datos)
├── Services/            # Lógica de negocio por módulo
├── Controllers/         # Controladores MVC (vistas Razor)
├── Controllers/Api/     # Controladores de la API REST
├── Views/               # Vistas Razor (.cshtml)
├── Areas/               # Identity UI personalizada (Login, Register en español)
├── Patterns/            # Patrones de diseño GOF
│   ├── Factory/         # ReminderFactory
│   ├── Decorator/       # LoggingGoalService
│   └── Observer/        # GoalEventManager + TaskCompletionObserver
└── wwwroot/             # Archivos estáticos (CSS, JS)
```
