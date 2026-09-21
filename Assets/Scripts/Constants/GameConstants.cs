/// <summary>
/// Constantes generales del juego.
/// Centraliza valores mágicos para facilitar mantenimiento y balanceo.
/// </summary>
public static class GameConstants
{
    #region Gameplay
    /// <summary>
    /// Número máximo de Digimon en el equipo.
    /// </summary>
    public const int MAX_DIGIMON_TEAM = 3;
    
    /// <summary>
    /// Número máximo de slots en el inventario.
    /// </summary>
    public const int MAX_INVENTORY_SLOTS = 20;
    
    /// <summary>
    /// Pasos necesarios para generar un huevo.
    /// </summary>
    public const int STEPS_PER_EGG = 5000;
    
    /// <summary>
    /// Probabilidad base de encuentro (0-1).
    /// </summary>
    public const float BASE_ENCOUNTER_RATE = 0.1f;
    
    /// <summary>
    /// Velocidad de movimiento del jugador (tiles por segundo).
    /// </summary>
    public const float PLAYER_MOVE_SPEED = 8f;
    #endregion
    
    #region Combat
    /// <summary>
    /// Tiempo límite por turno en segundos.
    /// </summary>
    public const float TURN_TIME_LIMIT = 30f;
    
    /// <summary>
    /// Stat base de velocidad.
    /// </summary>
    public const int BASE_SPEED = 100;
    
    /// <summary>
    /// Probabilidad de golpe crítico (0-1).
    /// </summary>
    public const float CRITICAL_CHANCE = 0.1f;
    
    /// <summary>
    /// Multiplicador de daño crítico.
    /// </summary>
    public const float CRITICAL_MULTIPLIER = 1.5f;
    
    /// <summary>
    /// Bonus de tipo efectivo (0-1).
    /// </summary>
    public const float TYPE_ADVANTAGE_BONUS = 0.2f;
    
    /// <summary>
    /// Penalización de tipo desventajoso (0-1).
    /// </summary>
    public const float TYPE_DISADVANTAGE_PENALTY = 0.2f;
    
    /// <summary>
    /// Bonus de mismo tipo (STAB) (0-1).
    /// </summary>
    public const float STAB_BONUS = 0.5f;
    #endregion
    
    #region Evolution
    /// <summary>
    /// Nivel mínimo para evolución a Champion.
    /// </summary>
    public const int MIN_LEVEL_CHAMPION = 11;
    
    /// <summary>
    /// Nivel mínimo para evolución a Ultimate.
    /// </summary>
    public const int MIN_LEVEL_ULTIMATE = 21;
    
    /// <summary>
    /// Nivel mínimo para evolución a Mega.
    /// </summary>
    public const int MIN_LEVEL_MEGA = 31;
    
    /// <summary>
    /// Friendship mínimo para evolución.
    /// </summary>
    public const int MIN_FRIENDSHIP_EVOLUTION = 50;
    
    /// <summary>
    /// Probabilidad base de éxito de evolución (0-1).
    /// </summary>
    public const float BASE_EVOLUTION_SUCCESS_RATE = 0.7f;
    #endregion
    
    #region Save System
    /// <summary>
    /// Versión actual del schema de guardado.
    /// </summary>
    public const int SAVE_VERSION = 2;
    
    /// <summary>
    /// Intervalo de auto-guardado en segundos.
    /// </summary>
    public const float AUTO_SAVE_INTERVAL = 60f;
    
    /// <summary>
    /// Número máximo de slots de guardado.
    /// </summary>
    public const int MAX_SAVE_SLOTS = 3;
    #endregion
    
    #region Performance
    /// <summary>
    /// Límite de sprites en cache.
    /// </summary>
    public const int SPRITE_CACHE_LIMIT = 100;
    
    /// <summary>
    /// Timeout de carga asíncrona en segundos.
    /// </summary>
    public const float ASYNC_LOAD_TIMEOUT = 5f;
    
    /// <summary>
    /// Target de FPS.
    /// </summary>
    public const int TARGET_FPS = 60;
    #endregion
    
    #region UI
    /// <summary>
    /// Duración por defecto de toast messages.
    /// </summary>
    public const float TOAST_DEFAULT_DURATION = 2f;
    
    /// <summary>
    /// Tiempo de animación de transición.
    /// </summary>
    public const float TRANSITION_ANIMATION_TIME = 0.3f;
    #endregion
}
