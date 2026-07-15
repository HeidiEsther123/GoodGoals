## ADR-06: Incorporación de Patrones de Diseño GOF a Good Goals

| Campo | Valor |
|---|---|
| Autor | Heidi Esther Peña Betanzos |
| Fecha | 15/07/2026 |
| Estado | Aceptado |

### Contexto 🧸

Good Goals ya cuenta con una Arquitectura en Capas formal (ADR-03) y una API REST documentada con Swagger (ADR-04). La aplicación gestiona Metas, Tareas, Notas y Recordatorios para usuarios que buscan mejorar su productividad y disciplina personal.

A medida que el sistema crece, empiezan a surgir necesidades concretas que la arquitectura en capas por sí sola no resuelve de forma elegante:

- **Creación repetitiva de objetos complejos:** al registrar una nueva Meta, se necesita también crear un Recordatorio asociado con configuración específica (tipo, fecha, prefijo en el título). Hacer esto directamente en el controlador mezcla responsabilidades.
- **Comportamiento transversal sin modificar código existente:** se necesita registrar (logging) cada operación importante sobre las Metas (crear, editar, borrar) sin tocar el `GoalService` que ya funciona correctamente.
- **Reacciones automáticas a eventos:** cuando una Meta se marca como completada, sus Tareas asociadas deberían completarse automáticamente, sin que el controlador tenga que orquestar manualmente ese proceso.

Las restricciones siguen siendo las mismas: desarrollo individual, tiempo limitado, y necesidad de mantener el código limpio, extensible y fácil de sustentar.

### Decisión 🍇

Se ha decidido incorporar tres patrones de diseño del catálogo GOF (Gang of Four) al proyecto Good Goals, cada uno resolviendo uno de los problemas descritos en el contexto:

**1. Factory (Fábrica) — `Patterns/Factory/`**
Una clase `ReminderFactory` centraliza la creación de objetos `Reminder`. Al crear una Meta nueva, el controlador le pide a la fábrica un recordatorio del tipo deseado (`OneTime`, `Daily`, `Weekly`) y la fábrica se encarga de configurar la fecha, el prefijo del título y los demás atributos. El controlador no necesita saber cómo se construye cada tipo.

**2. Decorator (Decorador) — `Patterns/Decorator/`**
La clase `LoggingGoalService` envuelve al `GoalService` original e implementa la misma interfaz `IGoalService`. Cada vez que se llama a `CreateAsync`, `UpdateAsync`, `DeleteAsync` o `ToggleCompletedAsync`, el Decorator escribe un log automático antes y después de delegar la operación al servicio original. El `GoalService` no fue modificado en ninguna línea — el comportamiento extra se agrega "por encima", respetando el principio Open/Closed.

**3. Observer (Observador) — `Patterns/Observer/`**
Un `GoalEventManager` mantiene una lista de observadores registrados. Cuando una Meta se completa (`ToggleCompleted`), el controlador notifica al `GoalEventManager`, que a su vez llama a todos los observadores suscritos. El observador concreto `TaskCompletionObserver` recibe la notificación y marca automáticamente como completadas todas las Tareas asociadas a esa Meta. Si en el futuro se necesita agregar otro comportamiento (enviar un correo, actualizar estadísticas), basta con agregar un nuevo observador sin modificar el controlador.

#### ¿Por qué estos tres?

Los tres patrones se complementan naturalmente en Good Goals:
- **Factory** resuelve *cómo se crean* los objetos de forma consistente.
- **Decorator** resuelve *cómo se extiende* el comportamiento sin modificar código existente.
- **Observer** resuelve *cómo se comunican* las capas ante eventos importantes del dominio.

Juntos demuestran que la Arquitectura en Capas del ADR-03 es lo suficientemente flexible para acomodar patrones de diseño avanzados sin necesidad de reestructurar el sistema.

### Alternativas consideradas 🫐

| Alternativa | Por qué la descarté |
|---|---|
| Implementar solo un patrón (el más fácil) | No demostraría dominio real de los patrones GOF ni cubriría los tres problemas identificados. El requisito de la actividad pide mínimo 2, pero los 3 tienen aplicación concreta en el sistema. |
| Usar Mediator en vez de Observer | Mediator es más adecuado cuando hay muchos emisores y receptores desacoplados (como en CQRS). En Good Goals el flujo es simple y direccional (Meta → Tareas), por lo que Observer es más claro y directo. |
| Usar Strategy en vez de Factory | Strategy es ideal cuando el algoritmo completo cambia en tiempo de ejecución. Aquí solo varía la *configuración* del objeto creado (fecha, prefijo), no el comportamiento posterior, por lo que Factory es más apropiado. |
| Implementar Abstract Factory | Una fábrica abstracta crea familias de objetos relacionados. Good Goals solo necesita crear variantes de un único tipo (`Reminder`), por lo que la Factory simple es suficiente y más fácil de mantener. |

### Consecuencias 👀

**✅ Lo que gano:**

- **Consecuencia técnica:** El código queda más limpio y cada clase tiene una responsabilidad única y clara. Agregar un nuevo tipo de recordatorio solo requiere modificar `ReminderFactory`. Agregar un nuevo comportamiento al completar una Meta solo requiere agregar un nuevo `IGoalObserver`. El `GoalService` nunca necesita modificarse para agregar logging o efectos secundarios.
- **Consecuencia sobre el proceso:** Los patrones hacen que el código sea más fácil de explicar y defender en una sustentación, ya que cada patrón tiene un nombre reconocido en la industria y un problema específico que resuelve en el sistema.

**⚠️ Lo que sacrifico o asumo:**

- **Limitación técnica:** Los patrones agregan capas de indirección que pueden dificultar la lectura del código para alguien que no los conoce. Un desarrollador nuevo tendría que entender Factory, Decorator y Observer antes de poder modificar el flujo de creación de Metas y Recordatorios.
- **Deuda o riesgo:** Si el sistema crece significativamente, el `GoalEventManager` con una lista simple de observadores en memoria podría quedarse corto. En ese caso habría que migrar a un sistema de eventos más robusto (como un bus de eventos o un mensaje queue), lo cual implicaría refactorizar el patrón Observer a algo más escalable.

### Dónde vive el código

```
Patterns/
├── Factory/
│   ├── ReminderType.cs          → enum con los tipos de recordatorio
│   ├── IReminderFactory.cs      → contrato de la fábrica
│   └── ReminderFactory.cs       → implementación concreta
├── Decorator/
│   └── LoggingGoalService.cs    → decorador con logging para IGoalService
└── Observer/
    ├── IGoalObserver.cs         → interfaz observador
    ├── IGoalSubject.cs          → interfaz sujeto
    ├── GoalEventManager.cs      → gestor de eventos (sujeto concreto)
    └── TaskCompletionObserver.cs → observador concreto
```
# Deuda Técnica — Good Goals

Documento de identificación y propuesta de solución para las deudas técnicas reales detectadas en el proyecto Good Goals durante su desarrollo.

---

## Deuda Técnica #1 — Cadena de conexión expuesta en `appsettings.json`

> **Categoría:** Configuración / Infraestructura

### ¿Qué es?

La cadena de conexión a SQL Server está escrita directamente en el archivo `appsettings.json`, que forma parte del repositorio de GitHub. En un entorno de producción real, esta cadena incluiría el servidor, el usuario y la contraseña de la base de datos — datos sensibles que no deberían estar en el código fuente ni en el control de versiones.

### ¿Por qué existe?

Se tomó la decisión consciente de dejar la cadena de conexión en `appsettings.json` para agilizar el desarrollo y las pruebas locales durante el cuatrimestre. Configurar variables de entorno o un gestor de secretos requería tiempo adicional que se priorizó para implementar las funcionalidades del sistema.

### Costo de no pagarla

Si el proyecto se desplegara en producción con esta configuración, las credenciales de la base de datos quedarían expuestas en el historial de Git, accesibles para cualquier persona con acceso al repositorio. Un atacante podría conectarse directamente a la base de datos y leer, modificar o eliminar todos los datos de los usuarios. En entornos empresariales, esto representa una violación de políticas de seguridad que puede derivar en sanciones legales.

### Propuesta de solución

Migrar la cadena de conexión a variables de entorno usando el sistema de secretos de .NET para desarrollo local, y variables de entorno del servidor o un servicio como Azure Key Vault para producción. ASP.NET Core ya soporta esto de forma nativa — solo requiere mover el valor fuera del archivo y agregar los archivos de configuración de producción al `.gitignore`.

**Técnica de refactorización:** *Externalización de configuración* — mover parámetros sensibles fuera del código fuente hacia el entorno de ejecución.

---
## Deuda Técnica #2 — Autenticación de la API REST basada en cookies en lugar de JWT

> **Categoría:** Arquitectura / Seguridad

### ¿Qué es?

La API REST de Good Goals usa autenticación por cookie de sesión de ASP.NET Core Identity, en vez de tokens JWT (JSON Web Tokens). Esto significa que la API solo puede ser consumida desde el mismo navegador donde el usuario inició sesión — no desde una app móvil, un cliente externo o herramientas como Postman sin configuración adicional.

### ¿Por qué existe?

Se eligió autenticación por cookie porque ya estaba configurada con ASP.NET Core Identity para las vistas MVC, y reutilizarla para la API evitaba implementar un sistema de autenticación adicional. Dado el tiempo limitado del cuatrimestre y el desarrollo individual, esta decisión permitió tener la API funcionando rápidamente sin duplicar infraestructura. Esta deuda fue documentada explícitamente en el ADR-04 como un trade-off conocido y aceptado.

### Costo de no pagarla

La API no puede ser consumida por clientes externos como una app móvil u otro servidor. Escalar el sistema a una arquitectura donde el frontend y el backend estén separados se vuelve imposible sin refactorizar la autenticación. Si en el futuro se quiere exponer la API a terceros, toda la capa de seguridad tendría que reescribirse desde cero. Las pruebas automatizadas de los endpoints también se complican porque dependen de una sesión de navegador activa.

### Propuesta de solución

Agregar autenticación JWT como segunda opción, manteniendo las cookies para las vistas MVC para no romper lo que ya funciona. Esto implicaría instalar el paquete de autenticación JWT Bearer, configurarlo con los parámetros del token (emisor, audiencia, clave secreta desde variables de entorno), y agregar un endpoint de login exclusivo para la API que reciba correo y contraseña y devuelva un token. Los controladores de la API usarían ese esquema de autenticación en vez del genérico.

**Técnica de refactorización:** *Introducción de capa de autenticación desacoplada* — separar el mecanismo de autenticación de la sesión web del mecanismo de autorización de la API.

---

## Uso de Ai 

Yo heidi Esther Peña Betanzos no use Ai
