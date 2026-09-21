using System;

/// <summary>
/// Interfaz para el gestor de interfaz de usuario y rendering.
/// </summary>
public interface IScreenManager
{
    /// <summary>
    /// Pantalla actual que se está mostrando.
    /// </summary>
    ScreenType CurrentScreen { get; }
    
    /// <summary>
    /// Inicializa el sistema de UI.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Renderiza el estado actual del juego.
    /// </summary>
    /// <param name="gameState">Estado actual a renderizar</param>
    void Render(GameState gameState);
    
    /// <summary>
    /// Transiciona a una pantalla específica.
    /// </summary>
    /// <param name="screen">Tipo de pantalla a mostrar</param>
    /// <param name="data">Datos opcionales para la pantalla</param>
    void Transition(ScreenType screen, object data = null);
    
    /// <summary>
    /// Muestra un mensaje temporal en pantalla.
    /// </summary>
    /// <param name="message">Mensaje a mostrar</param>
    /// <param name="duration">Duración en segundos</param>
    void ShowToast(string message, float duration = 2f);
    
    /// <summary>
    /// Limpia la pantalla actual.
    /// </summary>
    void Clear();
    
    /// <summary>
    /// Evento disparado cuando la pantalla cambia.
    /// </summary>
    event Action<ScreenType> OnScreenChanged;
}

/// <summary>
/// Tipos de pantalla disponibles en el juego.
/// </summary>
public enum ScreenType
{
    NONE = 0,
    TITLE = 1,
    MAIN_MENU = 2,
    GAME = 3,
    BATTLE = 4,
    EVOLUTION = 5,
    APP = 6,
    DIALOGUE = 7,
    GAME_OVER = 8
}
