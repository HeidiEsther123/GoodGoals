# ADR-03: Selección del Estilo Arquitectónico para Good Goals

| Campo | Valor |
| :--- | :--- |
| **Autor** | Heidi Esther Peña Betanzos |
| **Fecha** | 12/06/2026 |
| **Estado** | Aceptado |
---

## Contexto

Estoy construyendo **Good Goals**, mi aplicación dirigida a personas que desean mejorar su productividad, organizar su día a día y desarrollar disciplina mediante herramientas integradas como calendario, agenda, seguimiento de objetivos, notas y recordatorios. También está pensada para personas distraídas o con dificultades para mantener el enfoque, ofreciéndoles un espacio centralizado que evita la necesidad de usar varias aplicaciones al mismo tiempo y facilita la constancia.

El problema principal que resuelve es la dispersión: actualmente los usuarios deben recurrir a diferentes apps para tareas, notas, hábitos y calendario, lo que fragmenta su organización. Good Goals reúne todo en un solo sistema, permitiendo una visión clara y continua de sus actividades.

Las restricciones que influyeron en esta decisión incluyen:
* **Tiempo limitado:** El cuatrimestre es bastante corto para la cantidad de módulos a integrar.
* **Desarrollo individual:** El proyecto es de carácter individual, lo que limita la velocidad de desarrollo en comparación con un equipo completo.
* **Tecnologías conocidas:** Uso de herramientas e infraestructura revisadas en clase (Patrón MVC, APIs, bases de datos relacionales).
* **Entregable:** Necesidad estricta de presentar un prototipo 100% funcional, limpio y mantenible al cierre de la tercera unidad.

---

## Decisión

## Decisión

Se ha decidido implementar el estilo de **Arquitectura en Capas**, organizado formalmente bajo el patrón **Modelo-Vista-Controlador (MVC)** para el desarrollo del sistema Good Goals.

### ¿Por qué?
Este estilo resuelve el problema central y las restricciones de desarrollo debido a sus características fundamentales:
  **Separación de Responsabilidades:** 
  Divide el sistema en tres niveles independientes (Presentación, Lógica y Datos). Esto evita que el código de los múltiples módulos (notas, agenda, metas) se mezcle, facilitando el orden y la claridad.
  **Cohesión y Fluidez:**
  Al estar las capas conectadas de forma directa, la comunicación entre las notas, los recordatorios y el calendario es inmediata, garantizando la experiencia unificada y sin retrasos que el usuario necesita.
  **Optimización del Desarrollo Individual:** 
  Reduce al mínimo el tiempo invertido en configuraciones complejas de infraestructura, permitiendo concentrar el esfuerzo de una sola persona en programar las reglas de negocio y las pantallas usando el estándar técnico aprendido en clase.

### Alternativas consideradas

*(Mínimo 3 filas)*

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| **Arquitectura de Microservicios** | Añade una alta complejidad de red, sincronización y gestión de múltiples bases de datos independientes. En un desarrollo individual con tiempo ajustado, el riesgo de no terminar el sistema era muy elevado. |
| **Arquitectura Hexagonal (Puertos y Adaptadores)** | Aunque ofrece un excelente aislamiento de la lógica de negocio, introduce demasiado código repetitivo (*boilerplate*), interfaces y mapeadores desde el inicio, lo que ralentizaría la velocidad de entrega requerida. |
| **Arquitectura Desacoplada (SPA + API independiente)** | Requiere desarrollar y desplegar dos proyectos por separado (un frontend web y un backend de manera independiente). Esto duplicaría el esfuerzo de configuración de servidores y autenticación bajo un calendario escolar corto. 

---

## Consecuencias

**✅ Lo que gano:**

Menciona al menos:
- Una consecuencia **técnica** — qué se vuelve más fácil de construir, mantener o escalar en tu sistema
- Una consecuencia sobre el **proceso o el equipo** — cómo afecta la forma en que vas a trabajar

**⚠️ Lo que sacrifico o asumo:**

Menciona al menos:
- Una **limitación técnica** — qué no podrás hacer fácilmente con esta decisión
- Una **deuda o riesgo** — qué podrías tener que resolver más adelante si el proyecto crece

## Diagrama

Un boceto de cómo se estructura tu sistema (draw.io, Mermaid o a mano escaneado)

![Diagrama del sistema]( ./ruta/diagrama-nivel-1.png )
