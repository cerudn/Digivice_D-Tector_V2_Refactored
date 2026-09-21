using System;
using System.Threading.Tasks;

/// <summary>
/// Interfaz para la base de datos de sprites.
/// Gestiona la carga, cacheo y provisión de sprites.
/// </summary>
public interface ISpriteDatabase
{
    /// <summary>
    /// Inicializa la base de datos de sprites.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Obtiene el sprite en reposo de un Digimon.
    /// </summary>
    /// <param name="digimonID">ID del Digimon</param>
    /// <returns>Sprite del Digimon o null si no existe</returns>
    UnityEngine.Sprite GetDigimonSprite(int digimonID);
    
    /// <summary>
    /// Obtiene la animación completa de un Digimon.
    /// </summary>
    /// <param name="digimonID">ID del Digimon</param>
    /// <param name="type">Tipo de animación</param>
    /// <returns>Array de sprites de la animación</returns>
    UnityEngine.Sprite[] GetDigimonAnimation(int digimonID, AnimationType type);
    
    /// <summary>
    /// Obtiene el sprite de un Espíritu.
    /// </summary>
    /// <param name="spiritID">ID del Espíritu</param>
    /// <returns>Sprite del Espíritu</returns>
    UnityEngine.Sprite GetSpiritSprite(int spiritID);
    
    /// <summary>
    /// Obtiene un elemento de UI por nombre.
    /// </summary>
    /// <param name="elementName">Nombre del elemento</param>
    /// <returns>Sprite del elemento de UI</returns>
    UnityEngine.Sprite GetUIElement(string elementName);
    
    /// <summary>
    /// Obtiene un sprite de forma asíncrona sin bloquear el hilo principal.
    /// </summary>
    /// <param name="digimonID">ID del Digimon</param>
    /// <returns>Tarea con el sprite resultante</returns>
    Task<UnityEngine.Sprite> GetDigimonSpriteAsync(int digimonID);
    
    /// <summary>
    /// Precarga sprites en el cache para rendimiento.
    /// </summary>
    /// <param name="digimonIDs">IDs de Digimon a precargar</param>
    void PreloadSprites(int[] digimonIDs);
    
    /// <summary>
    /// Limpia el cache de sprites para liberar memoria.
    /// </summary>
    void ClearCache();
}

/// <summary>
/// Tipos de animación disponibles.
/// </summary>
public enum AnimationType
{
    IDLE = 0,
    WALK = 1,
    RUN = 2,
    ATTACK = 3,
    HIT = 4,
    EVOLUTION = 5,
    VICTORY = 6,
    FAINT = 7
}
