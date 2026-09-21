using System;

/// <summary>
/// Sistema global de eventos para comunicación entre sistemas.
/// Implementa el patrón Observer para reducir acoplamiento.
/// </summary>
public static class GameEvents
{
    #region GameState
    /// <summary>
    /// Disparado cuando el estado del juego cambia.
    /// </summary>
    public static event Action<GameState> OnStateChanged;
    public static void TriggerStateChanged(GameState state) => OnStateChanged?.Invoke(state);
    
    /// <summary>
    /// Disparado cuando el juego se pausa.
    /// </summary>
    public static event Action OnGamePaused;
    public static void TriggerGamePaused() => OnGamePaused?.Invoke();
    
    /// <summary>
    /// Disparado cuando el juego se reanuda.
    /// </summary>
    public static event Action OnGameResumed;
    public static void TriggerGameResumed() => OnGameResumed?.Invoke();
    #endregion
    
    #region Combat
    /// <summary>
    /// Disparado cuando comienza un ataque.
    /// </summary>
    public static event Action<Digimon, Digimon, Attack> OnAttackStarted;
    public static void TriggerAttackStarted(Digimon attacker, Digimon defender, Attack attack) => 
        OnAttackStarted?.Invoke(attacker, defender, attack);
    
    /// <summary>
    /// Disparado cuando se inflige daño.
    /// </summary>
    public static event Action<Digimon, int> OnDamageDealt;
    public static void TriggerDamageDealt(Digimon target, int damage) => 
        OnDamageDealt?.Invoke(target, damage);
    
    /// <summary>
    /// Disparado cuando un Digimon se debilita.
    /// </summary>
    public static event Action<Digimon> OnDigimonFainted;
    public static void TriggerDigimonFainted(Digimon digimon) => 
        OnDigimonFainted?.Invoke(digimon);
    
    /// <summary>
    /// Disparado cuando se gana una batalla.
    /// </summary>
    public static event Action OnBattleWon;
    public static void TriggerBattleWon() => OnBattleWon?.Invoke();
    
    /// <summary>
    /// Disparado cuando se pierde una batalla.
    /// </summary>
    public static event Action OnBattleLost;
    public static void TriggerBattleLost() => OnBattleLost?.Invoke();
    #endregion
    
    #region Evolution
    /// <summary>
    /// Disparado cuando comienza una evolución.
    /// </summary>
    public static event Action<Digimon, Digimon> OnEvolutionStarted;
    public static void TriggerEvolutionStarted(Digimon from, Digimon to) => 
        OnEvolutionStarted?.Invoke(from, to);
    
    /// <summary>
    /// Disparado cuando se completa una evolución.
    /// </summary>
    public static event Action<Digimon> OnEvolutionCompleted;
    public static void TriggerEvolutionCompleted(Digimon digimon) => 
        OnEvolutionCompleted?.Invoke(digimon);
    
    /// <summary>
    /// Disparado cuando falla una evolución.
    /// </summary>
    public static event Action<Digimon> OnEvolutionFailed;
    public static void TriggerEvolutionFailed(Digimon digimon) => 
        OnEvolutionFailed?.Invoke(digimon);
    #endregion
    
    #region Save/Load
    /// <summary>
    /// Disparado cuando se guarda la partida.
    /// </summary>
    public static event Action OnGameSaved;
    public static void TriggerGameSaved() => OnGameSaved?.Invoke();
    
    /// <summary>
    /// Disparado cuando se carga la partida.
    /// </summary>
    public static event Action OnGameLoaded;
    public static void TriggerGameLoaded() => OnGameLoaded?.Invoke();
    
    /// <summary>
    /// Disparado cuando ocurre un error de guardado.
    /// </summary>
    public static event Action<string> OnSaveError;
    public static void TriggerSaveError(string message) => OnSaveError?.Invoke(message);
    #endregion
    
    #region Player
    /// <summary>
    /// Disparado cuando cambian los pasos del jugador.
    /// </summary>
    public static event Action<int> OnStepsChanged;
    public static void TriggerStepsChanged(int steps) => OnStepsChanged?.Invoke(steps);
    
    /// <summary>
    /// Disparado cuando el jugador sube de nivel.
    /// </summary>
    public static event Action<int> OnLevelUp;
    public static void TriggerLevelUp(int newLevel) => OnLevelUp?.Invoke(newLevel);
    
    /// <summary>
    /// Disparado cuando se adquiere un item.
    /// </summary>
    public static event Action<Item> OnItemAcquired;
    public static void TriggerItemAcquired(Item item) => OnItemAcquired?.Invoke(item);
    
    /// <summary>
    /// Disparado cuando el jugador se mueve.
    /// </summary>
    public static event Action<UnityEngine.Vector2Int> OnPlayerMoved;
    public static void TriggerPlayerMoved(UnityEngine.Vector2Int position) => 
        OnPlayerMoved?.Invoke(position);
    #endregion
    
    #region UI
    /// <summary>
    /// Disparado cuando se debe mostrar un mensaje.
    /// </summary>
    public static event Action<string, float> OnShowMessage;
    public static void TriggerShowMessage(string message, float duration = 2f) => 
        OnShowMessage?.Invoke(message, duration);
    
    /// <summary>
    /// Disparado cuando se abre un menú.
    /// </summary>
    public static event Action<MenuType> OnMenuOpened;
    public static void TriggerMenuOpened(MenuType menuType) => OnMenuOpened?.Invoke(menuType);
    
    /// <summary>
    /// Disparado cuando se cierra un menú.
    /// </summary>
    public static event Action OnMenuClosed;
    public static void TriggerMenuClosed() => OnMenuClosed?.Invoke();
    #endregion
}

/// <summary>
/// Tipos de menú disponibles.
/// </summary>
public enum MenuType
{
    MAIN = 0,
    OPTIONS = 1,
    INVENTORY = 2,
    STATUS = 3,
    DIGIMON = 4,
    SAVE = 5,
    MAP = 6
}
