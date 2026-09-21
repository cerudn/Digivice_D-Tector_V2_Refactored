using System;

/// <summary>
/// Interfaz para el sistema de guardado y carga de partidas.
/// Incluye versionado para migración entre versiones.
/// </summary>
public interface ISaveSystem
{
    /// <summary>
    /// Versión actual del schema de guardado.
    /// </summary>
    int CurrentVersion { get; }
    
    /// <summary>
    /// Inicializa el sistema de guardado.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Guarda los datos de la partida.
    /// </summary>
    /// <param name="data">Datos a guardar</param>
    void Save(SaveData data);
    
    /// <summary>
    /// Carga los datos de la partida.
    /// </summary>
    /// <returns>Datos cargados o null si no existe</returns>
    SaveData Load();
    
    /// <summary>
    /// Verifica si existen datos de guardado.
    /// </summary>
    /// <returns>True si existe guardado, false en caso contrario</returns>
    bool HasSaveData();
    
    /// <summary>
    /// Elimina los datos de guardado.
    /// </summary>
    void DeleteSave();
    
    /// <summary>
    /// Evento disparado cuando se completa un guardado.
    /// </summary>
    event Action OnSaveCompleted;
    
    /// <summary>
    /// Evento disparado cuando se completa una carga.
    /// </summary>
    event Action OnLoadCompleted;
    
    /// <summary>
    /// Evento disparado cuando ocurre un error de guardado.
    /// </summary>
    event Action<string> OnSaveError;
}

/// <summary>
/// Datos principales de la partida.
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// Versión del schema de guardado.
    /// </summary>
    public int version = 2;
    
    /// <summary>
    /// Checksum para validación de integridad.
    /// </summary>
    public string checksum;
    
    /// <summary>
    /// Datos del jugador.
    /// </summary>
    public PlayerData player;
    
    /// <summary>
    /// Equipo de Digimon.
    /// </summary>
    public System.Collections.Generic.List<DigimonSaveData> digimonTeam;
    
    /// <summary>
    /// Progreso en los mundos.
    /// </summary>
    public WorldData worldProgress;
    
    /// <summary>
    /// Inventario de items.
    /// </summary>
    public System.Collections.Generic.List<ItemData> inventory;
    
    /// <summary>
    /// Tiempo total jugado en segundos.
    /// </summary>
    public float totalTimePlayed;
    
    /// <summary>
    /// Fecha del último guardado.
    /// </summary>
    public DateTime lastSaveDate;
    
    public SaveData()
    {
        version = 2;
        digimonTeam = new System.Collections.Generic.List<DigimonSaveData>();
        inventory = new System.Collections.Generic.List<ItemData>();
        worldProgress = new WorldData();
        lastSaveDate = DateTime.Now;
    }
}

/// <summary>
/// Datos del jugador.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerID;
    public int level;
    public int experience;
    public int steps;
    public int money;
    public int playTime;
}

/// <summary>
/// Datos de un Digimon guardado.
/// </summary>
[System.Serializable]
public class DigimonSaveData
{
    public int digimonID;
    public string nickname;
    public int level;
    public int experience;
    public int hp;
    public int mp;
    public int attack;
    public int defense;
    public int speed;
    public int friendship;
    public System.Collections.Generic.List<int> attacks;
}

/// <summary>
/// Datos de progreso de mundo.
/// </summary>
[System.Serializable]
public class WorldData
{
    public int currentWorldID;
    public System.Collections.Generic.List<int> unlockedWorlds;
    public System.Collections.Generic.Dictionary<int, bool> eventFlags;
    
    public WorldData()
    {
        unlockedWorlds = new System.Collections.Generic.List<int>();
        eventFlags = new System.Collections.Generic.Dictionary<int, bool>();
    }
}

/// <summary>
/// Datos de un item.
/// </summary>
[System.Serializable]
public class ItemData
{
    public int itemID;
    public int quantity;
}
