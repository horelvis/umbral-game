# Umbral: El Eco del Alma

*Un walking simulator narrativo en primera persona sobre la opresión, la esperanza y la redención espiritual.*

---

## Índice

1.  [Concepto del Juego](#1-concepto-del-juego)
2.  [Mecánicas Principales](#2-mecánicas-principales)
    - [Miedo (Distorsión)](#miedo-distorsión)
    - [Fe (Recurso)](#fe-recurso)
    - [Culpa (Amenaza)](#culpa-amenaza)
3.  [Estructura Narrativa](#3-estructura-narrativa)
4.  [Estado Actual del Proyecto (Implementación Base)](#4-estado-actual-del-proyecto-implementación-base)
5.  [Cómo Empezar (Para Desarrolladores)](#5-cómo-empezar-para-desarrolladores)

---

## 1. Concepto del Juego

**"Umbral: El Eco del Alma"** es una experiencia narrativa en primera persona ambientada en la dimensión espiritual "el Umbral". Inspirado en la atmósfera de juegos como *Amnesia* y la temática del libro "Nuestro Hogar" de Chico Xavier, el juego se centra en la redención.

El jugador es un alma atrapada que despierta en este páramo gris y cambiante. Guiado por seres de luz, deberá enfrentarse a sus culpas, miedos y deseos para poder ascender a planos superiores. El objetivo no es combatir, sino comprender, ocultarse y usar la luz interior como herramienta de sanación y no de agresión.

## 2. Mecánicas Principales

La jugabilidad se centra en la gestión de tres recursos interconectados que definen el estado espiritual del protagonista.

### Miedo (Distorsión)
- **Función:** No es una barra de vida, sino una medida de la estabilidad mental del protagonista.
- **Efectos:** A medida que aumenta, el mundo se distorsiona visual y auditivamente. Los controles pueden volverse más pesados y la percepción alterarse.
- **Gestión:** Aumenta al ser perseguido o presenciar eventos aterradores. Disminuye en "Refugios de Luz" o al resolver conflictos internos.

### Fe (Recurso)
- **Función:** Es el "combustible" espiritual del jugador para usar su **Luz Interior**.
- **Usos:**
    1.  **Luz Sostenida:** Un aura de luz suave para navegar en la oscuridad que consume Fe lentamente.
    2.  **Pulso de Claridad:** Un destello intenso para desorientar sombras, purificar "Nudos de Culpa" o revelar caminos. Consume una gran cantidad de Fe.
- **Gestión:** Se recupera al tomar decisiones altruistas o encontrar "Ecos de Esperanza".

### Culpa (Amenaza)
- **Función:** Actúa como un faro que atrae a las entidades oscuras del Umbral. No es una barra, sino una serie de "Nudos de Culpa" narrativos que el jugador arrastra.
- **Efectos:** Cada nudo no resuelto atrae a tipos específicos de enemigos y genera alucinaciones temáticas.
- **Gestión:** Se resuelve enfrentando los recuerdos asociados a la culpa a través de puzzles simbólicos, eliminando permanentemente la amenaza asociada.

## 3. Estructura Narrativa

La historia se divide en cinco capítulos:

1.  **Despertar:** El tutorial, donde el jugador descubre el mundo y sus mecánicas básicas.
2.  **Valle de los Lamentos:** Un laberinto de almas errantes y las primeras tentaciones ilusorias.
3.  **Los Señores del Dolor:** Encuentros con entidades poderosas que ofrecen poder a cambio de sumisión moral.
4.  **Río de Fuego:** Un viaje tenso donde las culpas emergen como espectros para poner a prueba la fe del jugador.
5.  **Juicio Interior / Ascenso:** El enfrentamiento final con la propia sombra, llevando a finales ramificados.

## 4. Estado Actual del Proyecto (Implementación Base)

Este repositorio contiene la **base de código C# inicial** para el proyecto en Unity. Se han implementado los sistemas centrales de forma modular y listos para ser conectados a assets visuales y de audio.

Todo el código reside bajo el namespace `Umbral`.

-   **`Assets/Scripts/Core/PlayerState.cs`**: El script más importante. Gestiona los valores de Miedo, Fe y la lista de Culpas no resueltas.
-   **`Assets/Scripts/Player/PlayerMovement.cs`**: Un controlador en primera persona que usa `CharacterController` para el movimiento y la rotación de la cámara.
-   **`Assets/Scripts/Player/InnerLight.cs`**: Gestiona la mecánica de Luz Interior, el consumo de Fe y los inputs del jugador para la luz sostenida y el pulso.
-   **`Assets/Scripts/Systems/FearSystem.cs`**: Un script "puente" que lee el nivel de Miedo del `PlayerState` y contiene la lógica para modular efectos visuales y de audio (actualmente simulado con `Debug.Log`).
-   **`Assets/Scripts/Systems/GuiltKnot.cs`**: Un `ScriptableObject` que permite a los diseñadores crear "Nudos de Culpa" como assets, definiendo su ID y qué enemigo atraen.
-   **`Assets/Scripts/Systems/GuiltSystem.cs`**: El gestor que lee las culpas del jugador y utiliza los `GuiltKnot` para simular la aparición de enemigos.

## 5. Cómo Empezar (Para Desarrolladores)

1.  Clona o descarga este repositorio.
2.  Abre la carpeta como un proyecto de Unity.
3.  Crea un `GameObject` vacío llamado **"Player"**.
    -   Añádele un componente `CharacterController`.
    -   Añade los scripts: `PlayerState.cs`, `PlayerMovement.cs`, y `InnerLight.cs`.
    -   Crea una `Camera` como objeto hijo del Player y arrástrala al campo correspondiente en el script `PlayerMovement`.
    -   Crea dos `Light` como hijos del Player (una para la luz sostenida, otra para el pulso) y arrástralas a los campos del script `InnerLight`.
4.  Crea un `GameObject` vacío llamado **"GameSystems"**.
    -   Añade los scripts `FearSystem.cs` y `GuiltSystem.cs`.
    -   Arrastra el "Player" al campo `_playerState` en ambos scripts.
    -   Para el `GuiltSystem`, crea algunos assets de `GuiltKnot` (`Assets > Create > Umbral > Guilt Knot`) y asígnalos a la lista en el inspector.
5.  ¡Entra en modo Play para probar la funcionalidad básica!
