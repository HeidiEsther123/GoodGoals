# ADR-02: GoodGoals

| Campo  | Valor |
|--------|-------|
| Autor  | Heidi Esther Peña Betanzos|
| Fecha  | 05/06/2026 |
| Estado | `Aceptado` |

---

## Contexto

¿Qué estás construyendo, qué problema resuelve y para quién es? Describe también las condiciones o restricciones que influyeron en esta decisión — por ejemplo, el tiempo disponible, el equipo, las tecnologías que ya conoces o las que viste en clase.

---

## Decisión



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


