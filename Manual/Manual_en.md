# ***Manual de Uso para Framework de comportamientos de enemigos para videojuegos 2D***

[🇬🇧 Read in English](../readme.md)  
***Bienvenido al manual de uso.***  
**Creadores:** Cristina Mora Velasco y Francisco Miguel Galván Muñoz  
**Fecha:** Marzo de 2025

## Índice

- [Introducción](#introducción)
- [Objetivo de la herramienta](#objetivo-de-la-herramienta)
- [Objetivo del manual](#objetivo-del-manual)
- [Funcionalidad](#funcionalidad)
- [Público Objetivo](#público-objetivo)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Contenido del Paquete](#contenido-del-paquete)
- [Componentes Detallados](#componentes-del-framework)
  - [Máquina de Estados Finita (FSM)](#máquina-de-estados-finita-fsm)
  - [Estado (State)](#estado)
  - [Sensores](#sensores)
  - [Actuadores](#actuadores)
  - [Animator Manager](#animator-manager)
- [Ejemplos Prácticos](#ejemplos-de-uso)
- [Solución de Problemas](#solución-de-problemas)
- [Preguntas Frecuentes](#preguntas-frecuentes)
- [Glosario](#glosario)
- [Contacto y Soporte](#contacto-y-soporte)

## Introducción

Este documento proporciona *instrucciones detalladas sobre cómo utilizar la herramienta de comportamiento de enemigos para videojuegos 2D*.  
Este manual se divide en varias secciones que cubren todos los aspectos necesarios para la instalación y el uso de la herramienta. Inicialmente, se guiará al usuario a través del proceso de instalación desde un repositorio de GitHub. A continuación, se detallará la arquitectura de la herramienta, explicando los componentes clave y el concepto de las Máquinas de Estados Finitas. Posteriormente, se presentará un flujo de trabajo paso a paso para la creación de nuevos enemigos, incluyendo la configuración de estados, transiciones, sensores y actuadores. Finalmente, se ofrecerán consejos y mejores prácticas para el diseño de enemigos efectivos, así como información sobre cómo obtener soporte técnico.

## Objetivo de la herramienta

Con el paso del tiempo, los juegos han evolucionado haciéndose cada vez más complejos. Los enemigos que, son el principal obstáculo del jugador, tienen que seguir siendo lo suficientemente desafiantes para captar la atención del jugador pero no sentirse abrumado. Esto incrementa el tiempo y complejidad de creación. Para facilitar esta tarea, **Enemy framework 2D** tiene como objetivo la creación de enemigos completamente funcionales partiendo de elementos sencillos llamados actuadores y controlados por una máquina de estados. Además, para poder tener información del exterior se necesitarán sensores, que funcionarán como transición entre los diferentes estados.

## Objetivo del manual

Este manual tiene como objetivo proporcionar una guía clara y detallada para que los usuarios puedan instalar, configurar y utilizar la herramienta con mayor facilidad.  

*La herramienta ha sido diseñada para simplificar y mejorar el proceso de creación de enemigos 2D funcionales dentro del entorno de Unity*.  

Utilizando una arquitectura basada en Máquinas de Estado Finitas (FSM), permite a los diseñadores definir el comportamiento de los enemigos de manera visual e intuitiva, a través de la adición de estados y transiciones personalizadas.

## Funcionalidad

- Creación y gestión de comportamientos de enemigos en 2D.
- Implementación de máquinas de estados para definir la IA de los enemigos.

## Público objetivo

Tanto la herramienta como el manual han sido creados para *diseñadores o personas sin conocimientos avanzados en programación*.  
Si bien *se recomienda tener un conocimiento básico de Unity* y de los conceptos fundamentales del desarrollo de juegos, este manual se ha elaborado con la intención de ser lo suficientemente completo como para que usuarios con distintos niveles de experiencia puedan utilizar la herramienta de manera efectiva.

## Requisitos

Antes de comenzar, asegúrate de cumplir con los siguientes requisitos:

- Disponer de una versión igual o superior a *2022.3.18 (LTS)* de Unity.

## Instalación

1. Descarga de la Herramienta desde GitHub:
   - La herramienta se distribuye como un paquete de Unity a través de una URL de GitHub. Para obtener la herramienta, accede al [link](https://github.com/CiscoGalvan/TFG/blob/main/Package/FrameworkEnemies2D.unitypackage).
   - Una vez en el repositorio, presiona las teclas *Control + Shift + S* o haz clic en *More File Actions* (botón de los tres puntos) y selecciona *Descargar*.
2. Abre Unity y carga tu proyecto o crea un nuevo proyecto 2D.
3. En Unity, ve a *Assets > Import Package > Custom Package*.
4. Selecciona el archivo descargado (*.unitypackage*).
5. Presiona *Importar* y asegúrate de marcar todas las opciones necesarias.
6. Una vez importado, verifica que los archivos de la herramienta aparecen en la ventana *Project* de Unity.

## Contenido del Paquete

### Scripts

- Contiene los scripts necesarios para el funcionamiento del framework.
- Incluye lógica de gestión de estados, comportamientos de enemigos y detección de colisiones.
- Organizados en subcarpetas según su funcionalidad (*FSM*, *Actuators*, *SensorsAndEmitters*, *Editors*, *PlayerBehaviour*, *Basic Components*, *Editors*, *Animation*).

### Scenes

- Contiene escenas de ejemplo con enemigos funcionales.
- Cada escena muestra configuraciones distintas.

### Prefabs

- Incluye prefabs de enemigos preconfigurados listos para su uso.

### Animations

- Contiene clips de animación de enemigos.
- Incluye animaciones como *Idle*, *Walk*, *Attack* y *Death*.
- Controller de ejemplo para controlar las animaciones.
- Compatible con el sistema de *Animator* de Unity.

## Componentes del Framework

### Actuadores

Los actuadores son componentes que permiten a los enemigos realizar acciones. Estas acciones son las que definen el movimiento o creación de otros enemigos.
Disponemos de 7 tipos de actuadores:

- **Spawner Actuator**:

  ![SpawnerActuator](./SpawnerActuator.png)  
  Permite generar (spawnear) nuevos enemigos.

- *Infinite Enemies:* si se quiere crear infinitos enemigos, en caso contrario se debe especificar la cantidad de veces que vamos a spawnear la lista.
- *Spawn Interval:* cada cuánto tiempo se crean.
  - *Prefab to Spawn:* objeto que queremos crear.
  - *Spawn Point:* posición donde queremos que se cree el objeto.

  Al ser una lista, podemos spawnear más de un objeto a la vez.

- **Horizontal Actuator**:  

  ![HorizontalActuator](./HorizontalActuator.png)  
Este actuador permite mover un objeto horizontalmente, ya sea a la izquierda o a la derecha, con diferentes configuraciones de velocidad y comportamiento tras una colisión. Tiene distintas configuraciones.

  - *Reaction After Collision*  
  Define qué sucede cuando el objeto choca contra otro:
    - *None:* No hay ninguna reacción al colisionar.
    - *Bounce:* El objeto cambia de dirección y sigue moviéndose en sentido contrario.
    - *Destroy:* El objeto desaparece al colisionar.
  - *Direction*  
  Determina hacia dónde se mueve el objeto:
    - *Left:* El objeto se moverá hacia la izquierda.
    - *Right:* El objeto se moverá hacia la derecha.
  - *Is Accelerated*  
    - *False:* Si no es acelerado, el enemigo se moverá con una velocidad lineal constante. Se podrá configurar:  
      - *Throw:* Se aplicará una única vez la fuerza, simulando un lanzamiento.
      - *Speed:* Establece la velocidad a la que se moverá el objeto.
    - *True:* Si el movimiento si es acelerado, la velocidad irá aumentando:
      - *Goal Speed:* Es la velocidad máxima que alcanzará el objeto después de acelerar.
      - *Interpolation Time:* Es el tiempo que tarda el objeto en pasar de velocidad 0 a su velocidad objetivo.
      - *Easing Function:* Define cómo se comporta la aceleración.

- **Vertical Actuator**:  
 ![VerticalActuator](./VerticalActuator.png)  
  Este actuador permite mover un objeto vertical, ya sea hacia arriba o hacia abajo, con diferentes configuraciones de velocidad y comportamiento tras una colisión.

  - *Reaction After Collision*  
  Define qué sucede cuando el objeto choca contra otro:
    - *None:* No hay ninguna reacción al colisionar
    - *Bounce:* El objeto cambia de dirección y sigue moviéndose en sentido contrario.
    - *Destroy:* El objeto desaparece al colisionar.
  - *Direction*  
  Determina hacia dónde se mueve el objeto:
    - *Up:* El objeto se moverá hacia arriba.
    - *Down:* El objeto se moverá hacia abajo.
  - *Is Accelerated*  
    - *False:* Si no es acelerado, el enemigo se moverá con una velocidad lineal constante. Se podrá configurar:  
      - *Throw:* Se aplicará una única vez la fuerza, simulando un lanzamiento.
      - *Speed:* Establece la velocidad a la que se moverá el objeto.
    - *True:* Si el movimiento es acelerado, la velocidad irá aumentando:
      - *Goal Speed:* Es la velocidad máxima que alcanzará el objeto después de acelerar.
      - *Interpolation Time:* Es el tiempo que tarda el objeto en pasar de velocidad 0 a su velocidad objetivo.
      - *Easing Function:* Define cómo se comporta la aceleración.

- **Directional Actuator**:  
![DirectionalActuator](./DirectionalActuator.png)  
  Hace que el enemigo se mueva en una dirección específica descrita por un ángulo.
  - *Reaction After Collision*  
  Define qué sucede cuando el objeto choca contra otro:
    - *None:* No hay ninguna reacción al colisionar
    - *Bounce:* El objeto cambia de dirección y simula un rebote.
    - *Destroy:* El objeto desaparece al colisionar.
  - *Angle:*  Ángulo con el que va a moverse el objeto
  - *Aim Player:* Indica si el objeto va a seguir la dirección del jugador (con esta opción el ángulo no aparece porque se le da valor en función de tu posición y la del objetivo)
  - *Is Accelerated*  
    - *False:* Si no es acelerado, el enemigo se moverá con una velocidad lineal constante. Se podrá configurar:  
      - *Throw:* Se aplicará una única vez la fuerza, simulando un lanzamiento
      - *Speed:* Establece la velocidad a la que se moverá el objeto

    - *True:* Si el movimiento si es acelerado, la velocidad irá aumentando:
      - *Goal Speed:* Es la velocidad máxima que alcanzará el objeto después de acelerar.
      - *Interpolation Time:* Es el tiempo que tarda el objeto en pasar de velocidad 0 a su velocidad objetivo.
      - *Easing Function:* Define cómo se comporta la aceleración

- **Circular Actuator**:  
![CircularrActuator](./CircularActuator.png)  
 Permite movimientos circulares en torno a un punto de rotación determinado.
  - *Rotation Point Position*  
    Define el punto central sobre el cual se realiza la rotación.  
    - *None:* Si no se asigna, el objeto girará en torno a su propio centro.  
    - *Transform:* Si se asigna un objeto, la rotación se realizará alrededor de ese punto.  

  - *Max Angle*  
    Ángulo máximo que puede alcanzar el movimiento circular (360 indica un círculo completo, el resto de ángulos se comporta como un péndulo).  

  - *Can Rotate*  
    Determina si el objeto puede rotar sobre su propio eje además de moverse en círculo.  
    - *False:* El objeto solo se moverá en la trayectoria circular sin girar sobre sí mismo.  
    - *True:* El objeto girará sobre su propio eje mientras se mueve.  

  - *Is Accelerated*
    - *False:* Si no es acelerado, el objeto se moverá con velocidad constante definida por el parámetro *Speed*.  
    - *True:* Si es acelerado, la velocidad aumentará progresivamente según los siguientes parámetros:  
      - *Goal Speed:* Es la velocidad máxima que alcanzará el objeto.  
      - *Interpolation Time:* Es el tiempo que tarda el objeto en pasar de velocidad 0 a su velocidad objetivo.  
      - *Easing Function:* Define cómo se comporta la aceleración.

- **Move to a Point Actuator**:  
Hace que el enemigo se mueva hacia un punto fijo específico del escenario. Hay dos configuraciones dependiendo del *Usage Way*
  - *Random Area*  
![MoveToAPointActuator](./MoveToAPointActuatorA.png)
Random area coge puntos aleatorios dentro de un área.
    - *Random Area:* Collider que servirá para la referencia del área
    - *Time Between Random Points:* Cada cuánto cambia el punto a otro distinto
  - *Waypoint*: Indica que queremos seguir un camino predeterminado de puntos
    - *Is A Cicle:* Indica si queremos que al llegar al final de los waypoints, se vuelva a iniciar la lista.
    - *Same Waypoint Behaviour:* Indica si queremos que el comportamiento sea el mismo para todos los waypoints.
      - Si es así, se creará un panel único de especificación de puntos:  
![MoveToAPointActuator](./MoveToAPointActuatorS.png)  
        - *Time Between Waypoints:* Tiempo que se tarda entre un punto y otro.
        - *Are Accelerated:* Si el movimiento es acelerado o no. En caso de serlo, aparecerá una easing function que indicará con qué aceleración se mueve.
        - *Should Stop:* Indica si debe o no parar al llegar a un punto. Si se debe parar, hay que  indicar cuánto tiempo.  
      - Si no es así, aparecerán los mismos datos por cada waypoint:  
    ![MoveToAPointActuator](./MoveToAPointActuator.png)  

- **Move to an Object Actuator**:  
![MoveToAnObjectActuator](./MoveToAnObjectActuator.png)  
  Hace que el enemigo se desplace automáticamente hacia un objeto determinado, si el objeto se mueve, el enemigo cambiará su dirección para ir hacia el objeto.
  - *Waypoint Transform:* Transform del objeto al que se quiere perseguir.
  - *Time to Reach:* Tiempo que tarda en llegar al objetivo.
  - *Is Accelerated:*
    - *False:* Si no es acelerado, la posición cambiará de manera constante.  
    - *True:* Si es acelerado, la posición se definirá mediante la función de easing:
    ![MoveToAnObjectActuator](./MoveToAnObjectActuatorA.png)  
- **Spline Follower Actuator**:  
![SpllineFollowerActuator](./Spline.png)  
  Hace que el enemigo se desplace  y rote automáticamente siguiendo una ruta definida por un spline.
  - *Spline Container:* Transform del objeto al que se quiere perseguir.
  - *Speed:* Tiempo que tarda en llegar al objetivo
  - *Is Accelerated*
    - *False:* Si no es acelerado, el objeto se moverá con velocidad constante definida por el parámetro *Speed*.  
    - *True:* Si es acelerado, la velocidad aumentará progresivamente según los siguientes parámetros:  
      - *Goal Speed:* Es la velocidad máxima que alcanzará el objeto.  
      - *Interpolation Time:* Es el tiempo que tarda el objeto en pasar de velocidad 0 a su velocidad objetivo.  
      - *Easing Function:* Define cómo se comporta la aceleración.

### Sensores

Los sensores permiten detectar información del entorno y activar transiciones.
 Disponemos de cinco sensores:

- **Area Sensor:**  
  ![AreaSensor](./AreaSensor.png)  
  El sensor de área detecta cuando un objeto específico entra dentro de su zona de detección.<br>
  - *Start Detecting Time:* tiempo de delay hasta que empiece la detección.
  - *Target:* objeto que se quiere detectar.
  - *Detection Condition:* Indica si quiere dectectar al salir o al entrar del área.

- **Collision Sensor:**  
  ![CollisionSensor](./CollisionSensor.png)  
  Detecta cuando el enemigo choca físicamente con otro objeto. A diferencia del *Area Sensor*, este requiere una colisión real en lugar de solo detectar la presencia dentro de un área.<br>
  Se debe especificar qué *capas físicas* activan el sensor.
  - *Start Detecting Time:* tiempo de delay hasta que empiece la detección.
  - *Layers to Collide:* Mascara de capas físicas donde se debe indicar con que queremos chocar.

- **Distance Sensor:**  
![DistanceSensor](./DistanceSensor.png)  
  Detecta cuando un objeto específico (Target) está a una *determinada distancia del enemigo*.
  - *Distance type:* tipo de distancia que se quiere comprobar.
    - Magnitud: 360 grados de detección.
    - Single Axix: un único eje.
  - *Detection Condition:* Indica si quiere dectectar al salir o al entrar del área.
  - *Target:* objeto que se quiere detectar.
  - *Start Detecting Time:* tiempo de delay hasta que empiece la detección.
  - *Detection Distance:* distancia de detección.
  
- **Time Sensor:**
![TimeSensor](./TimeSensor.png)  
 Detecta cuando pasa un tiempo específico.
  - *Start Detecting Time:* tiempo de delay hasta que empiece la detección.
  - *Detection Time:* tiempo de detección.
- **Damage Sensor:**  
![DamageSensor](./DamageSensor.png)  
  Detecta cuando una entidad *recibe daño*.
  Este sensor es utilizado a la hora de gestionar la *vida* tanto de los enemigos como del propio jugador.<br> Para que se pueda recibir daño se debe tener *Active From Start* a true.

- **Damage Emitter**:  
  Es el encargado de *hacer daño*, en él tienes que especificar el tipo de daño, cada tipo de daño tiene sus propios parámetros:

  - **Instant:**  
  ![DamageEmitter](./DamageEmitter.png)  
  Daño instantáneo que afecta una única vez al entrar en contacto.  
    - *Destroy After Doing Damage:* permite indicar si queremos que el objeto desaparezca tras hacer daño.  
    - *Instant Kill:* permite indicar si queremos que mate directamente a la entidad con la que colisiona.  
    - *Damage Amount:* en caso de no querer eliminar ni matar, se indica el daño que queremos aplicar.

  - **Permanence:**  
  ![DamageEmitterP](./DamageEmitterP.png)  
  El daño por permanencia afecta mientras estés dentro del objeto.  
    - *Damage Amount:* cantidad de vida que se resta cada vez.  
    - *Damage Cooldown:* intervalo de tiempo entre cada aplicación de daño.

  - **Residual:**  
  ![DamageEmitterR](./DamageEmitterR.png)  
  El daño residual sigue afectando incluso cuando ya no estás en contacto.  
    - *Destroy After Doing Damage:* permite indicar si el objeto debe eliminarse después del primer golpe.  
    - *Instant Damage Amount:* daño inicial que se aplica al primer contacto.  
    - *Residual Damage Amount:* daño aplicado en cada repetición residual.  
    - *Damage Cooldown:* intervalo de tiempo entre cada aplicación de daño residual.  
    - *Number Of Applications:* número total de veces que se aplica el daño residual.

### Estado

Un estado es un comportamiento concreto que puede tener un enemigo en un cierto tiempo. Los estados se encargan de almacenar las acciones.  
  ![State](./State.png)  
  Hace que el enemigo se desplace  y rote automáticamente siguiendo una ruta definida por un spline.

- *Actuator List:* Acción/acciones vamos a realizar
- *Transiton List:*  Para poder tener *Transiciones* de un estado a otro, se debe especificar el sensor que estará encargado de detectar ese cambio y el estado al que se desea pasar.
- *Damaged Emitters:* En caso de que queramos que en el estado se realice daño, se deberá especificar qué *DamageEmitter* se encontrará activo.  
- *Debug State:* Si deseamos *depurar* información sobre el movimiento que se va a realizar.

### Máquina de Estados Finita (FSM)

  ![FSM](./FSM.png)  
  La FSM organiza el comportamiento de un enemigo en **estados** (Idle, Patrol, Attack, etc.). Esta es la encargada de llamar y gestionar todos los estados de un enemigo.  

- *Initial State:* estado inicial del enemigo.

**Ejemplo:** Un "Guardia" puede tener estados como Patrol, Chase y Attack. Si el jugador entra en su campo de visión, transiciona de Patrol a Chase. Si lo alcanza, a Attack. Si lo pierde de vista, vuelve a Patrol.

### Animator Manager

Se encarga de gestionar las animaciones de los enemigos en función de sus estados y acciones. Si se quiere añadir una animación, es necesario añadir tambien un animator de Unity.  
Es importante que todos los Sprites que se quieran utilizar *se orienten hacia la derecha*.

### Life

Gestiona la vida de los objetos.  
 ![Life](./Life.png)  

- *Initial Life:* vida inicial.
- *Entity type:* tipo de entidad (player o enemy)

## Ejemplos de Uso

TODOS los ejemplos parten de la Scene Template: **Base Scene**.  
Para crear una nueva escena desplegar el menú de File, New Scene, seleccionar Base Scene y selecciona Create.
La escena cuenta con un jugador y un mundo listos para funcionar.

**AVISO**: En los ejemplos, cuando se dice borrar todos los estados del animator, se refiere a los que no son propios de Unity, es decir, los que aparecen en color Gris. Los estados propios de Unity seguirán apareciendo aunque se intenten borrar.  
**Aviso sobre el Arte:** El material gráfico utilizado principalmente en este framework ha sido obtenido del Asset Store de Unity y pertenece al creador Pixel Frog, cuya página de itch.io es: [https://pixelfrog-assets.itch.io/](https://pixelfrog-assets.itch.io/)  
El águila y efectos de items son de:
<https://assetstore.unity.com/packages/2d/characters/sunny-land-103349>

### Primer Ejemplo: PINCHOS

Uno de los enemigos más comunes son los pinchos, que no se mueven pero sí que dañan al jugador. Vamos a crearlos.
Para el ejemplo usaremos la imagen de los pinchos:  

![Pinchos](./Pinchos.png)

 1. Crea un objeto llamado pinchos partiendo del prefab BaseEnemy que se encuentra en Assets/Prefabs.
 2. Cambia el spriteRender a la imagen de pinchos (si no coincidiese ya) que se encuenta en assets/Animations/Sprites/Spikes y ajusta el collider a la imagen.
 3. Congela la posición en x y en y, para que los pinchos se mantengan fijos.
 4. Elimina el AnimatorManager y Animator, en este caso no son necesarios porque el objeto no tiene animación.

### Segundo Ejemplo: DEAMBULADOR

Otro enemigo muy común son deambuladores, también conocidos como: goomba, reptacillo, o con otro nombre en muchos juegos.
Para el ejemplo usaremos la imagen del conejo:  
![Bunny](./Bunny.png)

 1. Crea un objeto llamado deambulador partiendo del prefab BaseEnemy que se encuentra en Assets/Prefabs.
 2. Cambia el spriteRender a la imagen de Bunny que se encuenta en Assets/Animations/Sprites/Bunny y ajusta el collider a la imagen.
 3. Congela la rotación en constraints del RigidBody 2D.
 4. Configuramos el Damage Emiter:  
     - Queremos que haga daño desde el inicio
     - Que sea de tipo Instant
     - Que haga 1 de daño
 5. Añadimos un componente de movimiento Horizonal Actuator y lo añadimos a la lista de actuadores del State
 6. Configuramos el Horizonal Actuator:
    - Queremos que no sea acelerado
    - Que al colisionar rebote con las capas físicas Mundo y Jugador
    - Que no siga al jugador
    - Que la dirección sea hacia la derecha  
    - Que no sea un lanzamiento
    - Que tenga velocidad continua de 7  

Ahora vamos a añadir animaciones:
 
  7. Configuramos el Animator Manager  
      - Queremos que no haga flip en el eje y pero que sí lo haga en el x

  8. Duplicamos el controller animation que viene creado como ejemplo en Assets/Animations
  9. Entramos en el Editor de Animator de Unity (haciendo doble click sobre el controller que acabamos de crear), donde veremos muchos estados posibles, como solo queremos que haga la animación de Idle. movimiento horizontal, Damage y Die, borraremos el resto de estados (selecionamos con el ratón y pulsar suprimir).
  10. Hacemos Click sobre el estado Idle y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Idle que se encuentra en Assets/Animations/Sprites/Bunny
  11. Hacemos Click sobre el estado Horizontalovement y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Run que se encuentra en Assets/Animations/Sprites/Bunny
  12. Hacemos Click sobre el estado Damage y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Hit que se encuentra en Assets/Animations/Sprites/Bunny
  13. Hacemos Click sobre el estado Die y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Hit que se encuentra en Assets/Animations/Sprites/Bunny
  14. Añadimos el controlador que hemos duplicado al Animator del enemigo deambulador.

### Tercer Ejemplo: Torreta + balas

Vamos a continuar creando un enemigo que dispare balas, para ello vamos a crear primero las balas y luego el enemigo.
Para el ejemplo usaré la imagen de la bala:  
![Bullet](./Bullet.png)

 1. Crea un objeto partiendo del sprite de la bala que se encuentra en Assets/Animations/Sprites
 2. Añadir una capa física para el enemigo (si no está creada ya), por ejemplo Enemigo
 3. Añadir un componente de tipo box collider 2D y un rigidbody 2D
 4. Añadir un componente de tipo Damage Emitter.
 5. Indicar cómo queremos que haga daño el enemigo:  
     - Queremos que haga daño desde el inicio
     - Que sea de tipo Instant
     - Que se destruya despuhes de hacer daño
     - Que haga 1 de daño
 6. Vamos a añadir movimiento, eso se controla desde una máquina de estados, por lo tanto añadimos un componente de tipo FSM
 7. Añadimos un componente State y se lo asignamos a la FSM en el initial State.
 8. Añadimos el componente Directional Actuator y lo añadimos a la lista de actuadores del Estado
 9. Configuramos el Movimiento horizontal:
    - Queremos que colisione con las capas físicas Mundo y Jugador
    - Que al colisionar se destruya
    - Que no sea acelerado
    - Que siga al jugador
    - Que no sea un lanzamiento
    - Que tenga velocidad continua de 10
10. Añadimos el  componente DamageEmiter ya creado a la lista de DamageEmiters del Estado actual
Ahora vamos a Crear la Torreta:
Para el ejemplo usaré la imagen de la planta:  
![Planta](./Planta.png)
 1. Crea un objeto partiendo del sprite de la planta que se encuentra en Assets/Animations/Sprites
 2. Añadir una capa física para el enemigo (si no está creada ya), por ejemplo Enemigo
 3. Añadir un componente de tipo box collider 2D y un rigidbody 2D (congelar rotación y posición en constraints)
 4. Añadir un componente de tipo Damage Emitter.
 5. Indicar cómo queremos que haga daño el enemigo:  
     - Queremos que haga daño desde el inicio
     - Que sea de tipo Instant
     - Que haga 1 de daño
 6. Vamos a añadir la creación de otros enemigos (spawner), eso se controla desde una máquina de estados, por lo tanto añadimos un componente de tipo FSM
 7. Añadimos un componente State y se lo asignamos a la FSM en el initial State.
 8. Añadimos el componente de Spawner Actuator y lo añadimos a la lista de actuadores del estado
 9. Configuramos el Spawner Actuator:
    - Queremos que cree infinitos enemigos
    - Que sea cada 2 segundos
    - Que cree un único enemigo a la vez
10. Añadimos el  prefab de la bala a la lista del spawner: spawn list, en Prefab to Spawn.
11. Creamos un nuevo objeto vacío donde queramos que se cree el nuevo enemigo y se lo asignamos a la lista del spawner: spawn list, en  Spawn Point.
Ahora vamos a añadir animaciones:

12. Añadir un componente de tipo AnimationManager, veremos que al hacerlo se nos crea también un componente Animator de Unity.  
13. Configuramos el Animator Manager  
      - Queremos que no haga flip en el eje y ni en el eje x

14. Duplicamos el controller animation que viene creado como ejemplo en Assets/Animations
15. Entramos en el Editor de Animator de Unity (haciendo doble click sobre el controller que acabamos de crear), donde veremos muchos estados posibles, como solo queremos que haga la animación de Idle y spawn, borraremos el resto de estados (selecionamos con el ratón y pulsar suprimir).
16. Hacemos Click sobre el estado Idle y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Idleplant que se encuentra en Assets/Animations/Anim
17. Hacemos Click sobre el estado Spawn y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar SpawnPlant que se encuentra en Assets/Animations/Anim
  
18. Añadimos el controlador que hemos duplicado al Animator que se nos creó al añadir el AnimatorManager.

### Cuarto Ejemplo: TikTik (splines)

Vamos a crecrear un enemigo del HollowKnigth el TikTIk, este va recorriendo una plataforma bordeándola.  
Para el ejemplo usaré la imagen de la zarigüeya:  
![Oso](./Zariguella.png)
Antes de empezar con la creación del enemigo, añadiremos un objeto en 2d cuadrado que nos servirá como plataforma. Debemos añadirle un componente de tipo box collider 2D y un rigidbody 2D (congelar rotación y posición en constraints), así como, añadirlo ala capa Mundo.
Empecemos con el enemigo:

 1. Crea un objeto partiendo del sprite de la zarigüeya que se encuentra en Assets/Animations/Sprites
 2. Añadir una capa física para el enemigo (si no está creada ya), por ejemplo Enemigo
 3. Añadir un componente de tipo box collider 2D y un rigidbody 2D
 4. Añadir un componente de tipo Damage Emitter.
 5. Indicar cómo queremos que haga daño el enemigo:  
     - Queremos que haga daño desde el inicio
     - Que sea de tipo Instant
     - Que haga 1 de daño
 6. Vamos a añadir movimiento, eso se controla desde una máquina de estados, por lo tanto añadimos un componente de tipo FSM
 7. Añadimos un componente State y se lo asignamos a la FSM en el initial State.
 8. Añadimos el componente de Spline Follower Actuator y lo añadimos a la lista de actuadores del estado
 9. Creamos un Spline con forma cuadrada y lo giramos 90 grados en el eje de las X
 9. Configuramos el Spline Follower Actuator:
    - Añadimos el spline recien creado como referencia
    - Asignamos la velocidad a la que queremos que vaya
    - Queremos que se teletransporte el enemigo a la curba y no al contrario.
10. Añadimos el  componente DamageEmiter ya creado a la lista de DamageEmiters del Estado actual
Ahora vamos a añadir animaciones:

10. Añadir un componente de tipo AnimationManager, veremos que al hacerlo se nos crea también un componente Animator de Unity.  
11. Configuramos el Animator Manager  
      - Queremos que no haga flip en el eje Y ni en el eje X

12. Duplicamos el controller animation que viene creado como ejemplo en Assets/Animations
13. Entramos en el Editor de Animator de Unity (haciendo doble click sobre el controller que acabamos de crear), donde veremos muchos estados posibles, como solo queremos que haga la animación de Idle borramos el resto (selecionamos con el ratón y pulsar suprimir).
14. Hacemos Click sobre el estado Idle y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar Opossumwolk que se encuentra en Assets/Animations/Anim
  
15. Añadimos el controlador que hemos duplicado al Animator que se nos creó al añadir el AnimatorManager.

### Quinto Ejemplo: Estalactitas

Por último vamos a crecrear un enemigo común. Las estalactitas.
Para el ejemplo usaré la imagen del pájaro:  
![FatBird](./FatBird.png)

 1. Crea un objeto partiendo del sprite de FatBird que se encuentra en Assets/Animations/Sprites
 2. Añadir una capa física para el enemigo (si no está creada ya), por ejemplo Enemigo
 3. Añadir un componente de tipo box collider 2D y un rigidbody 2D (congelar la rotación en constrainsts)
 4. Añadir un componente de tipo Damage Emitter.
 5. Indicar cómo queremos que haga daño el enemigo:  
     - Queremos que haga daño desde el inicio
     - Que sea de tipo Instant
     - Que mate directamente al jugador
 6. Vamos a añadir acciones, eso se controla desde una máquina de estados, por lo tanto añadimos un componente de tipo FSM
 7. Añadimos un componente State y se lo asignamos a la FSM en el initial State.
 8. Añadimos un elemento a la lista de Sensor Transitions. Lo rellenemos creando y asignando un nuevo estado alestado de transición
 9. Para el sensor que activará la transición, vamos a crear un objeto 2d que contenga:

- Box Collider 2D
- Area sensor, que tenga tiempo de inicio 0 y como target el jugador.

 10. Para el segundo estado añadiremos un actuador a la Lista de Actuadores de tipo Vertical Actuator.
 11. Configuramos el Vertical Actuator:  
    - Queremos que se elimine al colisionar  
    - Que colisione con las capas físicas Mundo y Jugador  
    - Que no siga al jugador  
    - Que no sea ni acelerado ni sea un lanzamiento  
    - Que tenga una  velocidad de 13  
 12. Añadimos el  componente DamageEmiter ya creado a la lista de DamageEmiters del Estado dos

Ahora vamos a añadir animaciones:

  13. Añadir un componente de tipo AnimationManager, veremos que al hacerlo se nos crea también un componente Animator de Unity.  
  14. Configuramos el Animator Manager  
      - Queremos que no haga flip en el eje Y ni en el eje X

  15. Duplicamos el controller animation que viene creado como ejemplo en Assets/Animations
  16. Entramos en el Editor de Animator de Unity (haciendo doble click sobre el controller que acabamos de crear), donde veremos muchos estados posibles, como solo queremos que haga la animación de Idle, vertical movement y die borramos el resto(selecionamos con el ratón y pulsar suprimir).
  17. Hacemos Click sobre el estado Idle y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar IdleFatBird que se encuentra en Assets/Animations/Sprites
  18. Hacemos Click sobre el estado Die y arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar GraundFatBird que se encuentra en Assets/Animations/Sprites
  19. Hacemos DOBLE Click sobre el estado Vertical Movement, borramos el estado UP y en Down arrastramos la animación que queremos hacer hasta Motion, en este caso vamos a usar FallFatBird que se encuentra en Assets/Animations/Sprites
  
  20. Añadimos el controlador que hemos duplicado al Animator que se nos creó al añadir el AnimatorManager.

## Solución de Problemas

| Problema                  | Solución                          |
|---------------------------|----------------------------------|
| El paquete inicia con errores en consola   | Verifica la instalación y dependencias del proyecto. |
| | |
| | |

## Preguntas Frecuentes

Sección para responder dudas comunes sobre el uso del software.

## Glosario

Lista de términos técnicos y sus definiciones para facilitar la comprensión del manual:

- ***Arquitectura:*** En este caso, la arquitectura de una herramienta se refiere a como está estructurada, que elementos usa o como está organizada.
- ***Flujo de Trabajo:*** Es el orden o pasos que hay que completar en una tarea
- ***Máquinas de estado finitas (FSM):*** Una Máquina de Estados Finita es un modelo computacional utilizado para diseñar algoritmos que describen el comportamiento de un sistema a través de un número limitado de estados posibles y las transiciones entre esos estados. En el contexto de la inteligencia artificial de los videojuegos, cada estado representa un comportamiento específico. Las transiciones entre estos estados se activan mediante condiciones específicas, a menudo generadas por la interacción del enemigo con su entorno.

- ***Estado:*** En una máquina de estados, un estado representa una situación en la que un enemigo puede encontrarse en un momento dado. Define las acciones del enemigo mientras se mantiene en dicho estado. Por ejemplo, un enemigo puede estar en estado *Idle*, *Patrol*, *Attack*, etc.

- ***Serializado:*** Permite modificar valores sin necesidad de cambiar el código, editándolos desde el editor de Unity.
- ***Transform:*** Es un componente de Unity que almacena y gestiona la posición, rotación y escala de un objeto en la escena. Es fundamental para manipular cualquier objeto dentro del mundo del juego, ya que permite moverlo, rotarlo y escalarlo.
- ***Flip:*** voltear la imagen.

## Contacto y Soporte

Se recomienda revisar escenas de ejemplo y documentación adicional de los desarrolladores.
Para obtener soporte técnico adicional o para proporcionar comentarios sobre la herramienta, puede contactar directamente a los desarrolladores a través de los siguientes medios:[crmora03@ucm.es](mailto:crmora03@ucm.es).

---
© 2025 Cristina Mora Velasco y Francisco Miguel Galván Muñoz. Todos los derechos reservados.
