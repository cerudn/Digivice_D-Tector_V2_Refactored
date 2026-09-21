using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// GameManager refactorizado.
/// Coordina todos los sistemas del juego usando interfaces.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static readonly object padlock = new object();
    private static GameManager _instance;
    
    /// <summary>
    /// Instancia singleton thread-safe.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            lock (padlock)
            {
                return _instance;
            }
        }
    }
    #endregion
    
    #region Dependencies (Interfaces)
    [Header("System Dependencies")]
    [SerializeField] private ILogicManager logicManager;
    [SerializeField] private IScreenManager screenManager;
    [SerializeField] private ISaveSystem saveSystem;
    [SerializeField] private IAudioManager audioManager;
    [SerializeField] private ISpriteDatabase spriteDatabase;
    [SerializeField] private IInputManager inputManager;
    [SerializeField] private IWorldManager worldManager;
    
    // Getters pó·´·blicos
    public ILogicManager Logic => logicManager;
    public IScreenManager Screen => screenManager;
    public ISaveSystem Save => saveSystem;
    public IAudioManager Audio => audioManager;
    public ISpriteDatabase Sprites => spriteDatabase;
    public IInputManager Input => inputManager;
    public IWorldManager World => worldManager;
    #endregion
    
    #region State
    private GameState currentState;
    private bool isPaused;
    private bool isInitialized;
    #endregion
    
    #region Unity Lifecycle
    void Awake()
    {
        // Singleton thread-safe
        lock (padlock)
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        Initialize();
    }
    
    void Update()
    {
        if (!isInitialized || isPaused) return;
        
        // Procesar input global
        ProcessGlobalInput();
        
        // Actualizar ló·´·gica principal
        logicManager?.UpdateState(Time.deltaTime);
    }
    
    void OnDestroy()
    {
        // Limpiar singleton
        lock (padlock)
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        // Desuscribirse de eventos
        UnsubscribeFromEvents();
    }
    #endregion
    
    #region Initialization
    /// <summary>
    /// Inicializa todos los sistemas del juego.
    /// </summary>
    private void Initialize()
    {
        Console.WriteLine("[GameManager] Inicializando sistemas...");
        
        // Inicializar cada sistema
        logicManager?.Initialize();
        screenManager?.Initialize();
        saveSystem?.Initialize();
        audioManager?.Initialize();
        spriteDatabase?.Initialize();
        inputManager?.Initialize();
        worldManager?.Initialize();
        
        // Suscribirse a eventos globales
        SubscribeToEvents();
        
        // Configurar target de FPS
        Application.targetFrameRate = GameConstants.TARGET_FPS;
        
        isInitialized = true;
        Console.WriteLine("[GameManager] Sistemas inicializados correctamente");
    }
    #endregion
    
    #region Input Processing
    private void ProcessGlobalInput()
    {
        // Input global (no procesado por LogicManager)
        // - Pausa
        // - Capturas de pantalla
        // - Comandos de debug
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowDebugInfo();
        }
        #endif
    }
    #endregion
    
    #region Pause System
    /// <summary>
    /// Alterna el estado de pausa del juego.
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            Time.timeScale = 0f;
            GameEvents.TriggerGamePaused();
            Console.WriteLine("[GameManager] Juego pausado");
        }
        else
        {
            Time.timeScale = 1f;
            GameEvents.TriggerGameResumed();
            Console.WriteLine("[GameManager] Juego reanudado");
        }
    }
    
    /// <summary>
    /// Establece el estado de pausa explí·´­citamente.
    /// </summary>
    public void SetPause(bool paused)
    {
        if (isPaused != paused)
        {
            TogglePause();
        }
    }
    #endregion
    
    #region Event Subscriptions
    private void SubscribeToEvents()
    {
        GameEvents.OnStateChanged += HandleStateChanged;
        GameEvents.OnGameSaved += HandleGameSaved;
        GameEvents.OnGameLoaded += HandleGameLoaded;
        GameEvents.OnBattleWon += HandleBattleWon;
        GameEvents.OnBattleLost += HandleBattleLost;
    }
    
    private void UnsubscribeFromEvents()
    {
        GameEvents.OnStateChanged -= HandleStateChanged;
        GameEvents.OnGameSaved -= HandleGameSaved;
        GameEvents.OnGameLoaded -= HandleGameLoaded;
        GameEvents.OnBattleWon -= HandleBattleWon;
        GameEvents.OnBattleLost -= HandleBattleLost;
    }
    #endregion
    
    #region Event Handlers
    private void HandleStateChanged(GameState newState)
    {
        currentState = newState;
        Console.WriteLine($"[GameManager] Estado cambiado a: {newState}");
        
        // Actualizar UI segó·´·n estado
        screenManager?.Render(newState);
    }
    
    private void HandleGameSaved()
    {
        Console.WriteLine("[GameManager] Partida guardada");
        screenManager?.ShowToast("Partida guardada", GameConstants.TOAST_DEFAULT_DURATION);
    }
    
    private void HandleGameLoaded()
    {
        Console.WriteLine("[GameManager] Partida cargada");
    }
    
    private void HandleBattleWon()
    {
        Console.WriteLine("[GameManager] ¡Victoria en batalla!");
        audioManager?.PlaySFX(SoundIDs.MUSIC_VICTORY);
    }
    
    private void HandleBattleLost()
    {
        Console.WriteLine("[GameManager] Derrota en batalla");
        audioManager?.PlayMusic(SoundIDs.MUSIC_GAME_OVER);
    }
    #endregion
    
    #region Debug
    [ContextMenu("Debug/Show Debug Info")]
    private void ShowDebugInfo()
    {
        Console.WriteLine("=== GAME MANAGER DEBUG INFO ===");
        Console.WriteLine($"State: {currentState}");
        Console.WriteLine($"Paused: {isPaused}");
        Console.WriteLine($"Initialized: {isInitialized}");
        Console.WriteLine($"LogicManager: {logicManager != null}");
        Console.WriteLine($"ScreenManager: {screenManager != null}");
        Console.WriteLine($"SaveSystem: {saveSystem != null}");
        Console.WriteLine("===============================");
    }
    #endregion
}
