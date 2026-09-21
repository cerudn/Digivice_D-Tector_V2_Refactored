using System;

/// <summary>
/// Estado de ejecucó·´·n de apps del Digivice.
/// Maneja las diferentes aplicaciones (Status, Map, Database, etc.).
/// </summary>
public class AppState : IGameState
{
    private readonly ILogicManager logicManager;
    private readonly IScreenManager screenManager;
    private readonly IAudioManager audioManager;
    private readonly IInputManager inputManager;
    
    public GameState StateType => GameState.APP;
    
    // Estado de la app
    private AppType currentApp;
    private IAppController currentAppController;
    private bool isAppRunning;
    
    public AppState(ILogicManager lm, IScreenManager sm, IAudioManager am, IInputManager im)
    {
        logicManager = lm ?? throw new ArgumentNullException(nameof(lm));
        screenManager = sm ?? throw new ArgumentNullException(nameof(sm));
        audioManager = am ?? throw new ArgumentNullException(nameof(am));
        inputManager = im ?? throw new ArgumentNullException(nameof(im));
    }
    
    public void Enter()
    {
        Console.WriteLine("[AppState] Enter - Abriendo app");
        
        isAppRunning = true;
        
        // Habilitar input
        inputManager.EnableMenuInput();
        
        // Reproducir sonido
        audioManager.PlaySFX(SoundIDs.SFX_DIGIVICE);
        
        // Iniciar app actual
        if (currentAppController != null)
        {
            currentAppController.OnOpen();
        }
    }
    
    public void Update(float deltaTime)
    {
        if (!isAppRunning) return;
        
        // Actualizar app actual
        if (currentAppController != null)
        {
            currentAppController.OnUpdate(deltaTime);
        }
    }
    
    public void Exit()
    {
        Console.WriteLine("[AppState] Exit - Cerrando app");
        
        isAppRunning = false;
        
        inputManager.DisableMenuInput();
        
        // Cerrar app actual
        if (currentAppController != null)
        {
            currentAppController.OnClose();
        }
    }
    
    public void HandleInput(PlayerInput input)
    {
        if (!isAppRunning) return;
        
        // Input espec í—fico de la app
        if (currentAppController != null)
        {
            currentAppController.OnInput(input);
        }
        
        // Input global
        if (input.IsCancel)
        {
            CloseApp();
        }
    }
    
    #region App Management
    /// <summary>
    /// Abre una app espec í—fica.
    /// </summary>
    public void OpenApp(AppType appType)
    {
        currentApp = appType;
        currentAppController = CreateAppController(appType);
        
        Console.WriteLine($"[AppState] Abriendo app: {appType}");
        
        Enter();
    }
    
    /// <summary>
    /// Cierra la app actual.
    /// </summary>
    public void CloseApp()
    {
        Console.WriteLine($"[AppState] Cerrando app: {currentApp}");
        
        Exit();
        
        // Volver al estado anterior
        GameEvents.TriggerStateChanged(GameState.EXPLORATION);
    }
    
    /// <summary>
    /// Crea el controller para una app.
    /// </summary>
    private IAppController CreateAppController(AppType appType)
    {
        switch (appType)
        {
            case AppType.STATUS:
                return new StatusAppController();
                
            case AppType.MAP:
                return new MapAppController();
                
            case AppType.DATABASE:
                return new DatabaseAppController();
                
            case AppType.SPIRIT:
                return new SpiritAppController();
                
            case AppType.CAMP:
                return new CampAppController();
                
            default:
                Console.Warning($"[AppState] App {appType} no implementada");
                return new DefaultAppController();
        }
    }
    #endregion
    
    #region App Controllers
    /// <summary>
    /// Controller base para apps.
    /// </summary>
    private interface IAppController
    {
        void OnOpen();
        void OnUpdate(float deltaTime);
        void OnInput(PlayerInput input);
        void OnClose();
    }
    
    private class StatusAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[StatusApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[StatusApp] Close");
    }
    
    private class MapAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[MapApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[MapApp] Close");
    }
    
    private class DatabaseAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[DatabaseApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[DatabaseApp] Close");
    }
    
    private class SpiritAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[SpiritApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[SpiritApp] Close");
    }
    
    private class CampAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[CampApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[CampApp] Close");
    }
    
    private class DefaultAppController : IAppController
    {
        public void OnOpen() => Console.WriteLine("[DefaultApp] Open");
        public void OnUpdate(float dt) { }
        public void OnInput(PlayerInput input) { }
        public void OnClose() => Console.WriteLine("[DefaultApp] Close");
    }
    #endregion
}

/// <summary>
/// Tipos de apps disponibles.
/// </summary>
public enum AppType
{
    NONE = 0,
    STATUS = 1,
    MAP = 2,
    DATABASE = 3,
    SPIRIT = 4,
    CAMP = 5,
    CODE = 6,
    GAMES = 7
}
