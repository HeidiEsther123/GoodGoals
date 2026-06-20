## ADR-04: Incorporación de una API REST a Good Goals

| Campo | Valor |
|---|---|
| Autor | Heidi Esther Peña Betanzos |
| Fecha | 19/06/2026 |
| Estado | Aceptado |

### Contexto 🧸

Good Goals es mi aplicación dirigida a personas que desean mejorar su productividad, organizar su día a día y desarrollar disciplina mediante herramientas integradas como calendario, agenda, seguimiento de objetivos, notas y recordatorios. En los ADR anteriores (ADR-01, ADR-02 y ADR-03) ya habia definido que el sistema usa ASP.NET Core con arquitectura en capas bajo el patrón MVC, separando Presentación (Vistas Razor), Lógica de Negocio (Servicios) y Datos (Repositorios + EF Core + SQL Server).

Hasta este punto, la única forma de interactuar con el sistema era a través de las Vistas Razor renderizadas en el servidor. Sin embargo, surge la necesidad de que Good Goals también pueda comunicarse mediante un canal basado en datos (JSON), no solo en HTML. Esto responde a:

- **Consumo externo de los datos**: que otros clientes (una futura app móvil, un cliente de escritorio, o llamadas externas) puedan leer y modificar Metas, Tareas, Notas y Recordatorios sin depender de que el servidor genere HTML.
- **Verificación de la arquitectura por capas**: demostrar que la separación entre Presentación, Lógica de Negocio y Datos definida en el ADR-03 realmente funciona, ya que un nuevo "cliente" (la API) puede conectarse a la misma Capa de Servicios sin tocar ni duplicar las reglas de negocio.
- **Restricciones del proyecto**: tiempo de cuatrimestre corto, desarrollo individual y la necesidad de reutilizar al máximo el código ya construido para no duplicar lógica entre la app web y la API.

### Decisión 🍇

Se ha decidido incorporar una **API REST** dentro del mismo proyecto ASP.NET Core, implementada con controladores `[ApiController]` independientes de los controladores MVC, pero que **reutilizan la misma Capa de Servicios** (GoalService, TaskService, NoteService, ReminderService) ya definida en el ADR-03.

La API expone los siguientes recursos, todos protegidos con autenticación:

| Recurso | Endpoint base | Operaciones |
|---|---|---|
| Metas | `/api/goals` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id}, PATCH/{id}/toggle |
| Tareas | `/api/tasks` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id}, PATCH/{id}/toggle |
| Notas | `/api/notes` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id} |
| Recordatorios | `/api/reminders` | GET, GET/{id}, POST, PUT/{id}, DELETE/{id} |

La autenticación se maneja con **ASP.NET Core Identity** (cookies), de modo que la sesión iniciada en la web es la misma que valida las peticiones a la API, sin necesidad de implementar un sistema de tokens adicional. La documentación interactiva se genera automáticamente con **Swagger/OpenAPI**.

#### ¿Por qué?

- **Reutilización total de la lógica de negocio**: los controladores de la API no repiten reglas; simplemente inyectan la misma interfaz de Servicio que usan los controladores MVC (`IGoalService`, `ITaskService`, etc.). Esto es justamente lo que la Arquitectura en Capas del ADR-03 hace posible: la Presentación cambia (HTML vs. JSON), pero la Lógica de Negocio y los Datos permanecen intactos.
- **Bajo costo de implementación**: al estar dentro del mismo proyecto y compartir Program.cs, DbContext y Servicios, no se necesita levantar un proyecto adicional, ni configurar autenticación distinta, lo cual respeta la restricción de tiempo corto y desarrollo individual.
- **Consistencia con el patrón MVC ya aceptado**: ASP.NET Core permite que los controladores `[ApiController]` convivan naturalmente junto a los controladores `Controller` tradicionales dentro de la misma capa de Presentación, sin romper la separación de responsabilidades.
- **Documentación automática**: usar Swagger evita tener que escribir documentación manual de cada endpoint, lo cual era importante dado el tiempo disponible.

### Alternativas consideradas 🫐

| Alternativa | Por qué la descarté |
|---|---|
| Proyecto de API separado (Web API independiente del proyecto MVC) | Duplicaría el DbContext, los Servicios y la configuración de autenticación, lo que generaría dos copias de la misma lógica de negocio. Con tiempo y recursos individuales limitados, el riesgo de desincronización entre ambos proyectos era muy alto. |
| Autenticación basada en JWT (tokens) | Es el estándar para APIs públicas consumidas por apps externas, pero agrega complejidad de configuración (emisión, expiración y renovación de tokens) que no aporta valor inmediato cuando el primer consumidor de la API es la propia sesión web. Queda como mejora futura si se conecta una app móvil real. |
| No implementar API y mantener solo MVC | Incumple el requisito de la actividad de mostrar la incorporación de una API, y limita al sistema a un solo tipo de cliente (el navegador), reduciendo su capacidad de extensión futura. |

### Consecuencias 👀

**✅ Lo que gano:**

- **Consecuencia técnica:** Good Goals ahora tiene dos puntos de entrada (Vistas MVC y API REST) que comparten exactamente la misma Capa de Servicios y de Datos, validando en la práctica que la Arquitectura en Capas definida en el ADR-03 separa correctamente sus responsabilidades.
- **Consecuencia sobre el proceso:** Cualquier mejora futura en la lógica de negocio (por ejemplo, nuevas validaciones en Tareas) se escribe una sola vez en el Servicio y automáticamente queda disponible tanto para la web como para la API, sin duplicar trabajo.

**⚠️ Lo que sacrifico o asumo:**

- **Limitación técnica:** Al usar autenticación por cookie en lugar de JWT, la API en su forma actual está pensada para ser consumida desde el mismo dominio/sesión del navegador; no está lista todavía para ser consumida por una app móvil o un cliente externo sin antes incorporar un esquema de tokens.
- **Deuda o riesgo:** Si en el futuro se necesita exponer la API públicamente a terceros, será necesario revisar el modelo de autenticación (migrar a JWT o API Keys) y añadir control de versiones de la API (`/api/v1/...`), algo que por restricciones de tiempo no se abordó en esta entrega.

## 📸 Capturas 📸

<img width="1910" height="918" alt="Captura de pantalla 2026-06-19 201218" src="https://github.com/user-attachments/assets/9ed14214-5e5f-4b15-b447-2bf9c9095b43" />
<img width="1212" height="898" alt="Captura de pantalla 2026-06-19 215319" src="https://github.com/user-attachments/assets/b7126a69-d0c4-4da1-8cea-5897deaf2fc2" />

## 🤖 Uso AI 🤖

Yo Heidi Esther Peña Betanzos use inteligencia artificial para resolver problemas criticos que no pude arreglar por cuenta propia.
