# ***User Manual for 2D Video Game Enemy Behavior Framework***
[🇪🇸 Leer en Español](../readme.md)<br>
***Welcome to the user manual.***<br>
**Creators:** Cristina Mora Velasco and Francisco Miguel Galván Muñoz<br>
**Date:** March 2025

## Table of Contents
- [Introduction](#introduction)
- [Objective](#objective)
- [Functionality](#functionality)
- [Target Audience](#target-audience)
- [Requirements](#requirements)
- [Installation](#installation)
- [Package Contents](#package-contents)
- [Detailed Components](#framework-components)
  - [Finite State Machine (FSM)](#finite-state-machine-fsm)
  - [State](#state)
  - [Sensors](#sensors)
  - [Actuators](#actuators)
  - [Animator Manager](#animator-manager)
- [Practical Examples](#practical-examples)
  - [Basic Example](#basic-example)
  - [Intermediate Example](#intermediate-example)
  - [Advanced Example](#advanced-example)
- [Troubleshooting](#troubleshooting)
- [Frequently Asked Questions](#frequently-asked-questions)
- [Glossary](#glossary)
- [Contact and Support](#contact-and-support)

## Introduction
This document provides `detailed instructions on how to use the enemy behavior tool for 2D video games`.
This manual is divided into several sections covering all necessary aspects for the installation and use of the tool. Initially, the user will be guided through the installation process from a GitHub repository. Next, the architecture of the tool will be detailed, explaining the key components and the concept of Finite State Machines. Subsequently, a step-by-step workflow for creating new enemies will be presented, including the configuration of states, transitions, sensors, and actuators. Finally, tips and best practices for effective enemy design will be offered, as well as information on how to obtain technical support.

## Objective
This manual aims to provide a clear and detailed guide so that users can install, configure, and use the tool more easily.
`The tool has been designed to simplify and optimize the process of creating functional 2D enemies within the Unity environment`. Using an architecture based on Finite State Machines (FSM), allows designers to define enemy behavior visually and intuitively, through the addition of custom states and transitions.

## Functionality
- Creation and management of enemy behaviors in 2D.
- Implementation of state machines to define enemy AI.

## Target Audience
Both the tool and the manual have been `created for designers or individuals without advanced programming knowledge`.
While `basic knowledge of Unity` and fundamental game development concepts is recommended, this manual has been prepared with the intention of being comprehensive enough for users with varying levels of experience to use the tool effectively.

## Requirements
Before starting, make sure you meet the following requirements:
- A version of Unity equal to or greater than `2022.3.18 (LTS)`.

## Installation
Step-by-step installation:
1. **Download the Tool from GitHub:**
   - The tool is distributed as a Unity package via a GitHub URL. To obtain the tool, go to the [Link](https://github.com/CiscoGalvan/TFG/blob/main/Package/FrameworkEnemies2D.unitypackage).
   - Once in the repository, press `Control + Shift + S` keys or click on `More File Actions` (three dots button) and select `Download`.
2. **Open Unity and load your project or create a new 2D project.**
3. **In Unity, go to `Assets > Import Package > Custom Package`.**
4. **Select the downloaded file (`.unitypackage`).**
5. **Press `Import` and make sure to check all the necessary options.**
6. **Once imported, verify that the tool's assets appear in the Unity `Project` window.**

## Package Contents
### 📂 `Scripts`
- Contains the necessary scripts for the framework to function.
- Includes logic for state management, enemy behaviors, and collision detection.
- Organized into subfolders according to their functionality (`FSM`, `Actuators`, `SensorsAndEmitters`, `Editors`, `PlayerBehaviour`, `Basic Components`, `Animation`).

### 🎮 `Scenes`
- Contains example scenes with functional enemies.
- Each scene shows different configurations.

### 🏗️ `Prefabs`
- Includes pre-configured enemy prefabs ready for use.

### 🎞️ `Animations`
- Contains enemy animation clips.
- Includes animations such as `Idle`, `Walk`, `Attack`, and `Death`.
- Compatible with Unity's `Animator` system.

## Framework Components
### Finite State Machine (FSM)<br>
  ![FSM](./FSM.png)<br>
  The FSM is responsible for calling and managing all the states of an enemy.
  It is necessary to specify the `initial state` of the enemy.

### State<br>
  ![State](./State.png)<br>
  Within each state, we must specify which action/actions we will perform in the `Actuator List`.
  To have `Transitions` from one state to another, the sensor responsible for detecting that change and the state to which we want to transition must be specified.
  If we want damage to be dealt in the state, we must specify which `DamageEmitter` will be active.
  Finally, if we want to `see information about the movement to be performed via Gizmos`, we must activate `Debug State`.

### Sensors
Sensors allow detecting information from the environment and triggering transitions. We have five sensors available:

- **Area Sensor:**<br>
  ![AreaSensor](./AreaSensor.png)<br>
  The area sensor detects when a specific object (Target) enters its detection zone.<br>
  This sensor causes the Collider associated with the object to become a `Trigger`.

- **Collision Sensor:**
  ![CollisionSensor](./CollisionSensor.png)
  Detects when the enemy physically collides with another object. Unlike the `Area Sensor`, this requires a real collision rather than just detecting presence within an area.<br>
  You must specify which `layers` activate the sensor.

- **Distance Sensor:**
![DistanceSensor](./DistanceSensor.png)
  Detects when a specific object (Target) is at a `certain distance from the enemy`.<br>
  A `detection condition` is required, which can be:
  - Being within the detection distance.
  - Being outside of it.<br>

  It is necessary to specify the `time it is inactive at the start` (Start Detecting Time); if this is 0, the sensor starts activated.<br>

- **Time Sensor:**<br>
![TimeSensor](./TimeSensor.png)<br>
 Detects when a specific `time` passes.

- **Damage Sensor:**<br>
![DamageSensor](./DamageSensor.png)<br>
 Detects when an entity `receives damage`.
 This sensor is used to manage the `life` of both enemies and the player.<br> For damage to be received, `Active From Start` must be set to true.

- **Damage Emitter:**
  It is responsible for `dealing damage`; you have to specify the type of damage, and each damage type has its own parameters:
  - Instant:
    ![DamagEmitter](./DamageEmitter.png)
    Instant damage is that which affects you only once upon contact. As parameters, we can specify if we want to `destroy the object after dealing damage`, if we want it to `directly kill the entity it collides with`. If we do not want it to directly eliminate the target, we will indicate the `damage we want to inflict`.
  - Persistent:
    ![DamagEmitter](./DamageEmitterP.png)
    Persistent damage is that which affects you while you are inside the object. As parameters, we can specify the `amount of damage` we inflict and `how often` we inflict it.
  - Residual:
    ![DamagEmitter](./DamageEmitterR.png)
    Finally, we have residual damage. This is what affects you even when you are no longer in contact. As parameters, we can specify if we want to `destroy the object after the first contact`, the `amount of damage on the first hit` (which is usually larger), the `amount of damage per application`, `how often`, and `how many` applications of residual damage are applied.
---

### Actuators
Actuators allow performing actions during enemy states. We have 7 types of actuators available:

- **Spawner Actuator**:
![SpawnerActuator](./SpawnerActuator.png)
  Allows generating (spawning) new enemies.
  - `Infinite Enemies:` if you want to create infinite enemies; otherwise, you must specify the number of times we will spawn the list.
  - `Spawn Interval:` how often they are created.
  - `Prefab to Spawn:` object we want to create.
  - `Spawn Point:` position where we want the object to be created.

  Being a list, we can spawn more than one object at a time.


- **Horizontal Actuator**:
![HorizontalActuator](./HorizontalActuator.png)

This actuator allows moving an object horizontally, either to the left or right, with different speed configurations and behavior after a collision. It has different configurations.

  - `Reaction After Collision`
  Defines what happens when the object collides with another:
    - `None:` No reaction upon collision.
    - `Bounce:` The object changes direction and continues moving in the opposite direction.
    - `Destroy:` The object disappears upon collision.
  - `Direction`
  Determines where the object moves:
    - `Left:` The object will move to the left.
    - `Right:` The object will move to the right.
  - `Is Accelerated`
    - `False:` If not accelerated, the enemy will move with a constant linear speed. You can configure:
      - `Throw:` The force will be applied only once, simulating a launch.
      - `Speed:` Sets the speed at which the object will move.
    - `True:` If the movement is accelerated, the speed will increase:
      - `Goal Speed:` This is the maximum speed the object will reach after accelerating.
      - `Interpolation Time:` This is the time it takes for the object to go from speed 0 to its target speed.
      - `Easing Function:` Defines how the acceleration behaves.

- **Vertical Actuator**:

  ![VerticalActuator](./VerticalActuator.png)

  This actuator allows moving an object vertically, either up or down, with different speed configurations and behavior after a collision. It has different configurations.

  - `Reaction After Collision`
  Defines what happens when the object collides with another:
    - `None:` No reaction upon collision.
    - `Bounce:` The object changes direction and continues moving in the opposite direction.
    - `Destroy:` The object disappears upon collision.
  - `Direction`
  Determines where the object moves:
    - `Up:` The object will move upwards.
    - `Down:` The object will move downwards.
  - `Is Accelerated`
    - `False:` If not accelerated, the enemy will move with a constant linear speed. You can configure:
      - `Throw:` The force will be applied only once, simulating a launch.
      - `Speed:` Sets the speed at which the object will move.
    - `True:` If the movement is accelerated, the speed will increase:
      - `Goal Speed:` This is the maximum speed the object will reach after accelerating.
      - `Interpolation Time:` This is the time it takes for the object to go from speed 0 to its target speed.
      - `Easing Function:` Defines how the acceleration behaves.


- **Directional Actuator**:
![DirectionalActuator](./DirectionalActuator.png)<br>
  Makes the enemy move in a specific direction described by an angle.
    - `Reaction After Collision`
  Defines what happens when the object collides with another:
      - `None:` No reaction upon collision.
      - `Bounce:` The object changes direction and simulates a bounce.
      - `Destroy:` The object disappears upon collision.
    - `Angle:` Angle at which the object will move.
    - `Aim Player:` Indicates whether the object will follow the player's direction (with this option, the angle does not appear because its value is determined by your position and the target's position).
    - `Is Accelerated`
      - `False:` If not accelerated, the enemy will move with a constant linear speed. You can configure:
        - `Throw:` The force will be applied only once, simulating a launch.
        - `Speed:` Sets the speed at which the object will move.

      - `True:` If the movement is accelerated, the speed will increase:
        - `Goal Speed:` This is the maximum speed the object will reach after accelerating.
        - `Interpolation Time:` This is the time it takes for the object to go from speed 0 to its target speed.
        - `Easing Function:` Defines how the acceleration behaves.

- **Circular Actuator**:<br>
![CircularrActuator](./CircularActuator.png)<br>
 Allows circular movements around a specific rotation point.
  - `Rotation Point Position`
    Defines the central point around which the rotation occurs.
    - `None:` If not assigned, the object will rotate around its own center.
    - `Transform:` If an object is assigned, the rotation will occur around that point.

  - `Max Angle`
    Maximum angle that the circular movement can reach (360 indicates a full circle; other angles behave like a pendulum).

  - `Can Rotate`
    Determines if the object can rotate on its own axis in addition to moving in a circle.
    - `False:` The object will only move in the circular path without rotating itself.
    - `True:` The object will rotate on its own axis while moving.

  - `Is Accelerated`
    - `False:` If not accelerated, the object will move at a constant speed defined by the `Speed` parameter.
    - `True:` If accelerated, the speed will increase progressively according to the following parameters:
      - `Goal Speed:` This is the maximum speed the object will reach.
      - `Interpolation Time:` This is the time it takes for the object to go from speed 0 to its target speed.
      - `Easing Function:` Defines how the acceleration behaves.

- **Move to a Point Actuator**:
Makes the enemy move towards a specific fixed point in the scene. There are two configurations depending on `Use Way`:
  - `Random Area`
![MoveToAPointActuator](./MoveToAPointActuatorA.png)
Random area picks random points within an area.
    - `Random Area:` Collider that will serve as the area reference.
    - `Time Between Random Points:` How often the point changes to a different one.
  - `Waypoint`: Indicates that we want to follow a predetermined path of points.
    - `Is A Circle:` Indicates whether we want the list to restart when the end of the waypoints is reached.
    - `Same Waypoints Behaviour:` Indicates whether we want the behavior to be the same for all waypoints.
      - If so, a single point specification panel will be created:
![MoveToAPointActuator](./MoveToAPointActuatorS.png)
        - `Time Between Waypoints:` Time taken between one point and another.
        - `Are Accelerated:` Whether the movement is accelerated or not. If so, an easing function will appear indicating the acceleration.
        - `Should Stop:` Indicates whether to stop upon reaching a point. If it should stop, the duration must be indicated.
      - If not, the same data will appear for each waypoint.
    ![MoveToAPointActuator](./Manual/MoveToAPointActuator.png)



- **Move to an Object Actuator**:
![MoveToAnObjectActuator](./MoveToAnObjectActuator.png)
  Makes the enemy automatically move towards a specific object; if the object moves, the enemy will change its direction to go towards the object.
  - `Waypoint Transform:` Transform of the object to be pursued.
  - `Time to Reach:` Time it takes to reach the target.
  - `Is Accelerated:`
    - `False:` If not accelerated, the position will change constantly.
    - `True:` If accelerated, the position will be defined by the easing function.
    ![MoveToAnObjectActuator](./MoveToAnObjectActuatorA.png)



### Animator Manager
It is responsible for managing enemy animations based on their states and actions. If you want to add an animation, you also need to add a Unity animator.
It is important that all Sprites to be used `face to the right`.
### Life
Manages the life of objects.
 ![Life](./Life.png)
 - `Initial Life:` Initial health.
 - `Entity type:` Type of entity (player or enemy).
## Practical Examples
All examples are based on the following setup:
 1. Creation of a new scene.
 2. Drag the *Scene* prefab into the *Assets/Prefabs* folder.
 3. Add a new layer with the desired name for the scenario, for example, *World*.
 4. Drag the *Player* prefab into the *Assets/Prefabs* folder.
 3. Add a new layer with the desired name for the Player, for example, *Player*.

### First Example: SPIKES
### Second Example: CRAWLER
### Third Example: FLYVENGER
### Fourth Example: TikTik (splines)
### Fifth Example: Turret + bullets

## Troubleshooting
| Problem                        | Solution                                          |
|--------------------------------|---------------------------------------------------|
| The package starts with errors in the console | Verify the project's installation and dependencies. |
|                                |                                                   |
|                                |                                                   |

## Frequently Asked Questions
Section to answer common questions about using the software. TO BE FILLED IN AFTER USER TESTING

## Glossary
List of technical terms and their definitions to facilitate understanding of the manual:
- ***Finite State Machines (FSM):*** A Finite State Machine is a computational model used to design algorithms that describe the behavior of a system through a limited number of possible states and the transitions between those states. In the context of video game artificial intelligence, each state represents a specific behavior. Transitions between these states are triggered by specific conditions, often generated by the enemy's interaction with its environment.
- ***State:*** In a state machine, a state represents a situation in which an enemy can be at a given moment. It defines the enemy's actions while in that state. For example, an enemy can be in the `Idle`, `Patrol`, `Attack`, etc., state.

- ***Serialized:*** Allows modifying values without needing to change the code, by editing them from the Unity editor.

- ***Transform:*** It is a Unity component that stores and manages the position, rotation, and scale of an object in the scene. It is fundamental for manipulating any object within the game world, as it allows moving, rotating, and scaling it.

- ***Serialized:*** In simple terms, it means that an object's information can be saved and retrieved later without losing its data. In Unity, this is used to remember configurations or save game progress.

## Contact and Support

It is recommended to review example scenes and additional documentation from the developers.
For additional technical support or to provide feedback on the tool, you can contact the developers directly through the following means: [soporte@ejemplo.com](mailto:soporte@ejemplo.com).

---
© 2025 Cristina Mora Velasco and Francisco Miguel Galván Muñoz. All rights reserved.