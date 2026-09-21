using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Gestor de ló·´·gica principal del juego.
/// Implementa m á—quina de estados con validaciones de seguridad.
/// </summary>
public class LogicManager : MonoBehaviour, ILogicManager
{
    #region Singleton & Interfaces
    private static LogicManager _instance;
    
    public static LogicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LogicManager>();
                
                if (_instance == null)
                {
                    Debug.LogError("[LogicManager] No se encontró LogicManager en la escena!");
                }
            }
            
            return _instance;
        }
    }
    #endregion
    
    #region ILogicManager Implementation
    public GameState CurrentState { get; private set; }
    public PlayerCharacter Player { get; private set; }
    
    public event Action<GameState> OnStateChanged;
    #endregion
    
    #region State Machine
    private IGameState currentState;
    private Dictionary<GameState, IGameState> states;
    
    // Dependencias
    private IScreenManager screenManager;
    private IInputManager inputManager;
    private ISpriteDatabase spriteDatabase;
    #endregion
    
    #region State
    private bool isInitialized;
    #endregion
    
    #region Unity Lifecycle
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[LogicManager] Start() llamado antes de Initialize()");
            Initialize(screenManager, inputManager, spriteDatabase);
        }
    }
    
    void Update()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[LogicManager] Update() llamado pero no está inicializado");
            return;
        }
        
        try
        {
            // Procesar input
            var input = inputManager?.ReadInput();
            
            if (input.HasValue && input.Value.Type != InputType.NONE)
            {
                currentState?.HandleInput(input.Value);
            }
            
            // Actualizar estado
            currentState?.Update(Time.deltaTime);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Error en Update: {ex.Message}");
            Debug.LogError($"[LogicManager] Stack: {ex.StackTrace}");
            
            // Fallback a estado seguro
            SafeChangeState(GameState.MENU);
        }
    }
    #endregion
    
    #region Initialization
    /// <summary>
    /// Inicializa el LogicManager con dependencias.
    /// </summary>
    public void Initialize(IScreenManager sm, IInputManager im, ISpriteDatabase sd)
    {
        if (isInitialized)
        {
            Debug.LogWarning("[LogicManager] Ya está inicializado");
            return;
        }
        
        Debug.Log("[LogicManager] Inicializando...");
        
        screenManager = sm;
        inputManager = im;
        spriteDatabase = sd;
        
        // Validar dependencias
        if (screenManager == null)
        {
            Debug.LogError("[LogicManager] ScreenManager es null!");
            return;
        }
        
        if (inputManager == null)
        {
            Debug.LogError("[LogicManager] InputManager es null!");
            return;
        }
        
        if (spriteDatabase == null)
        {
            Debug.LogError("[LogicManager] SpriteDatabase es null!");
            return;
        }
        
        // Inicializar estados
        InitializeStates();
        
        // Estado inicial
        SafeChangeState(GameState.EXPLORATION);
        
        isInitialized = true;
        Debug.Log("[LogicManager] Inicializado correctamente");
    }
    
    private void InitializeStates()
    {
        states = new Dictionary<GameState, IGameState>();
        
        // Crear estados con dependencias
        try
        {
            states[GameState.EXPLORATION] = new ExplorationState(
                new WorldManagerWrapper(this),
                inputManager
            );
            
            states[GameState.BATTLE] = new BattleState(
                this,
                inputManager,
                screenManager,
                spriteDatabase
            );
            
            states[GameState.EVOLUTION] = new EvolutionState(
                this,
                screenManager,
                screenManager as AudioManager,
                spriteDatabase
            );
            
            states[GameState.MENU] = new MenuState(
                this,
                screenManager,
                screenManager as AudioManager,
                inputManager
            );
            
            states[GameState.APP] = new AppState(
                this,
                screenManager,
                screenManager as AudioManager,
                inputManager
            );
            
            Debug.Log($"[LogicManager] {states.Count} estados inicializados");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Error inicializando estados: {ex.Message}");
            throw;
        }
    }
    #endregion
    
    #region State Management
    /// <summary>
    /// Cambia al estado especificado con validaciones de seguridad.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[LogicManager] ChangeState() llamado pero no está inicializado");
            return;
        }
        
        SafeChangeState(newState);
    }
    
    /// <summary>
    /// Cambia de estado con validaciones y manejo de errores.
    /// </summary>
    private void SafeChangeState(GameState newState)
    {
        try
        {
            // VALIDACIÓ·N CR Í—TICA #1: Verificar que el estado existe
            if (states == null)
            {
                Debug.LogError("[LogicManager] Dictionary de estados es null!");
                return;
            }
            
            if (!states.ContainsKey(newState))
            {
                Debug.LogError($"[LogicManager] Estado {newState} no registrado en el Dictionary!");
                Debug.LogError($"[LogicManager] Estados disponibles: {string.Join(", ", states.Keys)}");
                
                // Fallback a estado seguro
                if (states.ContainsKey(GameState.MENU))
                {
                    Debug.LogWarning("[LogicManager] Fallback a MENU state");
                    PerformStateChange(GameState.MENU);
                }
                return;
            }
            
            // VALIDACIÓ·N CR Í—TICA #2: Verificar que el estado no es null
            if (states[newState] == null)
            {
                Debug.LogError($"[LogicManager] Estado {newState} es null en el Dictionary!");
                return;
            }
            
            // VALIDACIÓ·N CR Í—TICA #3: Prevenir cambio al mismo estado
            if (currentState == states[newState])
            {
                Debug.LogWarning($"[LogicManager] Intento de cambiar al mismo estado: {newState}");
                return;
            }
            
            // Realizar cambio
            PerformStateChange(newState);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Excepció·´·n en SafeChangeState: {ex.Message}");
            Debug.LogError($"[LogicManager] Stack: {ex.StackTrace}");
            
            // Fallback extremo
            if (states != null && states.ContainsKey(GameState.MENU))
            {
                Debug.LogError("[LogicManager] Fallback EXTREMO a MENU state");
                PerformStateChange(GameState.MENU);
            }
        }
    }
    
    /// <summary>
    /// Realiza el cambio de estado propiamente dicho.
    /// </summary>
    private void PerformStateChange(GameState newState)
    {
        Debug.Log($"[LogicManager] Cambiando estado: {currentState?.GetType().Name ?? "null"} → {newState}");
        
        try
        {
            // Salir del estado actual
            if (currentState != null)
            {
                Debug.Log($"[LogicManager] Saliendo de {currentState.GetType().Name}");
                currentState.Exit();
            }
            
            // Cambiar estado
            var previousState = currentState;
            currentState = states[newState];
            CurrentState = newState;
            
            // Entrar al nuevo estado
            if (currentState != null)
            {
                Debug.Log($"[LogicManager] Entrando a {currentState.GetType().Name}");
                currentState.Enter();
            }
            else
            {
                Debug.LogError($"[LogicManager] ¡ERROR CR Í—TICO! currentState es null después del cambio!");
                throw new InvalidOperationException("currentState es null después del cambio");
            }
            
            // Disparar evento
            OnStateChanged?.Invoke(newState);
            GameEvents.TriggerStateChanged(newState);
            
            Debug.Log($"[LogicManager] Cambio completado: {newState}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Error en PerformStateChange: {ex.Message}");
            Debug.LogError($"[LogicManager] Stack: {ex.StackTrace}");
            
            // Revertir al estado anterior si es posible
            if (previousState != null)
            {
                Debug.LogWarning("[LogicManager] Revertiendo al estado anterior");
                currentState = previousState;
            }
            
            throw; // Re-lanzar para que lo maneje SafeChangeState
        }
    }
    #endregion
    
    #region ILogicManager Methods
    public void ProcessInput(PlayerInput input)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[LogicManager] ProcessInput() llamado pero no está inicializado");
            return;
        }
        
        try
        {
            currentState?.HandleInput(input);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Error procesando input: {ex.Message}");
        }
    }
    
    public void UpdateState(float deltaTime)
    {
        if (!isInitialized)
        {
            return;
        }
        
        try
        {
            currentState?.Update(deltaTime);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LogicManager] Error actualizando estado: {ex.Message}");
        }
    }
    #endregion
    
    #region Utility
    /// <summary>
    /// Verifica si un estado está registrado.
    /// </summary>
    public bool IsStateRegistered(GameState state)
    {
        return states != null && states.ContainsKey(state);
    }
    
    /// <summary>
    /// Obtiene el estado actual de forma segura.
    /// </summary>
    public IGameState GetCurrentStateSafe()
    {
        return currentState;
    }
    #endregion
}

/// <summary>
/// Wrapper para WorldManager que usa LogicManager.
/// </summary>
public class WorldManagerWrapper : IWorldManager
{
    private readonly LogicManager logicManager;
    
    public WorldData CurrentWorld => null; // TODO: Implementar
    public UnityEngine.Vector2Int PlayerPosition => UnityEngine.Vector2Int.zero; // TODO: Implementar
    
    public event Action<UnityEngine.Vector2Int> OnPlayerMoved;
    public event Action<InteractionData> OnInteraction;
    
    public WorldManagerWrapper(LogicManager lm)
    {
        logicManager = lm;
    }
    
    public void Initialize() { }
    public void LoadCurrentArea() { }
    public bool MovePlayer(UnityEngine.Vector2Int direction) => true; // TODO: Implementar
    public void InteractWithNearest() { }
    public void TeleportPlayer(UnityEngine.Vector2Int position) { }
    public void LoadWorld(int worldID) { }
}
