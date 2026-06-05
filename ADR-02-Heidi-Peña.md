# ADR-02: GoodGoals

| Campo  | Valor |
|--------|-------|
| Autor  | Heidi Esther Peña Betanzos|
| Fecha  | 05/06/2026 |
| Estado | `Aceptado` |

---

## Contexto

Estoy construyendo Good Goals, una aplicación dirigida a personas que desean mejorar su productividad, organizar su día a día y desarrollar disciplina mediante herramientas integradas como calendario, agenda, seguimiento de objetivos, notas y recordatorios. También está pensada para personas distraídas o con dificultades para mantener el enfoque, ofreciéndoles un espacio centralizado que evita la necesidad de usar varias aplicaciones al mismo tiempo y facilita la constancia.

El problema principal que resuelve es la dispersión: actualmente los usuarios deben recurrir a diferentes apps para tareas, notas, hábitos y calendario, lo que fragmenta su organización. Good Goals reúne todo en un solo sistema, permitiendo una visión clara y continua de sus actividades.

Las restricciones que influyen en esta decisión incluyen:

Tiempo limitado del cuatrimestre (bastante corto)

El proyecto es de carácter individual lo que dificulta la rapidez.

Tecnologías conocidas en clase (MVC, API, bases de datos, etc.)

Necesidad de entregar un prototipo funcional y mantenible

Como parte de la arquitectura del sistema, es necesario definir formalmente las vistas arquitectónicas que describen cómo se estructura, despliega y ejecutara Good Goals.

---

## Decisión
Definir y documentar las siguientes cuatro vistas arquitectónicas aplicadas al sistema Good Goals:

Vista lógica: muestra los módulos principales del sistema (controladores, modelos, vistas, servicios, entidades).

Vista física: describe los componentes físicos donde vive el sistema (servidor, base de datos, archivos estáticos).

Vista de despliegue: explica cómo se distribuye la aplicación en infraestructura (ambiente local y producción).

Vista de procesos: representa los flujos principales de ejecución (crear meta, actualizar tareas, autenticación, etc.).


### ¿Por qué?

Se necesita definir y documentar las siguientes cuatro vistas arquitectónicas que seran aplicadas en el sistema Good Goals:

Vista lógica: muestra los módulos principales del sistema (controladores, modelos, vistas, servicios, entidades).

Vista física: describe los componentes físicos donde vive el sistema (servidor, base de datos, archivos estáticos).

Vista de despliegue: explica cómo se distribuye la aplicación en infraestructura (ambiente local y producción).

Vista de procesos: representa los flujos principales de ejecución (crear meta, actualizar tareas, autenticación, etc.).

*(Mínimo 3 filas)*

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| Documentar solo la vista lógica| No cumple con los requisitos de la actividad y deja incompleta la arquitectura.           |
|Implementar un modelo C4 completo     | Excede el alcance del proyecto y el tiempo disponible; innecesario para un sistema pequeño.                |
|Documentar las vistas directamente en el README      | Aunque el README es útil para una visión general, no es el lugar adecuado para registrar decisiones arquitectónicas formales. Mezclar documentación técnica con instrucciones de uso genera desorden.   |

---

## Consecuencias

**✅ Lo que gano:**

Consecuencia técnica:  
Las vistas arquitectónicas permiten visualizar el sistema de forma clara, detectar dependencias, identificar riesgos y mantener una estructura ordenada y escalable.

Consecuencia sobre el proceso/equipo:  
Aunque el proyecto es individual, estas vistas facilitan explicar el sistema como si se trabajara en equipo, mejoran la documentación y permiten defender el diseño de forma profesional.

**⚠️ Lo que sacrifico o asumo:**

Limitación técnica:  
Las vistas deben actualizarse manualmente si el sistema cambia; pueden quedar desactualizadas si no se mantiene disciplina.

Deuda o riesgo:  
Si el proyecto crece, será necesario migrar a vistas más complejas (C4, microservicios, contenedores reales). La arquitectura actual podría quedarse bastante corta.

## Diagramas

### Vista lógica

<img width="8192" height="3053" alt="Goal Management-2026-06-05-064608" src="https://github.com/user-attachments/assets/88480d7f-d327-46c1-8b54-ac63066e4478" />

### Vista física
<img width="3628" height="1960" alt="Goal Management-2026-06-05-064514" src="https://github.com/user-attachments/assets/95c563bc-1fb5-452d-94bb-67e095877b84" />

### Vista de despliegue
<img width="3998" height="2075" alt="Goal Management-2026-06-05-064412" src="https://github.com/user-attachments/assets/0f2b4c94-8a12-4f58-ad17-8e0ec0dcebde" />

### Vista de procesos
<img width="678" height="8192" alt="Goal Management-2026-06-05-064336" src="https://github.com/user-attachments/assets/5674e9d1-af3b-4a87-9544-014ff975b2fb" />

## 🤖 Uso de IA
Yo Heidi Esther Peña Betanzos use IA para aprender a como crear un diagrama en Mermaid ya que se me dificulto. Use Gemini y el pront fue el siguiente "Necesito ayuda en mis diagramas se me esta haciendo engorroso usar draw.io y quiero probar Mermaid ¿como lo puedo usar?"


