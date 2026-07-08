# Modelo C4 — Good Goals

Documentación de la arquitectura del sistema Good Goals usando el Modelo C4 (Niveles 1, 2 y 3), escrita como código con Mermaid.

\---

## Nivel 1 — Contexto

**¿Para quién es?** Para cualquier persona (técnica o no) que quiera entender qué es el sistema y quién lo usa.

**¿Qué pregunta responde?** ¿Qué hace Good Goals y con qué sistemas externos se relaciona?

```mermaid
C4Context
    title Sistema Good Goals — Diagrama de Contexto (Nivel 1)

    Person(usuario, "Usuario", "Persona que desea organizar sus metas, tareas, notas y recordatorios para mejorar su productividad.")

    System(goodgoals, "Good Goals", "Aplicación web que centraliza metas, tareas, notas y recordatorios en un solo lugar, con autenticación de usuarios y API REST.")

    System\_Ext(sqlserver, "SQL Server", "Base de datos relacional donde se persisten todos los datos del sistema.")
    System\_Ext(browser, "Navegador Web", "Cliente desde donde el usuario accede a la aplicación.")

    Rel(usuario, browser, "Accede a la app desde")
    Rel(browser, goodgoals, "Envía peticiones HTTP/HTTPS")
    Rel(goodgoals, sqlserver, "Lee y escribe datos usando EF Core")
```



