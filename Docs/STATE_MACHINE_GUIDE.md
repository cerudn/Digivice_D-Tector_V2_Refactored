# Guía de la Má—quina de Estados

## Visó·´·n General

El juego usa el patr ó—n **State** para manejar los diferentes estados del gameplay. Esto permite:
- **Có·´·digo organizado** - Cada estado en su propia clase
- **F á—cil extensó·´·n** - A ñ—adir nuevos estados sin modificar existentes
- **Menos bugs** - Menos condicionales anidados

## Arquitectura

```
LogicManager
    └── IGameState (interfaz)
        ├── ExplorationState
        ├── BattleState
        ├── EvolutionState
        ├── MenuState
        └── AppState
```

## Estados Implementados

### 1. ExplorationState

**Prop ó—sito:** Maneja la exploracó·´·n del mundo, movimiento y encuentros.

**Caracter í—sticas:**
- Movimiento del jugador
- Interaccó·´·n con objetos
- Encuentros aleatorios
- Transicó·´·n a batalla

**Input:**
- `WASD/Flechas` - Mover
- `Space/Z` - Accó·´·n
- `X/Esc` - Men ú—

**Transiciones:**
- `Exploration → Battle` (encuentro)
- `Exploration → Menu` (abrir men ú—)
- `Exploration → Evolution` (evolucionar)

### 2. BattleState

**Prop ó—sito:** Maneja el combate por turnos.

**Caracter í—sticas:**
- Sistema de turnos
- C á—lculo de daño
- Ataques y efectos
- Victoria/derrota

**Input:**
- `WASD/Flechas` - Navegar men ú— de combate
- `Space/Z` - Confirmar ataque
- `X/Esc` - Cancelar

**Transiciones:**
- `Battle → Exploration` (batalla termina)
- `Battle → Evolution` (evolucionar en batalla)
- `Battle → GameOver` (derrota)

### 3. EvolutionState

**Prop ó—sito:** Maneja el proceso de digievolucó·´·n.

**Caracter í—sticas:**
- Má—quina de estados de fases
- Animacó·´·n de evolucó·´·n
- Transformacó·´·n de stats
- Revelacó·´·n de nueva forma

**Fases:**
1. `INIT` - Inicio
2. `ANIMATION` - Efectos
3. `TRANSFORMATION` - Cambio de forma
4. `REVEAL` - Mostrar nuevo Digimon
5. `COMPLETE` - Final

**Transiciones:**
- `Evolution → Exploration` (completado)
- `Evolution → Battle` (interrumpido)

### 4. MenuState

**Prop ó—sito:** Maneja la navegacó·´·n de men ú—s.

**Caracter í—sticas:**
- Men ú— principal
- Submen ú—s (inventario, estado, etc.)
- Navegacó·´·n con cursor
- Ejecucó·´·n de opciones

**Men ú—s:**
- `MAIN` - Men ú— principal
- `INVENTORY` - Inventario
- `STATUS` - Estado del jugador
- `DIGIMON` - Equipo de Digimon
- `SAVE` - Guardar/cargar
- `OPTIONS` - Configuracó·´·n

**Input:**
- `W/S` - Navegar
- `Space/Z` - Confirmar
- `X/Esc` - Cancelar/Atr á—s

**Transiciones:**
- `Menu → Exploration` (cerrar)
- `Menu → Submenu` (entrar)
- `Submenu → Menu` (salir)

### 5. AppState

**Prop ó—sito:** Maneja las apps del Digivice.

**Caracter í—sticas:**
- Apps m ú—ltiples
- Controllers espec í—ficos
- Input contextual

**Apps:**
- `STATUS` - Ver estado
- `MAP` - Mapa
- `DATABASE` - Enciclopedia
- `SPIRIT` - Esp í—ritus
- `CAMP` - Campamento

**Input:**
- Depende de la app
- `X/Esc` - Cerrar app

**Transiciones:**
- `App → Exploration` (cerrar)
- `App → App` (cambiar app)

## C ó—mo Funciona

### 1. Interfaz IGameState

```csharp
public interface IGameState
{
    GameState StateType { get; }
    void Enter();
    void Update(float deltaTime);
    void Exit();
    void HandleInput(PlayerInput input);
}
```

### 2. LogicManager

```csharp
public class LogicManager : MonoBehaviour, ILogicManager
{
    private IGameState currentState;
    private Dictionary<GameState, IGameState> states;
    
    void Start()
    {
        // Inicializar estados
        states = new Dictionary<GameState, IGameState>
        {
            { GameState.EXPLORATION, new ExplorationState(...) },
            { GameState.BATTLE, new BattleState(...) },
            { GameState.EVOLUTION, new EvolutionState(...) },
            { GameState.MENU, new MenuState(...) },
            { GameState.APP, new AppState(...) }
        };
        
        // Estado inicial
        ChangeState(GameState.EXPLORATION);
    }
    
    void Update()
    {
        // Input
        var input = inputManager.ReadInput();
        currentState?.HandleInput(input);
        
        // Update
        currentState?.Update(Time.deltaTime);
    }
    
    public void ChangeState(GameState newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        currentState?.Enter();
    }
}
```

### 3. Ejemplo: ExplorationState

```csharp
public class ExplorationState : IGameState
{
    private readonly IWorldManager worldManager;
    private readonly IInputManager inputManager;
    
    public GameState StateType => GameState.EXPLORATION;
    
    public ExplorationState(IWorldManager wm, IInputManager im)
    {
        worldManager = wm;
        inputManager = im;
    }
    
    public void Enter()
    {
        worldManager.LoadCurrentArea();
        inputManager.EnableMovement();
    }
    
    public void Update(float deltaTime)
    {
        // L ó—gica de exploracó·´·n
    }
    
    public void Exit()
    {
        inputManager.DisableMovement();
    }
    
    public void HandleInput(PlayerInput input)
    {
        if (input.Type == InputType.MOVE)
        {
            worldManager.MovePlayer(input.Direction);
        }
    }
}
```

## A ñ—adir Nuevo Estado

### Paso 1: Crear Clase

```csharp
public class CutsceneState : IGameState
{
    public GameState StateType => GameState.CUTSCENE;
    
    public void Enter() { }
    public void Update(float deltaTime) { }
    public void Exit() { }
    public void HandleInput(PlayerInput input) { }
}
```

### Paso 2: Registrar en LogicManager

```csharp
states[GameState.CUTSCENE] = new CutsceneState(...);
```

### Paso 3: A ñ—adir Enum

```csharp
public enum GameState
{
    // ...
    CUTSCENE = 8
}
```

## Beneficios del Patr ó—n State

### 1. Có—digo Organizado

**Antes (spaghetti):**
```csharp
void Update()
{
    if (state == EXPLORATION)
    {
        if (input == MOVE)
        {
            if (canMove)
            {
                // 50 l í—neas...
            }
        }
    }
    else if (state == BATTLE)
    {
        // 200 l í—neas...
    }
    // ...
}
```

**Despu é—s (organizado):**
```csharp
void Update()
{
    currentState?.Update(Time.deltaTime);
}
```

### 2. F á—cil Testing

```csharp
[Test]
public void ExplorationState_MoveInput_ShouldMovePlayer()
{
    var state = new ExplorationState(mockWorld, mockInput);
    state.Enter();
    
    var input = new PlayerInput(InputType.MOVE, Vector2Int.up);
    state.HandleInput(input);
    
    mockWorld.Verify(w => w.MovePlayer(Vector2Int.up));
}
```

### 3. Menos Bugs

- Cada estado es independiente
- F á—cil de rastrear bugs
- Menos efectos secundarios

## Debugging

### Ver Estado Actual

```csharp
Console.WriteLine($"Estado actual: {logicManager.CurrentState}");
```

### Log de Transiciones

```csharp
public void ChangeState(GameState newState)
{
    Console.WriteLine($"[LogicManager] {currentState?.StateType} → {newState}");
    // ...
}
```

### Inspeccionar Estado

```csharp
[ContextMenu("Debug/Show State Info")]
void ShowStateInfo()
{
    Console.WriteLine($"Current: {currentState?.GetType().Name}");
    Console.WriteLine($"Type: {currentState?.StateType}");
}
```

## Mejores Pr á—cticas

### 1. Mantener Estados Peque ñ—os

- Cada estado hace UNA cosa
- Extraer l ó—gica compleja a helpers

### 2. Usar Inyeccó·´·n de Dependencias

```csharp
// ✅ BIEN
public ExplorationState(IWorldManager wm, IInputManager im)
{
    worldManager = wm;
    inputManager = im;
}

// ❌ MAL
public ExplorationState()
{
    worldManager = FindObjectOfType<WorldManager>();
}
```

### 3. Manejar Excepciones

```csharp
public void Update(float deltaTime)
{
    try
    {
        // L ó—gica
    }
    catch (Exception ex)
    {
        Console.Error($"[ExplorationState] Error: {ex.Message}");
        GameEvents.TriggerStateChanged(GameState.MENU);
    }
}
```

### 4. Limpiar Recursos

```csharp
public void Exit()
{
    // Desuscribirse de eventos
    worldManager.OnPlayerMoved -= OnPlayerMoved;
    
    // Liberar memoria
    cachedData = null;
}
```

## Referencias

- [State Pattern](https://refactoring.guru/design-patterns/state)
- [Unity State Machines](https://learn.unity.com/tutorial/state-machines)

---

**Ú·—ltima actualizacó·´·n:** 21 de septiembre de 2026  
**Versó·´·n:** 1.0
