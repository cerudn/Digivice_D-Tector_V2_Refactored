using System;
using System.Collections.Generic;

/// <summary>
/// Datos principales de la partida.
/// Incluye versionado para migració·´·n entre versiones.
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// Versió·´·n del schema de guardado.
    /// </summary>
    public int version = 4;
    
    /// <summary>
    /// Checksum para validació·´·n de integridad.
    /// </summary>
    public string checksum;
    
    /// <summary>
    /// Datos del jugador.
    /// </summary>
    public PlayerData player;
    
    /// <summary>
    /// Equipo de Digimon.
    /// </summary>
    public List<DigimonSaveData> digimonTeam;
    
    /// <summary>
    /// Progreso en los mundos.
    /// </summary>
    public WorldData worldProgress;
    
    /// <summary>
    /// Inventario de items.
    /// </summary>
    public List<ItemData> inventory;
    
    /// <summary>
    /// Tiempo total jugado en segundos.
    /// </summary>
    public float totalTimePlayed;
    
    /// <summary>
    /// Fecha del ú—ltimo guardado.
    /// </summary>
    public DateTime lastSaveDate;
    
    /// <summary>
    /// Flags de eventos completados.
    /// </summary>
    public Dictionary<string, bool> completedEvents;
    
    /// <summary>
    /// Estad í—sticas del jugador.
    /// </summary>
    public PlayerStats stats;
    
    public SaveData()
    {
        version = 4;
        digimonTeam = new List<DigimonSaveData>();
        inventory = new List<ItemData>();
        worldProgress = new WorldData();
        completedEvents = new Dictionary<string, bool>();
        stats = new PlayerStats();
        lastSaveDate = DateTime.Now;
    }
    
    /// <summary>
    /// Valida la integridad de los datos.
    /// </summary>
    public bool Validate()
    {
        // Verificar que el equipo no esté vacío
        if (digimonTeam == null || digimonTeam.Count == 0)
        {
            Console.Warning("[SaveData] Equipo vacío");
            return false;
        }
        
        // Verificar que al menos un Digimon tenga HP > 0
        bool hasActiveDigimon = digimonTeam.Exists(d => d.hp > 0);
        
        if (!hasActiveDigimon)
        {
            Console.Warning("[SaveData] Todos los Digimon debilitados");
            return false;
        }
        
        return true;
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
    public int currentWorldID;
    public UnityEngine.Vector2Int currentPosition;
    
    public PlayerData()
    {
        playerName = "Entrenador";
        playerID = 1;
        level = 1;
        steps = 0;
        money = 0;
        currentWorldID = WorldIDs.FILE_ISLAND;
    }
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
    public List<int> attacks;
    public List<int> equippedItems;
    public EvolutionStage stage;
    
    public DigimonSaveData()
    {
        attacks = new List<int>();
        equippedItems = new List<int>();
        friendship = 50;
        stage = EvolutionStage.Rookie;
    }
    
    /// <summary>
    /// Convierte a objeto Digimon.
    /// </summary>
    public Digimon ToDigimon()
    {
        var digimon = new Digimon(digimonID, level)
        {
            Nickname = nickname,
            Experience = experience,
            HP = hp,
            MP = mp,
            Attack = attack,
            Defense = defense,
            Speed = speed,
            Friendship = friendship,
            Stage = stage
        };
        
        // Cargar ataques
        foreach (int attackID in attacks)
        {
            var attack = Database.GetAttack(attackID);
            if (attack != null)
            {
                digimon.LearnedAttacks.Add(attack);
            }
        }
        
        return digimon;
    }
    
    /// <summary>
    /// Crea desde objeto Digimon.
    /// </summary>
    public static DigimonSaveData FromDigimon(Digimon digimon)
    {
        var data = new DigimonSaveData
        {
            digimonID = digimon.DigimonID,
            nickname = digimon.Nickname,
            level = digimon.Level,
            experience = digimon.Experience,
            hp = digimon.HP,
            mp = digimon.MP,
            attack = digimon.Attack,
            defense = digimon.Defense,
            speed = digimon.Speed,
            friendship = digimon.Friendship,
            stage = digimon.Stage,
            attacks = new List<int>(),
            equippedItems = new List<int>()
        };
        
        // Guardar IDs de ataques
        foreach (var attack in digimon.LearnedAttacks)
        {
            data.attacks.Add(attack.ID);
        }
        
        return data;
    }
}

/// <summary>
/// Datos de progreso de mundo.
/// </summary>
[System.Serializable]
public class WorldData
{
    public int currentWorldID;
    public List<int> unlockedWorlds;
    public Dictionary<int, bool> eventFlags;
    public Dictionary<int, float> completionPercentage;
    
    public WorldData()
    {
        unlockedWorlds = new List<int> { WorldIDs.FILE_ISLAND };
        eventFlags = new Dictionary<int, bool>();
        completionPercentage = new Dictionary<int, float>();
        currentWorldID = WorldIDs.FILE_ISLAND;
    }
    
    /// <summary>
    /// Marca un evento como completado.
    /// </summary>
    public void SetEventFlag(int eventID, bool completed)
    {
        eventFlags[eventID] = completed;
    }
    
    /// <summary>
    /// Verifica si un evento está completado.
    /// </summary>
    public bool IsEventCompleted(int eventID)
    {
        return eventFlags.ContainsKey(eventID) && eventFlags[eventID];
    }
    
    /// <summary>
    /// Desbloquea un mundo.
    /// </summary>
    public void UnlockWorld(int worldID)
    {
        if (!unlockedWorlds.Contains(worldID))
        {
            unlockedWorlds.Add(worldID);
        }
    }
    
    /// <summary>
    /// Verifica si un mundo está desbloqueado.
    /// </summary>
    public bool IsWorldUnlocked(int worldID)
    {
        return unlockedWorlds.Contains(worldID);
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
    
    public ItemData()
    {
        quantity = 1;
    }
    
    public ItemData(int id, int qty = 1)
    {
        itemID = id;
        quantity = qty;
    }
}

/// <summary>
/// Estad í—sticas del jugador.
/// </summary>
[System.Serializable]
public class PlayerStats
{
    public int totalBattles;
    public int battlesWon;
    public int battlesLost;
    public int totalSteps;
    public int digimonRaised;
    public int evolutionsPerformed;
    public float totalPlayTime;
    public DateTime firstSaveDate;
    
    public PlayerStats()
    {
        firstSaveDate = DateTime.Now;
    }
    
    /// <summary>
    /// Porcentaje de victorias.
    /// </summary>
    public float WinRate => totalBattles > 0 ? (float)battlesWon / totalBattles : 0f;
    
    /// <summary>
    /// Racha actual de victorias (placeholder).
    /// </summary>
    public int WinStreak => 0; // Implementar tracking
}
