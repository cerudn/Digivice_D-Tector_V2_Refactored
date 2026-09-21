/// <summary>
/// Interfaz para todos los estados del juego.
/// Implementa el patró·´·n State para LogicManager.
/// </summary>
public interface IGameState
{
    /// <summary>
    /// Tipo de estado que representa.
    /// </summary>
    GameState StateType { get; }
    
    /// <summary>
    /// Llamado cuando se entra al estado.
    /// </summary>
    void Enter();
    
    /// <summary>
    /// Llamado en cada frame mientras el estado está activo.
    /// </summary>
    /// <param name="deltaTime">Tiempo transcurrido</param>
    void Update(float deltaTime);
    
    /// <summary>
    /// Llamado cuando se sale del estado.
    /// </summary>
    void Exit();
    
    /// <summary>
    /// Procesa la entrada del jugador.
    /// </summary>
    /// <param name="input">Entrada a procesar</param>
    void HandleInput(PlayerInput input);
}
