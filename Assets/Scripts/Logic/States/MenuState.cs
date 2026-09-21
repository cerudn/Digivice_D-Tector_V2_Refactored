using System;
using System.Collections.Generic;

/// <summary>
/// Estado de navegacó·´·n de men ú—s.
/// Maneja todos los men ú—s del juego (main menu, options, inventory, etc.).
/// </summary>
public class MenuState : IGameState
{
    private readonly ILogicManager logicManager;
    private readonly IScreenManager screenManager;
    private readonly IAudioManager audioManager;
    private readonly IInputManager inputManager;
    
    public GameState StateType => GameState.MENU;
    
    // Estado del men ú—
    private MenuType currentMenu;
    private int selectedIndex;
    private List<string> menuOptions;
    private bool isSubMenu;
    
    public MenuState(ILogicManager lm, IScreenManager sm, IAudioManager am, IInputManager im)
    {
        logicManager = lm ?? throw new ArgumentNullException(nameof(lm));
        screenManager = sm ?? throw new ArgumentNullException(nameof(sm));
        audioManager = am ?? throw new ArgumentNullException(nameof(am));
        inputManager = im ?? throw new ArgumentNullException(nameof(im));
    }
    
    public void Enter()
    {
        Console.WriteLine("[MenuState] Enter - Abriendo men ú—");
        
        selectedIndex = 0;
        isSubMenu = false;
        
        // Habilitar input de men ú—
        inputManager.EnableMenuInput();
        
        // Reproducir sonido de men ú—
        audioManager.PlaySFX(SoundIDs.SFX_CURSOR);
        
        // Mostrar men ú— actual
        RenderMenu();
    }
    
    public void Update(float deltaTime)
    {
        // Actualizar animaciones del men ú—
        // Timeout por inactividad
    }
    
    public void Exit()
    {
        Console.WriteLine("[MenuState] Exit - Cerrando men ú—");
        
        inputManager.DisableMenuInput();
        
        // Limpiar men ú—
        menuOptions?.Clear();
    }
    
    public void HandleInput(PlayerInput input)
    {
        switch (input.Type)
        {
            case InputType.MOVE:
                HandleMoveInput(input.Direction);
                break;
                
            case InputType.CONFIRM:
                HandleConfirmInput();
                break;
                
            case InputType.CANCEL:
                HandleCancelInput();
                break;
        }
    }
    
    #region Input Handling
    private void HandleMoveInput(Vector2Int direction)
    {
        // Navegacó·´·n vertical
        if (direction.y > 0)
        {
            selectedIndex = (selectedIndex - 1 + menuOptions.Count) % menuOptions.Count;
            audioManager.PlaySFX(SoundIDs.SFX_CURSOR);
            RenderMenu();
        }
        else if (direction.y < 0)
        {
            selectedIndex = (selectedIndex + 1) % menuOptions.Count;
            audioManager.PlaySFX(SoundIDs.SFX_CURSOR);
            RenderMenu();
        }
    }
    
    private void HandleConfirmInput()
    {
        audioManager.PlaySFX(SoundIDs.SFX_CONFIRM);
        
        // Ejecutar opcó·´·n seleccionada
        ExecuteOption(selectedIndex);
    }
    
    private void HandleCancelInput()
    {
        audioManager.PlaySFX(SoundIDs.SFX_CANCEL);
        
        if (isSubMenu)
        {
            // Volver al men ú— principal
            currentMenu = MenuType.MAIN;
            RenderMenu();
        }
        else
        {
            // Cerrar men ú— completamente
            GameEvents.TriggerMenuClosed();
            GameEvents.TriggerStateChanged(GameState.EXPLORATION);
        }
    }
    #endregion
    
    #region Menu Rendering
    private void RenderMenu()
    {
        switch (currentMenu)
        {
            case MenuType.MAIN:
                RenderMainMenu();
                break;
                
            case MenuType.INVENTORY:
                RenderInventory();
                break;
                
            case MenuType.STATUS:
                RenderStatus();
                break;
                
            case MenuType.DIGIMON:
                RenderDigimonMenu();
                break;
                
            case MenuType.SAVE:
                RenderSaveMenu();
                break;
                
            case MenuType.OPTIONS:
                RenderOptionsMenu();
                break;
        }
        
        GameEvents.TriggerShowMessage(currentMenu.ToString(), 0f);
    }
    
    private void RenderMainMenu()
    {
        menuOptions = new List<string>
        {
            "Estado",
            "Digimon",
            "Inventario",
            "Mapa",
            "Guardar",
            "Opciones",
            "Salir"
        };
        
        Console.WriteLine($"[MenuState] Renderizando men ú— principal - Opcoó·´·n {selectedIndex}");
    }
    
    private void RenderInventory()
    {
        menuOptions = new List<string>
        {
            "Items",
            "Equipamiento",
            "Materiales",
            "Atr á—s"
        };
        
        Console.WriteLine("[MenuState] Renderizando inventario");
    }
    
    private void RenderStatus()
    {
        menuOptions = new List<string>
        {
            "Jugador",
            "Estad í—sticas",
            "Atr á—s"
        };
        
        Console.WriteLine("[MenuState] Renderizando estado");
    }
    
    private void RenderDigimonMenu()
    {
        menuOptions = new List<string>
        {
            "Ver Equipo",
            "Cambiar Orden",
            "Ataques",
            "Atr á—s"
        };
        
        Console.WriteLine("[MenuState] Renderizando men ú— de Digimon");
    }
    
    private void RenderSaveMenu()
    {
        menuOptions = new List<string>
        {
            "Guardar Partida",
            "Cargar Partida",
            "Borrar Partida",
            "Atr á—s"
        };
        
        Console.WriteLine("[MenuState] Renderizando men ú— de guardado");
    }
    
    private void RenderOptionsMenu()
    {
        menuOptions = new List<string>
        {
            "Configuracó·´·n",
            "Cr é—ditos",
            "Atr á—s"
        };
        
        Console.WriteLine("[MenuState] Renderizando opciones");
    }
    #endregion
    
    #region Option Execution
    private void ExecuteOption(int index)
    {
        switch (currentMenu)
        {
            case MenuType.MAIN:
                ExecuteMainOption(index);
                break;
                
            case MenuType.INVENTORY:
                ExecuteInventoryOption(index);
                break;
                
            // ... más casos seg ú—n sea necesario
        }
    }
    
    private void ExecuteMainOption(int index)
    {
        switch (index)
        {
            case 0: // Estado
                currentMenu = MenuType.STATUS;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 1: // Digimon
                currentMenu = MenuType.DIGIMON;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 2: // Inventario
                currentMenu = MenuType.INVENTORY;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 3: // Mapa
                currentMenu = MenuType.MAP;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 4: // Guardar
                currentMenu = MenuType.SAVE;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 5: // Opciones
                currentMenu = MenuType.OPTIONS;
                isSubMenu = true;
                selectedIndex = 0;
                RenderMenu();
                break;
                
            case 6: // Salir
                GameEvents.TriggerMenuClosed();
                GameEvents.TriggerStateChanged(GameState.EXPLORATION);
                break;
        }
    }
    
    private void ExecuteInventoryOption(int index)
    {
        switch (index)
        {
            case 0: // Items
                // Mostrar lista de items
                break;
                
            case 1: // Equipamiento
                // Mostrar equipamiento
                break;
                
            case 2: // Materiales
                // Mostrar materiales
                break;
                
            case 3: // Atr á—s
                currentMenu = MenuType.MAIN;
                isSubMenu = false;
                selectedIndex = 2;
                RenderMenu();
                break;
        }
    }
    #endregion
    
    #region Utility
    /// <summary>
    /// Abre un men ú— espec í—fico.
    /// </summary>
    public void OpenMenu(MenuType menuType)
    {
        currentMenu = menuType;
        selectedIndex = 0;
        isSubMenu = false;
        Enter();
    }
    #endregion
}
