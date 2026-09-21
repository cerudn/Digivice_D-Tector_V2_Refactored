using System;

/// <summary>
/// Interfaz para el gestor de lÃ³gica principal del juego.
/// Implementa el patrÃ³n State para manejar los diferentes estados del juego.
/// </summary>
public interface ILogicManager
{
    /// <summary>
    /// Estado actual del juego.
    /// </summary>
    GameState CurrentState { get; }
    
    /// <summary>
    /// Personaje del jugador actual.
    /// </summary>
    PlayerCharacter Player { get; }
    
    /// <summary>
    /// Inicializa el sistema de lÃ³gica.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Procesa la entrada del usuario y la traduce a acciones de juego.
    /// </summary>
    /// <param name="input">Entrada del jugador</param>
    void ProcessInput(PlayerInput input);
    
    /// <summary>
    /// Actualiza el estado actual del juego.
    /// </summary>
    /// <param name="deltaTime">Tiempo transcurrido desde el Ãºltimo frame</param>
    void UpdateState(float deltaTime);
    
    /// <summary>
    /// Cambia al estado de juego especificado.
    /// </summary>
    /// <param name="newState">Nuevo estado al que transicionar</param>
    void ChangeState(GameState newState);
    
    /// <summary>
    /// Evento disparado cuando el estado del juego cambia.
    /// </summary>
    event Action<GameState> OnStateChanged;
}

/// <summary>
/// Estados principales del juego.
/// </summary>
public enum GameState
{
    NONE = 0,
    EXPLORATION = 1,
    BATTLE = 2,
    EVOLUTION = 3,
    MENU = 4,
    APP = 5,
    CUTSCENE = 6,
    GAME_OVER = 7
}

/// <summary>
/// Estructura que representa la entrada del jugador.
/// </summary>
[System.Serializable]
public struct PlayerInput
{
    public InputType Type;
    public Vector2Int Direction;
    public int ActionID;
    public bool IsConfirm;
    public bool IsCancel;
    
    public PlayerInput(InputType type, Vector2Int direction = default, int actionID = 0)
    {
        Type = type;
        Direction = direction;
        ActionID = actionID;
        IsConfirm = false;
        IsCancel = false;
    }
}

/// <summary>
/// Tipos de entrada del jugador.
/// </summary>
public enum InputType
{
    NONE = 0,
    MOVE = 1,
    ACTION = 2,
    MENU = 3,
    CONFIRM = 4,
    CANCEL = 5,
    SPECIAL = 6
}

/// <summary>
/// Estructura simple para vectores 2D de enteros.
/// </summary>
[System.Serializable]
public struct Vector2Int
{
    public int x;
    public int y;
    
    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public static Vector2Int zero => new Vector2Int(0, 0);
    public static Vector2Int up => new Vector2Int(0, 1);
    public static Vector2Int down => new Vector2Int(0, -1);
    public static Vector2Int left => new Vector2Int(-1, 0);
    public static Vector2Int right => new Vector2Int(1, 0);
}

