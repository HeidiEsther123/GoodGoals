# ADR-01: [Título corto de la decisión]

| Campo  | Valor |
|--------|-------|
| Autor  | Heidi Esther Peña Betanzos|
| Fecha  | DD/MM/AAAA |
| Estado | `Aceptado` |

---

## Contexto

¿Qué estás construyendo, qué problema resuelve y para quién es? Describe también las condiciones o restricciones que influyeron en esta decisión — por ejemplo, el tiempo disponible, el equipo, las tecnologías que ya conoces o las que viste en clase.

---

## Decisión

Estoy construyendo Good Goals, una aplicación dirigida a personas que desean mejorar su productividad, organizar su día a día y desarrollar disciplina mediante herramientas integradas como calendario, agenda, seguimiento de objetivos, notas y recordatorios. También está pensada para personas distraídas o con dificultades para mantener el enfoque, ofreciéndoles un espacio centralizado que evita la necesidad de usar varias aplicaciones al mismo tiempo y facilita la constancia.

El problema principal que resuelve es la dispersión: actualmente los usuarios deben recurrir a diferentes apps para tareas, notas, hábitos y calendario, lo que fragmenta su organización. Good Goals reúne todo en un solo sistema, permitiendo una visión clara y continua de sus actividades.

Las restricciones que influyen en esta decisión incluyen:

Tiempo limitado del cuatrimestre (bastante corto)

El proyecto es de carácter individual por ende debo de prestarle mas atencion

Tecnologías conocidas en clase (MVC, API, bases de datos relacionales, etc.)

Necesidad de entregar un prototipo funcional y mantenible

Como parte de la arquitectura del sistema, es necesario definir formalmente las vistas arquitectónicas que describen cómo se estructura, despliega y ejecuta Good Goals.

### ¿Por qué?

Decidí documentar estas vistas porque permiten entender el sistema desde diferentes perspectivas y facilitan la comunicación de la arquitectura. Cada vista responde a una pregunta distinta:

Lógica: ¿qué componentes existen y cómo se relacionan?

Física: ¿en qué artefactos o máquinas vive el sistema?

Despliegue: ¿cómo se distribuye la aplicación en los entornos?

Procesos: ¿cómo fluye la información dentro del sistema?

### Alternativas consideradas

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

Un boceto de cómo se estructura tu sistema (draw.io, Mermaid o a mano escaneado)

![Diagrama del sistema]( ./ruta/diagrama-nivel-1.png )
