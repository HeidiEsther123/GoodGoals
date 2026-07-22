## ADR-07: Incorporación de Pruebas Unitarias e Integración Continua

| Campo | Valor |
|---|---|
| Autor | Heidi Esther Peña Betanzos |
| Fecha | 22/07/2026 |
| Estado | Aceptado |

### Contexto 🧸

Good Goals ya cuenta con una Arquitectura en Capas formal (ADR-03), una API REST documentada (ADR-04), y tres patrones de diseño GOF incorporados (ADR-05/ADR-06). Sin embargo, hasta ahora la única forma de verificar que el sistema seguía funcionando después de un cambio era probarlo manualmente, a mano, navegando la aplicación.

Esto genera un problema conocido: cada vez que se modifica una clase (por ejemplo, al refactorizar `GoalService` o ajustar `ReminderFactory`), no hay ninguna garantía automática de que el comportamiento siga siendo el esperado. "Funciona en mi máquina" no es una prueba, es una esperanza — y en un proyecto que ya mezcla lógica de negocio, patrones de diseño y una API REST, el riesgo de romper algo sin darse cuenta crece con cada entrega.

Las restricciones siguen siendo las mismas de ADRs anteriores: desarrollo individual, tiempo limitado, y necesidad de que el código sea fácil de sustentar.

### Decisión 🍇

Se decidió incorporar una suite de pruebas unitarias con **xUnit** y un pipeline de **Integración Continua (CI)** con GitHub Actions, que compila el proyecto y corre las pruebas automáticamente en cada `push` y en cada Pull Request.

**Clases seleccionadas para las pruebas:**

**1. `GoalService` (`Services/`)**
Es el servicio con más lógica de negocio propia sobre la entidad principal del sistema (Metas). Se probaron los casos más importantes:
- Que `GetUserGoalsAsync` solo regrese las metas del usuario dueño (aislamiento entre usuarios — regla de seguridad, no solo de datos).
- Que `GetByIdAsync` no permita acceder a una meta de otro usuario.
- Que `CreateAsync` asigne correctamente la fecha de creación.
- Que `ToggleCompletedAsync` invierta el estado y maneje el caso de un Id inexistente.
- Que `DeleteAsync` desvincule primero las Tareas asociadas antes de eliminar la Meta — este es el caso más delicado, ya que si se rompe, quedarían referencias huérfanas en la base de datos.

**2. `ReminderFactory` (`Patterns/Factory/`)**
Se eligió por ser lógica pura, sin dependencias externas (no toca base de datos ni otros servicios), lo que la hace ideal para aislar y verificar con pruebas rápidas. Se comprobó que cada `ReminderType` (`OneTime`, `Daily`, `Weekly`) calcule correctamente la fecha del recordatorio y anteponga el prefijo correspondiente al título — la regla de negocio central que la fábrica centraliza.

**Fakes en memoria en lugar de mocks:**
En vez de usar una librería de mocking, se construyó un `GenericRepositoryFake<T>` — una implementación real de `IGenericRepository<T>` que opera sobre una lista en memoria. Esto permite probar los servicios exactamente como usan el repositorio real, sin necesidad de una base de datos SQL Server durante las pruebas, manteniendo los tests rápidos y deterministas.

**Pipeline de CI:**
El workflow de GitHub Actions instala **dos versiones del SDK de .NET** (`8.0.x` para el proyecto principal `GoodGoals` y `10.0.x` para `GoodGoals.Tests`, ya que se creó con una versión más reciente del SDK), y ejecuta `dotnet restore`, `dotnet build` y `dotnet test` sobre la solución completa (`GoodGoals.slnx`) en cada push y pull request.

#### ¿Por qué estas dos clases primero?

`GoalService` representa la lógica de negocio "de siempre" (CRUD con reglas), mientras que `ReminderFactory` representa un patrón de diseño (Factory) documentado en el ADR-06. Probar ambas demuestra que la suite de pruebas no se limita a un solo tipo de código, sino que cubre tanto los servicios de negocio como los patrones GOF incorporados al sistema.

### Alternativas consideradas 🫐

| Alternativa | Por qué la descarté |
|---|---|
| Usar una librería de mocking (Moq/NSubstitute) | Un fake real en memoria es más simple de entender y mantener para un proyecto de este tamaño, y evita agregar una dependencia extra solo para simular repositorios sencillos. |
| Probar directamente contra SQL Server | Haría las pruebas lentas y dependientes de un entorno externo (requeriría una base de datos disponible en el pipeline de CI). Un fake en memoria mantiene las pruebas rápidas y reproducibles en cualquier máquina. |
| Empezar probando `LoggingGoalService` (Decorator) u `GoalEventManager` (Observer) | Ambos dependen de `ILogger` o de otros servicios (`ITaskService`), lo que requeriría fakes adicionales más complejos. Se priorizó primero `GoalService` y `ReminderFactory` por ser más autocontenidos, dejando los patrones Decorator/Observer para una siguiente iteración de pruebas. |
| Usar NUnit o MSTest en vez de xUnit | Los tres frameworks resuelven lo mismo (Arrange-Act-Assert); se eligió xUnit por ser el estándar más usado actualmente en proyectos .NET modernos. |

### Consecuencias 👀

**✅ Lo que gano:**

- **Consecuencia técnica:** Cualquier cambio futuro a `GoalService` o `ReminderFactory` que rompa su comportamiento esperado será detectado automáticamente por el pipeline, antes de llegar a producción — sin depender de pruebas manuales.
- **Consecuencia sobre el proceso:** El check verde/rojo de GitHub Actions en cada Pull Request da visibilidad inmediata del estado del proyecto, y sirve como evidencia objetiva y verificable de que el sistema compila y pasa sus pruebas en cada entrega.

**⚠️ Lo que sacrifico o asumo:**

- **Limitación técnica:** La suite actual cubre solo 2 de las clases del sistema (`GoalService` y `ReminderFactory`). `NoteService`, `TaskService`, `ReminderService`, `LoggingGoalService` y `GoalEventManager` todavía no tienen pruebas — quedan como trabajo pendiente.
- **Deuda o riesgo:** El proyecto principal usa .NET 8 y el proyecto de pruebas se generó en .NET 10 (versión del SDK disponible al momento de crearlo). Aunque el pipeline instala ambas versiones sin problema, sería más limpio homologar ambos proyectos a la misma versión de framework en el futuro.
- **Nota técnica:** Durante la configuración se detectó que el `.csproj` principal, al no tener exclusiones explícitas, compilaba por accidente los archivos del proyecto `GoodGoals.Tests` (por vivir en una subcarpeta de la misma raíz). Se corrigió agregando un `<ItemGroup>` con `<Compile Remove="GoodGoals.Tests\**" />` para excluir esa carpeta del proyecto principal.

### Dónde vive el código

```
GoodGoals.Tests/
├── Fakes/
│   └── GenericRepositoryFake.cs   → fake en memoria de IGenericRepository<T>
├── Services/
│   └── GoalServiceTests.cs        → pruebas de GoalService
└── Patterns/
    └── ReminderFactoryTests.cs    → pruebas de ReminderFactory (patrón Factory)

.github/workflows/
└── ci.yml                          → pipeline de GitHub Actions (build + test)
```
