# 📌 Procedural Terrain Generation - TCC Documentation

This project implements a procedural terrain generation system featuring biome customization, object clustering, dynamic chunk loading, and mob spawning.

## 📋 Table of Contents
- [📊 General Concepts](#-procedural-generation-concepts)
- [🛠️ Requirements](#️-requirements)
- [🚀 Getting Started](#-getting-started)
- [📜 Script Descriptions](#-script-descriptions)
- [🎛️ Core Components](#-core-components)
- [🌳 Biome System](#-biome-system)
- [🎮 Mob System](#-mob-system)
- [📦 Dungeon & Portal System](#-dungeon--portal-system)
-  [📜 License](#-license)

## 📊 General  Concepts

### 🌄 Noise-Based Terrain Generation
Procedural terrains in this project are generated using **Perlin Noise** or **Simplex Noise**, which are gradient noise functions commonly used in terrain generation.

- **Perlin Noise**: A smooth, continuous noise function ideal for natural-looking heightmaps.
- **Simplex Noise**: A more computationally efficient alternative to Perlin Noise that reduces artifacts in higher dimensions.

These noise functions are used in **heightmaps**, determining terrain elevation based on a coordinate system.

### 🗺️ Voronoi Diagram for Biome Distribution
Voronoi diagrams are used in procedural generation to divide the terrain into distinct biome regions.

- **How it Works**: The algorithm places a set of **seed points** randomly or based on predefined rules. The terrain is then divided into cells, where each point inside a cell is closer to its respective seed than to any other.
- **Application in Biomes**: Each Voronoi cell represents a different biome (e.g., desert, forest, mountains), ensuring a more natural and visually appealing transition between biomes.
- **Biome Blending**: The Voronoi structure can be combined with Perlin Noise to create **smoother biome transitions** rather than harsh edges.

### 🌿 Object Clustering (Foliage, Rocks, and Mobs)
Clusters refer to **grouped placement of objects** in the world, commonly used for vegetation, rocks, and mob spawning.

- **How Clusters Work**: Instead of placing objects randomly, clustering ensures that similar objects (e.g., trees, bushes, rocks) are grouped together in natural formations.
- **Cluster Generation**:  
  - Uses **Poisson Disc Sampling** or **Cellular Automata** to create natural-looking object distribution.
  - Can be adjusted based on **density parameters**, ensuring variation in each biome.
- **Application in Mobs**: Some mobs may spawn in packs or groups instead of being randomly scattered.

### 🔦 Raycasts & Collision Detection ("Cast" System)
Casts are used to detect objects, players, and obstacles in the environment using **raycasting**.

- **Raycast**: A straight-line physics check that detects if something is hit.
  - Used to detect **players**, **obstacles**, and **terrain height**.
- **SphereCast**: Similar to Raycast but with a spherical area, useful for **mob detection ranges**.
- **OverlapSphere**: A method that detects multiple objects within a given radius, often used for **area-based enemy detection**.
- **Application in AI**:  
  - Mobs use `Raycast` to check if they have a clear line of sight to the player.
  - `OverlapSphere` is used for **group detection**, allowing packs of mobs to react together.

### 🎭 State Machines for AI Behavior
Mobs use a **Finite State Machine (FSM)** to control their behavior.

- **Example States**:
  - **Idle** → Waiting for the player.
  - **Wandering** → Moving randomly within a defined area.
  - **Chasing** → Following the player if detected.
  - **Attacking** → Engaging in combat.


Using FSMs makes AI behavior more predictable and easier to modify.


## 🛠️ Requirements
**Unity Version:** 2021.3+ (LTS recommended)  

**Script Dependencies:**
- `EndlessTerrain` - Manages chunk loading
- `TerrainGenerator` - Generates procedural terrain

## 🚀 Getting Started

### To Generate Terrain:
1. Attach the `EndlessTerrain` and `TerrainGenerator` scripts to a GameObject.

### To Create Biomes:
1. Right-click in the project window.
2. Navigate to `Create > Scriptable Objects > Biome`.
3. Configure the biome properties as described in the [Biome System](#-biome-system) section.
4. Assign the created biome to the **biomeDefinitions** field in `TerrainGenerator`.

### To Create Mobs:
Each mob GameObject must have the following **scripts**:
- `MobActionsController`
- `MobStatusController`
- `MobMovementStateMachine`
- `RotateToGroundNormal` (Aligns to terrain slope)
- `HealthManager`
- `SpeedManager`

Additionally, the following **components** must be attached:
- `NavMeshAgent`
- `Rigidbody`
- `Collider`

Other required settings:
- The mob must be converted into a prefab (drag from `Hierarchy` to `Project` window).
- Assign a layer (preferably `mob`).
- The player must have `PlayerStatusController` with:
  - `Collider`
  - `layer: player`
  - `tag: player`
  - **Method**: `ReceiveDamage(int)` (to detect and attack the player).

### To Create Dungeons:
1. Create a new `GameObject` to represent the dungeon.
2. Attach the following scripts:
   - `DungeonGenerator` (responsible for dungeon creation)
   - `DungeonMobSpawner` (spawns mobs inside the dungeon)
3. Convert it into a prefab (drag from `Hierarchy` to `Project` window).

### To Create Portals:
1. Create a new `GameObject` for the portal.
2. Attach the `Portal` script.
3. Ensure the **player has the following:**
   - `PlayerMovementController` script
   - A `Collider` (necessary for portal interaction)
4. Convert the portal into a prefab.

### To Create Dungeon Rooms:
1. Create a 3D object resembling a room with doors and walls.
2. Attach the `RoomBehaviour` script.
3. Convert it into a prefab.

## 📜 Script Descriptions

### `EndlessTerrain`
- Controls chunk visibility based on the player's position.

### `TerrainGenerator`
- Manages the procedural generation of terrains with biomes.

### `MobActionsController`
- Handles mob behavior and state transitions.

### `MobStatusController`
- Manages the mob's health, status effects, and other properties.

### `MobMovementStateMachine`
- Implements a finite state machine for mob movement.

### `HealthManager`
- Manages the mob’s health and damage system.

### `SpeedManager`
- Controls movement speed and speed-related properties.

### `RotateToGroundNormal`
- Adjusts the mob’s rotation to align with the terrain slope.

### `DungeonGenerator`
- Procedurally generates dungeon layouts.

### `DungeonMobSpawner`
- Handles spawning mobs inside dungeons.

### `RoomBehaviour`
- Configures and manages dungeon rooms.

---
## 🎛️ Core Components
### 🌍 EndlessTerrain
Manages infinite terrain generation based on player position.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `maxViewDst` | `float` | Maximum distance the player can see the terrain. Determines chunk loading. | `250` | ✔️ |
| `viewer` | `Transform` | The player's transform, which is used to determine the loaded chunks. | `null` | ✔️ |
| `shouldHaveMaxChunkPerSide` | `bool` | Enables a finite world by limiting the number of chunks per side. | `false` | ✔️ |
| `maxChunksPerSide` | `int` | The maximum number of chunks per side (if finite world mode is enabled). | `2` | ✔️ |
| `portalSettings` | `PortalSettings` | Defines portal configurations| `null` | ❌ |
| `mobSettings` | `MobSettings` | Manages mob spawning parameters. | `null` | ❌ |

---

### 🏔️ TerrainGenerator
Handles the procedural terrain generation using noise functions.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `chunkSize` | `int` | Size of each terrain chunk. | `241` | ✔️ |
| `octaves` | `int` | Number of layers of Perlin noise applied. More octaves add more detail. | `5` | ✔️ |
| `lacunarity` | `float` | Controls frequency scaling for noise layers. | `2.0` | ✔️ |
| `terrainTextureBasedOnVoronoiPoints` | `bool` | Enables Voronoi-based texturing for biome variation. | `true` | ✔️ |
| `NumVoronoiPoints` | `int` | Number of points used in Voronoi-based terrain segmentation. | `8` | ✔️ |
| `VoronoiSeed` | `int` | Seed for Voronoi pattern generation (0 = random). | `0` | ✔️ |
| `VoronoiScale` | `float` | Controls the scale of the Voronoi points (distance). | `350` | ✔️ |
| `useWeightedBiome` | `bool` | Enables a weighted biome system for more realistic transitions. | `true` | ✔️ |
| `levelOfDetail` | `int` | Level of terrain mesh simplification (0-6, higher = lower detail). | `0` | ✔️ |
| `biomeDefinitions` | `BiomeInstance[]` | List of biome definitions used in terrain generation. | `null` | ✔️ |
| `shouldSpawnObjects` | `bool` | Toggles object spawning on the terrain. | `true` | ✔️ |
| `clusterBaseFrequency` | `float` | Defines the base frequency for object clustering. | - | ❌ |
| `clusterAmplitude` | `float` | Determines how dense clusters of objects can be. | - | ❌ |
| `clusteringIntensity` | `float` | Controls the spread of clusters in the terrain. | - | ❌ |

---

## 🌳 Biome System

### BiomeInstance
Container class for biome configurations at runtime.

| Parameter | Type | Description |
|-----------|------|-------------|
| `BiomePrefab` | `Biome` | Reference to the biome prefab |
| `runtimeObjects` | `List<BiomeObject>` | Active objects in the biome |

### Biome Class
Defines biome properties such as terrain features and spawnable objects.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `name` | `string` | Unique identifier for the biome. | `null` | ✔️ |
| `minHeight / maxHeight` | `float` | Defines the elevation range where this biome can appear. | - | ❌ |
| `texture` | `Texture2D` | The texture assigned to this biome. | `null` | ❌ |
| `amplitude` | `float` | Controls the vertical variation of the biome. | - | ✔️ |
| `frequency` | `float` | Controls the frequency of terrain variations within this biome. | - | ✔️ |
| `objects` | `List<BiomeObject>` | List of objects that spawn in this biome. | `null` | ❌ |

### BiomeObject
Defines spawnable objects inside a biome.

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| `terrainObject` | `GameObject` | Prefab of the object to be spawned. | `null` |
| `probabilityToSpawn` | `float` | Probability of this object spawning per terrain cell. | `1.0` |
| `slopeThreshold` | `float` | Maximum terrain slope allowed for placement. | `80` |
| `isClusterable` | `bool` | Determines whether this object can spawn in clusters. | `true` |
| `clusterCount` | `int` | Number of clusters per chunk. | `5` |
| `clusterRadius` | `float` | Defines the area a cluster will cover. | `50` |

---
## 🎮 Mob System

### Cast
Defines detection and interaction mechanics for mobs.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `castType` | `CastType` | Type of detection (Sphere, Box, Capsule, Ray). | - | ✔️ |
| `castSize` | `float` | Defines the size of the detection area. | - | ✔️ |
| `boxSize` | `Vector3` | Dimensions of the box cast. | - | ❌ |
| `customOrigin` | `Vector3` | Specifies a custom origin point for the cast. | - | ❌ |
| `customAngle` | `Vector3` | Specifies a custom angle for the cast. | - | ❌ |
| `targetLayers` | `LayerMask` | Defines which layers can be detected. | - | ✔️ |


### HealthManager Class
Defines health-related properties and behaviors.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| hp | float | Current health of the entity | Not specified | ✔️ |
| maxHp | float | Maximum health of the entity | 100 | ✔️ |
| incrementFactor | float | Factor for increasing max health | 15 | ❌ |
| hpRegen | float | Passive health regeneration rate | 1 | ❌ |
| hpTickRegen | float | Health regeneration per tick | 0.5 | ❌ |
| hpHealFactor | float | Healing factor applied to health | Not specified | ❌ |
| hpDamageFactor | float | Damage factor applied to health | Not specified | ❌ |
| healthRegenCooldownDuration | float | Cooldown duration before health regen starts | 15 | ❌ |
| healthRegenCooldownTimer | float | Timer for health regeneration cooldown | -15 | ❌ |

### SpeedManager Class
Defines movement speed-related properties.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| speed | float | Base speed of the entity | Not specified | ✔️ |
| speedWhileRunningMultiplier | float | Speed multiplier when running | Not specified | ❌ |
| incrementFactor | float | Factor for increasing speed | 15 | ❌ |

### MobStatusController Class
Manages the status components of a mob.

| Parameter | Type | Description | Required |
|-----------|------|-------------|----------|
| healthManager | HealthManager | Associated health manager | ✔️ |
| speedManager | SpeedManager | Associated speed manager | ✔️ |

### MobActionsController Class
Controls movement and actions of a mob.

| Parameter | Type | Description | Required |
|-----------|------|-------------|----------|
| wanderDistance | float | Maximum distance the animal can wander | ✔️ |
| maxWalkTime | float | Maximum time the animal can walk | ✔️ |
| patrolPoints | Vector3[] | Patrol points for movement | ❌ |
| currentPatrolPoint | int | Current patrol point index | ❌ |
| idleTime | float | Time the animal remains idle | ✔️ |
| type | string | Mob type ("Player" should not be chosen) | ✔️ |
| detectionRange | float | Detection range of the mob | ✔️ |
| detectionCast | Cast | Responsible for detection | ✔️ |
| escapeMaxDistance | float | Maximum escape distance for prey | ✔️ |
| maxChaseTime | float | Maximum chase time for a predator | ✔️ |
| biteDamage | int | Damage inflicted when biting | ✔️ |
| isPartialWait | bool | If the predator stops moving after biting | ✔️ |
| biteCooldown | float | Cooldown time between bites | ✔️ |
| attackDistance | float | Maximum attack distance | ✔️ |
| playerHasMaxChaseTime | bool | If predator has max chase time for player | ✔️ |
| Preys | List<string> | List of prey types (must include "Player") | ✔️ |
| stoppingMargin | float | Margin for predator to stop chasing | ✔️ |
| detectionDistance | Vector3 | Detection distance represented as a vector | ✔️ |
| offSetDetectionDistance | Vector3 | Offset for detection distance | ✔️ |
| mobTransform | Transform | Mob’s transform (position, rotation, scale) | ✔️ |


### RotateToGroundNormal Class
Aligns an object with the ground surface.

| Parameter | Type | Description | Required |
|-----------|------|-------------|----------|
| rotationSpeed | float | Rotation speed of the object | ✔️ |
| modelTransform | Transform | Transform of the model being rotated | ✔️ |

### MobSettings
Controls the general behavior of mob spawning.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `prefabs` | `List<SpawnableMob>` | List of available mobs for spawning. | `null` | ✔️ |
| `maxNumberOfMobs` | `int` | Maximum number of mobs allowed in the world. | `50` | ✔️ |
| `shouldWaitToStartSpawning` | `bool` | If enabled, mobs will not spawn immediately. | `false` | ❌ |
| `waitingTime` | `float` | Time delay before spawning begins. | `5.0` | ❌ |
| `shouldHaveRandomWaitingTime` | `bool` | If true, waiting time is randomized. | `false` | ❌ |
| `minWaitingTime` | `float` | Minimum randomized waiting time. | `0` | ❌ |
| `maxWaitingTime` | `float` | Maximum randomized waiting time. | `10` | ❌ |

### SpawnableMob
Defines mob-specific spawning rules.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `mobPrefab` | `GameObject` | Mob prefab reference | `null` | ✔️ |
| `allowedBiomes` | `List<Biome>` | Biomes where the mob can spawn | `null` | ✔️ |
| `maxInstances` | `int` | Maximum number of this mob type | `5` | ✔️ |
| `spawnWeight` | `float` | Higher values increase spawn probability | `1.0` | ✔️ |
| `spawnTime` | `float` | Fixed interval for mob spawn | `10` | ✔️ |
| `shouldHaveRandomSpawnTime` | `bool` | Allows random spawn intervals | `false`| ✔️ |

---

## 📦 Dungeon & Portal System


### RoomBehaviour Class
Defines room properties and grid-based structure.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| walls | GameObject[] | List of GameObjects representing room walls (0 - North, 1 - South, 2 - East, 3 - West) | null | ✔️ |
| doors | GameObject[] | List of GameObjects representing room doors | null | ✔️ |
| upperWalls | GameObject[] | List of GameObjects representing upper walls | null | ❌ |
| upperDoors | GameObject[] | List of GameObjects representing upper doors | null | ❌ |
| roomGrid | GridCell[,] | Grid representing the room structure | null | ✔️ |
| gridRows | int | Number of rows in the grid |  | ✔️ |
| gridColumns | int | Number of columns in the grid |  | ✔️ |
| gridOrigin | Vector3 | Origin point of the grid |  | ✔️ |
| calcBasedOffSet | Vector3 | Offset used for room dimension calculations |  | ❌ |
| cellWidth | float | Width of each grid cell |  | ✔️ |
| cellDepth | float | Depth of each grid cell |  | ✔️ |

### GridCell Class
Represents an individual grid cell.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| isOccupied | bool | Indicates if the cell is occupied | false | ✔️ |
| occupant | GameObject | Object occupying the cell (mob or item) | null | ❌ |
| position | Vector3 | World position of the cell |  | ✔️ |

### Rule Class
Defines rules for room spawning.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| room | GameObject | Room associated with the rule | null | ✔️ |
| minPosition | Vector3Int | Minimum spawn position |  | ✔️ |
| maxPosition | Vector3Int | Maximum spawn position |  | ✔️ |
| obligatory | bool | Indicates if the room is mandatory | false | ❌ |
| maxRoomsPerFloor | int | Maximum rooms per floor |  | ❌ |
| totalMaxRooms | int | Total maximum rooms |  | ✔️ |
| minFloorTospawn | int | Minimum floors required for spawn |  | ❌ |
| maxFloorTospawn | int | Maximum floors allowed for spawn |  | ❌ |
| hasMaxRoomsToSpawn | bool | Indicates if a max room count exists | false | ❌ |


### DungeonMobSpawner
Handles mob spawning mechanics in dungeons.

| Parameter | Type | Description | Required |
|-----------|------|-------------|----------|
| `currentMobs` | `int` | Current number of mobs in the dungeon. | ✔️ |
| `minMobsToStartSpawned` | `float` | Minimum number of mobs to start spawned. | ✔️ |
| `maxMobs` | `int` | Maximum number of allowed mobs. | ✔️ |
| `hasMaxMobs` | `bool` | Indicates if a maximum number of mobs is set. | ✔️ |
| `weightSpawnFactor` | `float` | Weight factor used in spawn calculation. | ✔️ |
| `hasRandomSpawnTime` | `bool` | Indicates if spawn time is random. | ✔️ |
| `spawnTime` | `float` | Fixed spawn time for mobs. | ❌ |
| `minTimeToStartSpawning` | `float` | Minimum time before spawning starts. | ✔️ |
| `minTimeToSpawn` | `float` | Minimum time between consecutive spawns. | ✔️ |
| `maxTimeToSpawn` | `float` | Maximum time between consecutive spawns. | ✔️ |
| `spawnTimeReducedPerDifficulty` | `int` | Exponential time reduction per difficulty level. | ✔️ |
| `willMaxMobsIncrease` | `bool` | Indicates if max mob count increases with difficulty. | ✔️ |
| `maxMobIncrease` | `int` | Number of additional mobs per difficulty level. | ✔️ |
| `weightSpawnFactorIncreasePerDifficulty` | `int` | Increase in spawn factor per difficulty level. | ✔️ |
| `spawnDificulty` | `ESpawnDifficulty` | Current difficulty level. | ✔️ |
| `mobList` | `List<SpawnableMob>` | List of spawnable mobs. | ✔️ |


### DungeonGenerator Class
Controls dungeon generation parameters.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| seed | int | Seed for dungeon generation |  | ✔️ |
| randomSeed | int | If 0, a random seed is used |  | ❌ |
| size | Vector2Int | Size of the dungeon (number of cells) |  | ✔️ |
| startPos | int | Starting position for generation |  | ✔️ |
| offset | Vector3 | Offset applied to the dungeon position |  | ❌ |
| stairRoom | GameObject | Prefab for stair rooms connecting floors | null | ❌ |
| maxNumberOfStairsPerFloor | int | Maximum stair rooms per floor |  | ❌ |
| minNumberOfStairsPerFloor | int | Minimum stair rooms per floor |  | ❌ |
| randomMaxStairsPerFloor | bool | Determines if max stairs per floor is random | false | ❌ |
| canPlaceStairInEmptyCells | bool | Determines if stairs can be placed in empty cells | false | ❌ |
| stairSpawnChance | float | Chance to spawn a stair per cell (0-100%) |  | ❌ |
| minFloors | int | Minimum number of floors in the dungeon |  | ✔️ |
| maxFloors | int | Maximum number of floors in the dungeon |  | ✔️ |
| mobSpawner | DungeonMobSpawner | Controller for mob spawning | null | ❌ |
| rooms | List<Rule> | List of possible rooms to generate | null | ❌ |
| maxFloors | int | Maximum number of floors in the dungeon |  | ✔️ |
| mobSpawner | DungeonMobSpawner | Controller for mob spawning | null |  |
| rooms | List<Rule> | List of possible rooms to generate | null |  |

### Portal
Handles portal-based dungeon entry.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `shouldInstantiateDungeon` | `bool` | Determines if the dungeon is instantiated in the same scene or another. | `true` | ✔️ |
| `sceneToLoad` | `string` | Name of the scene to be loaded. | - | ✔️ |
| `dungeonObject` | `GameObject` | Object representing the dungeon associated with the portal. | - | ✔️ |
| `despawnTime` | `float` | Fixed time for dungeon activation. | - | ❌ |
| `minDespawnTime` | `float` | Minimum random time for activation. | - | ❌ |
| `maxDespawnTime` | `float` | Maximum random time for activation. | - | ❌ |
| `shouldHaveRandomDespawnTime` | `bool` | Determines if the despawn time should be random. | - | ✔️ |

---

### SpawnablePortal
Handles the spawning of portal instances.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `prefab` | `GameObject` | Prefab of the portal to be generated. | - | ✔️ |
| `maxInstances` | `int` | Maximum number of allowed portal instances. | - | ✔️ |
| `spawnTime` | `float` | Fixed time to generate the portal. | - | ❌ |
| `minSpawnTime` | `float` | Minimum variation for random spawn time. | - | ❌ |
| `maxSpawnTime` | `float` | Maximum variation for random spawn time. | - | ❌ |
| `shouldHaveRandomSpawnTime` | `bool` | Determines if the spawn time should be random. | - | ✔️ |
| `currentInstances` | `int` | Current number of generated portal instances. | - | ✔️ |


### PortalSettings
Handles portal-based dungeon entry.

| Parameter | Type | Description | Default | Required |
|-----------|------|-------------|---------|----------|
| `maxNumberOfPortals` | `int` | Maximum number of portals allowed | `3` | ✔️ |
| `shouldWaitToStartSpawning` | `bool` | Delays portal activation | `false` | ✔️ |
| `waitingTime` | `float` | Fixed delay before portals activate | `10.0` | ✔️ |
| `minWaitingTime` | `float` | Fixed min delay before portals activate | 
| `maxWaitingTime` | `float` | Fixed max delay before portals activate |
| `shouldHaveRandomWaitingTime` | `bool` | Should have a random spawn time |


## 📜 License
MIT License - See LICENSE for details.
