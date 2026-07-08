# Modelo C4 — Good Goals

Documentación de la arquitectura del sistema Good Goals usando el Modelo C4 (Niveles 1, 2 y 3), escrita como código con Mermaid.

---

## Nivel 1 — Contexto

**¿Para quién es?** Para cualquier persona (técnica o no) que quiera entender qué es el sistema y quién lo usa.

**¿Qué pregunta responde?** ¿Qué hace Good Goals y con qué sistemas externos se relaciona?

```mermaid
C4Context
    title Sistema Good Goals — Diagrama de Contexto (Nivel 1)

    Person(usuario, "Usuario", "Persona que desea organizar sus metas, tareas, notas y recordatorios para mejorar su productividad.")

    System(goodgoals, "Good Goals", "Aplicación web que centraliza metas, tareas, notas y recordatorios en un solo lugar, con autenticación de usuarios y API REST.")

    System_Ext(sqlserver, "SQL Server", "Base de datos relacional donde se persisten todos los datos del sistema.")
    System_Ext(browser, "Navegador Web", "Cliente desde donde el usuario accede a la aplicación.")

    Rel(usuario, browser, "Accede a la app desde")
    Rel(browser, goodgoals, "Envía peticiones HTTP/HTTPS")
    Rel(goodgoals, sqlserver, "Lee y escribe datos usando EF Core")
```

---
---
Nivel 2 — Contenedores
¿Para quién es? Para desarrolladores y arquitectos que necesitan entender las piezas técnicas principales del sistema.
¿Qué pregunta responde? ¿Cuáles son los contenedores (aplicaciones, bases de datos) que componen Good Goals y cómo se comunican entre sí?
```mermaid
C4Container
    title Good Goals — Diagrama de Contenedores (Nivel 2)

    Person(usuario, "Usuario", "Accede a la aplicación desde el navegador.")

    Container_Boundary(goodgoals, "Good Goals") {
        Container(webapp, "Aplicación Web MVC", "ASP.NET Core MVC / .NET 8", "Sirve las vistas Razor al navegador. Maneja autenticación con Identity y expone la API REST documentada con Swagger.")
        Container(api, "API REST", "ASP.NET Core Web API", "Expone endpoints JSON para Metas, Tareas, Notas y Recordatorios. Reutiliza la misma capa de Servicios que el MVC.")
        Container(identity, "Identity", "ASP.NET Core Identity", "Gestiona el registro, login y autenticación de usuarios mediante cookies.")
    }

    ContainerDb(db, "GoodGoalsDb", "SQL Server", "Almacena usuarios, metas, tareas, notas y recordatorios. Accedida mediante Entity Framework Core.")

    Rel(usuario, webapp, "Usa la interfaz web", "HTTPS")
    Rel(usuario, api, "Puede consumir la API", "HTTPS / JSON")
    Rel(webapp, identity, "Delega autenticación a")
    Rel(webapp, db, "Lee y escribe datos", "EF Core / SQL")
    Rel(api, db, "Lee y escribe datos", "EF Core / SQL")
    Rel(identity, db, "Persiste usuarios y sesiones", "EF Core / SQL")
```
---
Nivel 3 — Componentes
¿Para quién es? Para el equipo de desarrollo que trabaja directamente en el código del sistema.
¿Qué pregunta responde? ¿Qué hay dentro de la aplicación web principal? ¿Cómo se organizan los controladores, servicios, repositorios y patrones GOF?
```mermaid
C4Component
    title Good Goals — Diagrama de Componentes (Nivel 3)

    Person(usuario, "Usuario", "Interactúa con la aplicación.")

    Container_Boundary(webapp, "Aplicación Web — ASP.NET Core") {

        Component(goalsCtrl, "GoalsController", "ASP.NET Core MVC Controller", "Maneja las vistas y acciones CRUD de Metas. Usa Observer al completar una meta y Factory al crearla.")
        Component(tasksCtrl, "TasksController", "ASP.NET Core MVC Controller", "Maneja las vistas y acciones CRUD de Tareas.")
        Component(notesCtrl, "NotesController", "ASP.NET Core MVC Controller", "Maneja las vistas y acciones CRUD de Notas.")
        Component(remindersCtrl, "RemindersController", "ASP.NET Core MVC Controller", "Maneja las vistas y acciones CRUD de Recordatorios.")

        Component(goalsApi, "GoalsApiController", "ASP.NET Core ApiController", "Expone endpoints REST de Metas en /api/goals.")
        Component(tasksApi, "TasksApiController", "ASP.NET Core ApiController", "Expone endpoints REST de Tareas en /api/tasks.")
        Component(notesApi, "NotesApiController", "ASP.NET Core ApiController", "Expone endpoints REST de Notas en /api/notes.")
        Component(remindersApi, "RemindersApiController", "ASP.NET Core ApiController", "Expone endpoints REST de Recordatorios en /api/reminders.")

        Component(goalService, "LoggingGoalService", "Decorator — Patrón GOF Estructural", "Envuelve a GoalService y agrega logging automático en cada operación sin modificar el servicio original.")
        Component(taskService, "TaskService", "Service — Lógica de Negocio", "Reglas de negocio de Tareas.")
        Component(noteService, "NoteService", "Service — Lógica de Negocio", "Reglas de negocio de Notas.")
        Component(reminderService, "ReminderService", "Service — Lógica de Negocio", "Reglas de negocio de Recordatorios.")

        Component(factory, "ReminderFactory", "Factory — Patrón GOF Creacional", "Crea objetos Reminder configurados según el tipo (OneTime, Daily, Weekly) al crear una nueva Meta.")
        Component(observer, "GoalEventManager", "Observer — Patrón GOF de Comportamiento", "Notifica a los observadores cuando una Meta se completa. TaskCompletionObserver marca las Tareas asociadas como completadas.")

        Component(repo, "GenericRepository<T>", "Repository — Acceso a Datos", "Repositorio genérico que abstrae las operaciones de EF Core para cualquier entidad.")
    }

    ContainerDb(db, "GoodGoalsDb", "SQL Server", "Base de datos relacional.")

    Rel(usuario, goalsCtrl, "Gestiona metas desde la UI")
    Rel(usuario, goalsApi, "Consulta metas via API")

    Rel(goalsCtrl, goalService, "Delega lógica a")
    Rel(tasksCtrl, taskService, "Delega lógica a")
    Rel(notesCtrl, noteService, "Delega lógica a")
    Rel(remindersCtrl, reminderService, "Delega lógica a")

    Rel(goalsApi, goalService, "Delega lógica a")
    Rel(tasksApi, taskService, "Delega lógica a")
    Rel(notesApi, noteService, "Delega lógica a")
    Rel(remindersApi, reminderService, "Delega lógica a")

    Rel(goalsCtrl, factory, "Usa al crear una meta")
    Rel(goalsCtrl, observer, "Notifica al completar una meta")

    Rel(goalService, repo, "Accede a datos via")
    Rel(taskService, repo, "Accede a datos via")
    Rel(noteService, repo, "Accede a datos via")
    Rel(reminderService, repo, "Accede a datos via")
    Rel(factory, reminderService, "Delega creación a")

    Rel(repo, db, "Lee y escribe", "EF Core / SQL")
```




