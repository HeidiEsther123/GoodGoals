# Good Goals 🎯

Aplicación de productividad personal (Metas, Tareas, Notas, Recordatorios) construida en
ASP.NET Core MVC + Web API + Entity Framework Core + SQL Server, siguiendo la
Arquitectura en Capas (MVC) definida en el ADR-03 del proyecto.

## Requisitos previos
- .NET 8 SDK (https://dotnet.microsoft.com/download)
- SQL Server LocalDB (viene con Visual Studio) o SQL Server normal
- Visual Studio 2022 o VS Code

## Cómo correrlo

1. Descomprime el proyecto y abre la carpeta `GoodGoals` en Visual Studio o VS Code.

2. Restaura los paquetes NuGet:
   ```
   dotnet restore
   ```

3. Si usas otro motor de SQL Server (no LocalDB), edita la cadena de conexión en
   `appsettings.json` (`ConnectionStrings:DefaultConnection`).

4. Crea la base de datos con las migraciones de EF Core:
   ```
   dotnet tool install --global dotnet-ef   (si no lo tienes instalado)
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Ejecuta el proyecto:
   ```
   dotnet run
   ```

6. Abre el navegador en la URL que indique la consola (normalmente `https://localhost:7xxx`):
   - `/` → página principal
   - `/Identity/Account/Register` → crear cuenta
   - `/Goals`, `/Tasks`, `/Notes`, `/Reminders` → módulos de la app (requieren login)
   - `/swagger` → documentación interactiva de la API REST

## Estructura del proyecto (Arquitectura en Capas / MVC)

```
GoodGoals/
├── Models/              # Capa de Modelo (entidades: Goal, TaskItem, Note, Reminder, ApplicationUser)
├── Data/                # AppDbContext (EF Core)
├── Repositories/        # Repositorio genérico (acceso a datos)
├── Services/            # Capa de Lógica de Negocio (reglas por módulo)
├── Controllers/         # Capa de Presentación - Controladores MVC (vistas)
├── Controllers/Api/     # Capa de Presentación - Controladores de la API REST
└── Views/                # Capa de Presentación - Vistas Razor (.cshtml)
```

## API REST

Todos los endpoints requieren autenticación (cookie de Identity, vía login en el navegador).

| Recurso        | Endpoint base       |
|----------------|----------------------|
| Metas          | `/api/goals`         |
| Tareas         | `/api/tasks`          |
| Notas          | `/api/notes`          |
| Recordatorios  | `/api/reminders`      |

Cada uno soporta: `GET` (listar), `GET /{id}` (detalle), `POST` (crear), `PUT /{id}` (editar),
`DELETE /{id}` (eliminar). Metas y Tareas además tienen `PATCH /{id}/toggle` para
marcar como completada/pendiente.

Pruébalos directamente desde `/swagger` una vez que inicies sesión en la app
(la sesión de Identity se comparte porque la API usa autenticación por cookie).
