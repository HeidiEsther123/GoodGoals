# ADR-01: GooodGoals

| Campo  | Valor |
|--------|-------|
| Autor  | Heidi Esther Peña Betanzos |
| Fecha  | 14/05/2026 |
| Estado | `Propuesto`|

---

## Contexto

Estoy construyendo Good Goals, una aplicación dirigida a personas que desean mejorar su productividad, organizar su día a día y desarrollar disciplina mediante herramientas integradas como calendario, agenda, seguimiento de objetivos, notas y recordatorios. También está pensada para personas distraídas o con dificultades para mantener el enfoque, ofreciéndoles un espacio centralizado que evita la necesidad de usar varias aplicaciones al mismo tiempo y facilita la constancia.

El problema principal que resuelve es la dispersión. Actualmente, los usuarios deben recurrir a diferentes apps para tareas, notas, hábitos y calendario, lo que fragmenta su organización. Good Goals reúne todo en un solo sistema, permitiendo una visión clara y continua de sus actividades. 

- Las restricciones que influyen en esta decisión incluyen:

- Tiempo limitado del cuatrimestre(dastante corto)

- El proyecto es de caracter invididual

- Tecnologías conocidas en clase (MVC, API, bases de datos relacionales,etc)

- Necesidad de entregar un prototipo funcional y mantenible
---

## Decisión

Backend: ASP.NET Core

Frontend: MVC

Base de datos: SQL Server

Estilo arquitectónico: MVC

Patrón: Capas (Controllers, Models, Views)

### ¿Por qué?

Decidí construir Good Goals utilizando ASP.NET Core como tecnología principal para el backend, ya que permite desarrollar aplicaciones web modernas y escalables con C#. Para el frontend elegí el enfoque MVC, aprovechando las vistas y la estructura que ofrece este patrón dentro del propio framework. La base de datos seleccionada es SQL Server, porque se integra de forma natural con .NET y facilita el manejo de información estructurada. El estilo arquitectónico elegido es MVC y el proyecto está organizado mediante un patrón por capas que separa controladores, modelos y vistas, lo que permite mantener el código ordenado, claro y fácil de extender.
(ademas es lo que he aprendido en esta unidad y me gustaria reforzarlo)
### Alternativas consideradas

*(Mínimo 3 filas)*

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| Django + PostgreSQL | Requiere más configuración inicial y curva de aprendizaje mayor para el tiempo disponible.  | 
| Firebase | Muy rápido para prototipos, pero genera dependencia fuerte del proveedor. |
| Spring Boot con Java | lo consideré porque es un framework robusto y muy usado en la industria, pero lo descarté ya que requiere trabajar en un ecosistema completamente distinto al de .NET. a  mi gusto es mas tedioso, además de que su integración con SQL Server no es tan natural como en ASP.NET Core. |

---

## Consecuencias

**✅ Lo que gano:**

Consecuencia técnica:  
Con ASP.NET Core, MVC y SQL Server puedo mantener una estructura clara, escalable y fácil de extender. La separación por capas permite agregar nuevas entidades como hábitos, estadísticas o rutinas sin afectar otras partes del sistema.

Consecuencia sobre el proceso/equipo:  
La arquitectura MVC facilita trabajar de forma ordenada, probar cada parte por separado y mantener el proyecto organizado.

**⚠️ Lo que sacrifico o asumo:**

Limitación técnica:  
ASP.NET Core puede requerir más configuración inicial que otras tecnologías más simples, y SQL Server puede ser menos flexible si en el futuro se necesitan datos no estructurados.

Deuda o riesgo:  
Mantener el patrón MVC implica organizar correctamente cada capa, lo que requiere disciplina durante el desarrollo. Si el proyecto escala, será necesario reforzar temas como autenticación, seguridad y validaciones.

## Diagrama

Un boceto de cómo se estructura tu sistema (draw.io, Mermaid o a mano escaneado)

<img width="122" height="392" alt="Diagrama sin título drawio (1)" src="https://github.com/user-attachments/assets/7ecc828f-9f0c-4a83-bfbe-8cac6914a0e5" />


