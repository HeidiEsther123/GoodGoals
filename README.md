### Universidad: Tecnologico de Software
### Materia: Arquitectura de Software
### Maestro: Jorge Javier Pedroza Romero
### Alumno: Heidi Esther Peña Betanzos
### Grado: 3B

Aplicación web de productividad personal construida con **ASP.NET Core MVC + Web API + Entity Framework Core + SQL Server**, siguiendo una Arquitectura en Capas formal e incorporando patrones de diseño GOF, pruebas unitarias e integración continua.
---

## ¿Qué es Good Goals?

Good Goals es una herramienta diseñada para personas que desean mejorar su productividad y fortalecer su disciplina personal. Está especialmente pensada para ser accesible y amigable con quienes presentan déficit de atención. Reúne en un solo lugar calendario, agenda, seguimiento de objetivos, notas y recordatorios:

- 🎯 **Metas** — define objetivos con fecha límite y seguimiento de avance
- ✅ **Tareas** — organiza actividades del día a día, asociadas opcionalmente a una meta
- 🗒️ **Notas** — espacio libre para ideas, apuntes y reflexiones
- ⏰ **Recordatorios** — avisos programados para no olvidar nada importante

---
## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (proyecto principal)
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (proyecto de pruebas)
- SQL Server o SQL Server LocalDB (incluido con Visual Studio)
- Visual Studio 2022/2026 o VS Code

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
├── GoodGoals.Tests/     # Suite de pruebas xUnit
│   ├── Fakes/           # GenericRepositoryFake<T> (repositorio en memoria)
│   ├── Services/        # Pruebas de GoalService
│   └── Patterns/        # Pruebas de ReminderFactory
├── .github/workflows/   # Pipeline de GitHub Actions (CI)
└── wwwroot/             # Archivos estáticos (CSS, JS)
```
---
## API REST

Todos los endpoints requieren autenticación. Documentación disponible en `/swagger`.

| Recurso | Endpoint base | Métodos |
|---|---|---|
| Metas | `/api/goals` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id}, PATCH/{id}/toggle |
| Tareas | `/api/tasks` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id}, PATCH/{id}/toggle |
| Notas | `/api/notes` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id} |
| Recordatorios | `/api/reminders` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id} |

---

## Patrones de Diseño GOF implementados

### 🏭 Factory — `Patterns/Factory/`
Centraliza la creación de `Reminder`. Al crear una Meta, la `ReminderFactory` genera automáticamente un recordatorio semanal asociado, con el tipo, fecha y prefijo configurados de forma consistente.

### 🎨 Decorator — `Patterns/Decorator/`
`LoggingGoalService` envuelve al `GoalService` original sin modificarlo. Cada operación sobre Metas (crear, editar, borrar, completar) genera automáticamente un log, respetando el principio Open/Closed.

### 👁️ Observer — `Patterns/Observer/`
Cuando una Meta se completa, el `GoalEventManager` notifica al `TaskCompletionObserver`, que automáticamente marca como completadas todas las Tareas asociadas a esa Meta.

---

## Pruebas unitarias e Integración Continua

### 🧪 Pruebas — `GoodGoals.Tests/`
Suite de pruebas con **xUnit** sobre `GoodGoals.Tests`, cubriendo:

- **`GoalService`** — filtrado de metas por usuario, seguridad (no acceder a metas ajenas), creación, cambio de estado (toggle), y eliminación con desvinculación de tareas asociadas.
- **`ReminderFactory`** (patrón Factory) — cálculo correcto de fecha y prefijo de título según el tipo de recordatorio (`OneTime`, `Daily`, `Weekly`).

Las pruebas usan un `GenericRepositoryFake<T>` — una implementación en memoria de `IGenericRepository<T>` — en lugar de una base de datos real, para mantenerlas rápidas y deterministas.

Para correrlas localmente:
```bash
dotnet test GoodGoals.slnx
```

### ⚙️ Integración Continua — `.github/workflows/ci.yml`
Cada `push` y Pull Request dispara un workflow de GitHub Actions que:

1. Restaura los paquetes NuGet
2. Compila la solución completa (`GoodGoals.slnx`)
3. Corre la suite de pruebas xUnit

El resultado (✅ o ❌) aparece como check en cada Pull Request, dando visibilidad inmediata de si un cambio rompió algo.

---

## Decisiones arquitectónicas (ADRs)

| ADR | Decisión |
|---|---|
| [ADR-01](ADR-01-Heidi%20Esther-Peña%20Betanzos.md) | Elección de tecnología: ASP.NET Core MVC + SQL Server |
| [ADR-02](ADR-02-Heidi-Peña.md) | Definición de las 4 vistas arquitectónicas |
| [ADR-03](ADR-03-Heidi-Peña.md) | Arquitectura en Capas formal (Servicios + Repositorios) |
| ADR-04 | Incorporación de la API REST con Swagger |
| ADR-05 | Incorporación de patrones de diseño GOF (Factory, Decorator, Observer) |
| ADR-07 | Incorporación de pruebas unitarias e integración continua |

---

## Ramas del repositorio

| Rama | Contenido |
|---|---|
| `Mian` | ADR-01 + prototipo inicial MVC  + ADR-02 + diagramas de las 4 vistas + ADR-03 + Repositories/ + Services/ |
| `Api` | + ADR-04 + Controllers/Api/ + Swagger |
| `gof` | + ADR-05 + Patterns/ (Factory, Decorator, Observer) |
| `pruebas-unitarias` | + ADR-07 + GoodGoals.Tests/ + .github/workflows/ci.yml |

## Uso de AI
