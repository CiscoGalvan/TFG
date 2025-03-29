# ***Manual de Uso para Framework de comportamientos de enemigos para videojuegos 2D***
***Bienvenido al manual de uso.***  
**Creadores:** Cristina Mora Velasco y Francisco Miguel Galván Muñoz  
**Fecha:** Marzo de 2025

## Índice
- [Introducción](#introducción)
- [Objetivo](#objetivo)
- [Funcionalidad](#funcionalidad)
- [Público Objetivo](#público-objetivo)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Contenido del Paquete](#Contenido-del-Paquete)
- [Componentes del Framework](#componentes-del-framework)
  - [Sensores](#sensores)
  - [Actuadores](#actuadores)
  - [Estado (State)](#estado-state)
  - [Máquina de Estados Finita (FSM)](#máquina-de-estados-finita-fsm)
  - [Animator Manager](#animator-manager)
- [Ejemplos de Uso](#ejemplos-de-uso)
  - [Ejemplo básico](#ejemplo-básico)
  - [Ejemplo intermedio](#ejemplo-intermedio)
  - [Ejemplo avanzado](#ejemplo-avanzado)
- [Solución de Problemas](#solución-de-problemas)
- [Preguntas Frecuentes](#preguntas-frecuentes)
- [Glosario](#glosario)
- [Contacto y Soporte](#contacto-y-soporte)

## Introducción
Este documento proporciona `instrucciones detalladas sobre cómo utilizar la herramienta de comportamiento de enemigos para videojuegos 2D`.  
Este manual se divide en varias secciones que cubren todos los aspectos necesarios para la instalación y el uso de la herramienta. Inicialmente, se guiará al usuario a través del proceso de instalación desde un repositorio de GitHub. A continuación, se detallará la arquitectura de la herramienta, explicando los componentes clave y el concepto de las Máquinas de Estados Finitas. Posteriormente, se presentará un flujo de trabajo paso a paso para la creación de nuevos enemigos, incluyendo la configuración de estados, transiciones, sensores y actuadores. Finalmente, se ofrecerán consejos y mejores prácticas para el diseño de enemigos efectivos, así como información sobre cómo obtener soporte técnico.

## Objetivo 
Este manual tiene como objetivo proporcionar una guía clara y detallada para que los usuarios puedan instalar, configurar y utilizar la herramienta con mayor facilidad.  

La herramienta ha sido diseñada para simplificar y optimizar el proceso de creación de enemigos 2D funcionales dentro del entorno de Unity. Utilizando una arquitectura basada en Máquinas de Estado Finito (FSM), permite a los desarrolladores definir el comportamiento de los enemigos de manera visual e intuitiva, a través de la adición de estados y transiciones personalizadas.


## Funcionalidad
- Creación y gestión de comportamientos de enemigos en 2D.
- Implementación de máquinas de estados para definir la IA de los enemigos.

## Público objetivo
Tanto la herramienta como el manual han sido `creados para diseñadores o personas sin conocimientos avanzados en programación`.   
Si bien se `recomienda tener un conocimiento básico de Unity` y de los conceptos fundamentales del desarrollo de juegos, este manual se ha elaborado con la intención de ser lo suficientemente completo como para que usuarios con distintos niveles de experiencia puedan utilizar la herramienta de manera efectiva.

## Requisitos
Antes de comenzar, asegúrate de cumplir con los siguientes requisitos:
- Sistema operativo compatible: Windows, macOS o Linux.  
- Disponer de una versión igual o superior a `2022.3.18 (LTS)` de Unity.

## Instalación
Paso a paso para la intalación:  
1. Descarge de la Herramienta desde GitHub: 
&nbsp;&nbsp;La herramienta se distribuye como un paquete de Unity a través de una URL de GitHub.Para obtener la herramienta, accede al  [Link](https://github.com/CiscoGalvan/TFG/blob/main/Package/FrameworkEnemies2D.unitypackage).  
Una vez en el repositorio, presione las teclas: `control + shift + s` o dele a `more file actions` (botón de los 3 puntos) y seleccionar `descargar`.  
2. Abra Unity y carge su proyecto o cree un nuevo proyecto 2D.
3. En Unity, ve a `Assets > Import Package > Custom Package`.
4. Selecciona el archivo descargado (`.unitypackage`).
5. Presiona `Importar` y asegúrate de marcar todas las opciones necesarias.
6. Una vez importado, verifica que los activos de la herramienta aparecen en la ventana `Project` de Unity.


## Contenido del Paquete
### 📂 `Scripts`
- Contiene los scripts necesarios para el funcionamiento del framework.
- Incluye lógica de gestión de estados, comportamientos de enemigos y detección de colisiones.
- Organizados en subcarpetas según su funcionalidad (`FSM`, `Actuators`, `SensorsAndEmitters`, `Editors`, `PlayerBehaviour`, `Basic Components`, `Editors`, `Animation`).

### 🎮 Carpeta `Scenes`
- Contiene escenas de ejemplo con enemigos funcionales.
- Cada escena muestra configuraciones distintas.

### 🏗️ Carpeta `Prefabs`
- Incluye prefabs de enemigos preconfigurados listos para su uso.

### 🎞️ Carpeta `Animations`
- Contiene clips de animación de enemigos.
- Incluye animaciones como `Idle`, `Walk`, `Attack` y `Death`.
- Compatible con el sistema de `Animator` de Unity.


## Mapa de Navegación
Explicación sobre la estructura de la herramienta, menús y funciones principales.


Descripción General de las Máquinas de Estados Finitas para la IA de Enemigos: en glosario


## Solución de Problemas
| Problema                  | Solución                          |
|---------------------------|----------------------------------|
| La aplicación no inicia   | Verifica la instalación y dependencias. |
| Error al abrir un archivo | Asegúrate de que el formato es compatible. |
| Rendimiento lento         | Cierra otras aplicaciones y reinicia el programa. |

## Preguntas Frecuentes
Sección para responder dudas comunes sobre el uso del software. A RELLENAR CUANDO HAGAMOS PRUEBAS DE USUARIOS

## Glosario
Lista de términos técnicos y sus definiciones para facilitar la comprensión del manual.
- ***Máquinas de estado finitas (FSM):*** Una Máquina de Estados Finita  es un modelo computacional utilizado para diseñar algoritmos que describen el comportamiento de un sistema a través de un número limitado de estados posibles y las transiciones entre esos estados . En el contexto de la inteligencia artificial de los videojuegos, cada estado representa un comportamiento específico que un enemigo puede exhibir, como estar inactivo (Idle), patrullar un área (Patrol), atacar al jugador (Attack) o perseguirlo (Chase). Las transiciones entre estos estados se activan mediante condiciones o eventos específicos, a menudo generados por la interacción del enemigo con su entorno a través de sensores. En cualquier momento dado, el enemigo se encuentra en un único estado "actual", y su comportamiento está determinado por las reglas definidas para ese estado. Las FSMs son una técnica fundamental en la IA de videojuegos, ofreciendo una manera clara y controlada de gestionar comportamientos complejos . Su naturaleza determinista facilita la depuración y la comprensión del flujo de la lógica del enemigo.   

## Contacto y Soporte

Se recomienda revisar escenas de ejemplo ydocumentación adicional de los desarrolladores.
Para obtener soporte técnico adicional o para proporcionar comentarios sobre la herramienta, puede contactar directamente a los desarrolladores a través de los siguientes medios: [soporte@ejemplo.com](mailto:soporte@ejemplo.com).


---
© 2025 Cristina Mora Velasco y Francisco Miguel Galván Muñoz. Todos los derechos reservados.



![Mi imagen](./imagen.png)
![Mi GIF](a.gif)
