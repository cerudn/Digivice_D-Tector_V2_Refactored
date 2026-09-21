using System;

/// <summary>
/// Estado de exploració·´·n del mundo.
/// Maneja movimiento, interacció·´·n y encuentros aleatorios.
/// </summary>
public class ExplorationState : IGameState
{
    private readonly IWorldManager worldManager;
    private readonly IInputManager inputManager;
    private readonly IAudioManager audioManager;
    
    public GameState StateType => GameState.EXPLORATION;
    
    public ExplorationState(IWorldManager wm, IInputManager im, IAudioManager am)
    {
        worldManager = wm ?? throw new ArgumentNullException(nameof(wm));
        inputManager = im ?? throw new ArgumentNullException(nameof(im));
        audioManager = am ?? throw new ArgumentNullException(nameof(am));
    }
    
    public void Enter()
    {
        Console.WriteLine("[ExplorationState] Enter - Cargando área actual");
        worldManager.LoadCurrentArea();
        inputManager.EnableMovement();
        audioManager.PlayMusic(SoundIDs.MUSIC_EXPLORATION);
        
        // Suscribirse a eventos
        worldManager.OnPlayerMoved += OnPlayerMoved;
    }
    
    public void Update(float deltaTime)
    {
        // Ló·´·gica de exploració·´·n por frame
        // - Animació·´·n de movimiento
        // - Detecció·´·n de encuentros
        // - Actualizació·´·n de posició·´·n
    }
    
    public void Exit()
    {
        Console.WriteLine("[ExplorationState] Exit - Limpiando estado");
        inputManager.DisableMovement();
        
        // Desuscribirse de eventos
        worldManager.OnPlayerMoved -= OnPlayerMoved;
    }
    
    public void HandleInput(PlayerInput input)
    {
        switch (input.Type)
        {
            case InputType.MOVE:
                HandleMoveInput(input.Direction);
                break;
                
            case InputType.ACTION:
                HandleActionInput();
                break;
                
            case InputType.MENU:
                HandleMenuInput();
                break;
                
            case InputType.CONFIRM:
                HandleConfirmInput();
                break;
                
            case InputType.CANCEL:
                HandleCancelInput();
                break;
        }
    }
    
    private void HandleMoveInput(Vector2Int direction)
    {
        bool moved = worldManager.MovePlayer(direction);
        
        if (moved)
        {
            // Reproducir sonido de paso
            audioManager.PlaySFX(SoundIDs.SFX_STEP);
            
            // Verificar encuentro aleatorio
            CheckRandomEncounter();
        }
    }
    
    private void HandleActionInput()
    {
        worldManager.InteractWithNearest();
    }
    
    private void HandleMenuInput()
    {
        // Disparar evento para abrir menú
        GameEvents.TriggerMenuOpened(MenuType.MAIN);
    }
    
    private void HandleConfirmInput()
    {
        // Confirmar acció·´·n contextual
    }
    
    private void HandleCancelInput()
    {
        // Cancelar acció·´·n o cerrar menú
    }
    
    private void OnPlayerMoved(UnityEngine.Vector2Int newPosition)
    {
        // Actualizar UI de posició·´·n
        // Verificar triggers de eventos
        // Guardar posició·´·n para auto-save
    }
    
    private void CheckRandomEncounter()
    {
        // Ló·´·gica de encuentro aleatorio basada en:
        // - Tasa base de encuentro
        // - Área actual
        // - Items equipados
        // - Flags de historia
        
        float encounterChance = GameConstants.BASE_ENCOUNTER_RATE;
        
        if (UnityEngine.Random.value < encounterChance)
        {
            TriggerBattleEncounter();
        }
    }
    
    private void TriggerBattleEncounter()
    {
        Console.WriteLine("[ExplorationState] ¡Encuentro aleatorio!");
        // Disparar evento de transició·´·n a batalla
        GameEvents.TriggerStateChanged(GameState.BATTLE);
    }
}
