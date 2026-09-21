using System;

/// <summary>
/// Interfaz para el gestor del mundo y exploración.
/// </summary>
public interface IWorldManager
{
    /// <summary>
    /// Mundo actual.
    /// </summary>
    WorldData CurrentWorld { get; }
    
    /// <summary>
    /// Posición actual del jugador.
    /// </summary>
    UnityEngine.Vector2Int PlayerPosition { get; }
    
    /// <summary>
    /// Inicializa el gestor del mundo.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Carga el área actual del mundo.
    /// </summary>
    void LoadCurrentArea();
    
    /// <summary>
    /// Mueve al jugador en la dirección especificada.
    /// </summary>
    /// <param name="direction">Dirección de movimiento</param>
    /// <returns>True si el movimiento fue exitoso</returns>
    bool MovePlayer(UnityEngine.Vector2Int direction);
    
    /// <summary>
    /// Interactúa con el objeto más cercano.
    /// </summary>
    void InteractWithNearest();
    
    /// <summary>
    /// Teletransporta al jugador a una posición específica.
    /// </summary>
    /// <param name="position">Nueva posición</param>
    void TeleportPlayer(UnityEngine.Vector2Int position);
    
    /// <summary>
    /// Carga un mundo específico.
    /// </summary>
    /// <param name="worldID">ID del mundo</param>
    void LoadWorld(int worldID);
    
    /// <summary>
    /// Evento disparado cuando el jugador cambia de posición.
    /// </summary>
    event Action<UnityEngine.Vector2Int> OnPlayerMoved;
    
    /// <summary>
    /// Evento disparado cuando ocurre una interacción.
    /// </summary>
    event Action<InteractionData> OnInteraction;
}

/// <summary>
/// Datos de una interacción.
/// </summary>
[System.Serializable]
public class InteractionData
{
    public int objectID;
    public string interactionType;
    public object data;
}
