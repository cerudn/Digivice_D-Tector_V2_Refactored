# Arquitectura del Proyecto - Digivice D-Tector V2 Refactored

## Visión General

Este proyecto es una refactorización completa del Digivice D-Tector V2 original, preparado para una futura migración a V4. La arquitectura sigue principios SOLID y patrones de diseño modernos.

## Principios de Diseño

### 1. Inversión de Dependencias
Todos los managers dependen de interfaces, no de implementaciones concretas.

```csharp
// ❌ ANTES (V2 original)
public class GameManager : MonoBehaviour {
    [SerializeField] private LogicManager logicManager; // Dependencia concreta
}

// ✅ AHORA (Refactored)
public class GameManager : MonoBehaviour {
    [SerializeField] private ILogicManager logicManager; // Dependencia de interfaz
}
```

### 2. Patrón Observer
Comunicación entre sistemas mediante eventos globales (`GameEvents`).

### 3. Patrón State
Máquina de estados explícita para `LogicManager`.

### 4. Centralización de Constants
Todos los valores mágicos y IDs en archivos `Constants/`.

## Estructura de Carpetas

```
Assets/
├── Scripts/
│   ├── Interfaces/           # Contratos para todos los sistemas
│   ├── Constants/            # Valores centralizados
│   ├── Events/               # Sistema de eventos global
│   ├── Core/                 # Implementaciones de managers
│   ├── Logic/                # Lógica de negocio con patrón State
│   ├── Data/                 # ScriptableObjects
│   ├── SavedGame/            # Sistema versionado
│   └── SpriteSystem/         # Optimizado con cacheo
├── Data/                     # Assets de ScriptableObjects
├── Scenes/                   # Escenas del juego
└── Resources/                # Recursos cargables
```

## Componentes Principales

### GameManager
- **Rol**: Coordinador principal
- **Dependencias**: `ILogicManager`, `IScreenManager`, `ISaveSystem`, `IAudioManager`
- **Responsabilidades**:
  - Inicializar todos los sistemas
  - Coordinar ciclo de vida del juego
  - Manejar eventos globales

### LogicManager
- **Rol**: Máquina de estados de lógica
- **Estados**: `EXPLORATION`, `BATTLE`, `EVOLUTION`, `MENU`, `APP`
- **Patrón**: State Pattern con `IGameState` interface

### ScreenManager
- **Rol**: Director de UI
- **Sistema**: Rendering custom con `ScreenElement` builders
- **Responsabilidades**: Transiciones, animaciones, layout

### SaveSystem
- **Rol**: Persistencia de datos
- **Características**: Versionado, checksum, migración
- **Schema**: `SaveData` con `PlayerData`, `DigimonSaveData`, `WorldData`

## Flujo de Datos

```
[InputManager] → [LogicManager] → [GameManager] → [ScreenManager]
     ↓                ↓                 ↓              ↓
[GameEvents] ←────────────────────────────────────
     ↓
[AudioManager, SpriteDatabase, SavedGame, etc.]
```

## Próximos Pasos

1. Implementar clases concretas de interfaces
2. Migrar datos a ScriptableObjects
3. Implementar patrón State en LogicManager
4. Agregar tests unitarios
5. Documentar APIs con XML comments

## Referencias

- [Patrón State](https://refactoring.guru/design-patterns/state)
- [Patrón Observer](https://refactoring.guru/design-patterns/observer)
- [Principios SOLID](https://en.wikipedia.org/wiki/SOLID)
