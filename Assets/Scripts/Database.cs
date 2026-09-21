using System;
using System.Collections.Generic;

/// <summary>
/// Base de datos estática de referencia rá—pida.
/// TODO: Migrar a ScriptableObjects en Fase 1B.3
/// </summary>
public static class Database
{
    #region Digimon Data
    private static Dictionary<int, DigimonBaseStats> digimonStatsCache;
    private static Dictionary<int, List<LearnableAttack>> digimonAttacksCache;
    
    /// <summary>
    /// Obtiene los stats base de un Digimon.
    /// </summary>
    public static DigimonBaseStats GetDigimonBaseStats(int digimonID)
    {
        if (digimonStatsCache == null)
        {
            InitializeDigimonStats();
        }
        
        if (digimonStatsCache.TryGetValue(digimonID, out var stats))
        {
            return stats;
        }
        
        Console.WriteLine($"[Database] Warning: Stats not found for Digimon ID {digimonID}");
        return DigimonBaseStats.Default;
    }
    
    /// <summary>
    /// Obtiene la lista de ataques aprendibles de un Digimon.
    /// </summary>
    public static List<LearnableAttack> GetDigimonLearnableAttacks(int digimonID)
    {
        if (digimonAttacksCache == null)
        {
            InitializeDigimonAttacks();
        }
        
        if (digimonAttacksCache.TryGetValue(digimonID, out var attacks))
        {
            return attacks;
        }
        
        return new List<LearnableAttack>();
    }
    
    private static void InitializeDigimonStats()
    {
        digimonStatsCache = new Dictionary<int, DigimonBaseStats>();
        
        // TODO: Cargar desde ScriptableObjects
        // Placeholder - datos de ejemplo
        digimonStatsCache[DigimonIDs.AGUMON] = new DigimonBaseStats
        {
            HP = 50,
            MP = 20,
            Attack = 60,
            Defense = 40,
            Speed = 50
        };
        
        digimonStatsCache[DigimonIDs.GABUMON] = new DigimonBaseStats
        {
            HP = 45,
            MP = 25,
            Attack = 55,
            Defense = 45,
            Speed = 55
        };
        
        Console.WriteLine($"[Database] Initialized {digimonStatsCache.Count} Digimon stats");
    }
    
    private static void InitializeDigimonAttacks()
    {
        digimonAttacksCache = new Dictionary<int, List<LearnableAttack>>();
        
        // TODO: Cargar desde ScriptableObjects
        digimonAttacksCache[DigimonIDs.AGUMON] = new List<LearnableAttack>
        {
            new LearnableAttack { Level = 1, AttackID = 1 }, // Scratch
            new LearnableAttack { Level = 5, AttackID = 2 }, // Pepper Breath
        };
        
        Console.WriteLine($"[Database] Initialized {digimonAttacksCache.Count} Digimon attack lists");
    }
    #endregion
    
    #region Attack Data
    private static Dictionary<int, Attack> attacksCache;
    
    /// <summary>
    /// Obtiene un ataque por ID.
    /// </summary>
    public static Attack GetAttack(int attackID)
    {
        if (attacksCache == null)
        {
            InitializeAttacks();
        }
        
        if (attacksCache.TryGetValue(attackID, out var attack))
        {
            return attack;
        }
        
        return null;
    }
    
    private static void InitializeAttacks()
    {
        attacksCache = new Dictionary<int, Attack>();
        
        // TODO: Cargar desde ScriptableObjects
        attacksCache[1] = new Attack
        {
            ID = 1,
            Name = "Scratch",
            Power = 40,
            Accuracy = 100,
            MPCost = 0,
            Element = ElementType.None,
            Type = AttackType.PHYSICAL
        };
        
        attacksCache[2] = new Attack
        {
            ID = 2,
            Name = "Pepper Breath",
            Power = 60,
            Accuracy = 95,
            MPCost = 5,
            Element = ElementType.Fire,
            Type = AttackType.SPECIAL
        };
        
        Console.WriteLine($"[Database] Initialized {attacksCache.Count} attacks");
    }
    #endregion
    
    #region Evolution Data
    /// <summary>
    /// Verifica si un Digimon puede evolucionar.
    /// </summary>
    public static bool CanEvolve(Digimon digimon)
    {
        // Verificar nivel mí—nimo
        int minLevel = GetMinLevelForStage(digimon.Stage + 1);
        
        if (digimon.Level < minLevel)
        {
            return false;
        }
        
        // Verificar friendship
        if (digimon.Friendship < GameConstants.MIN_FRIENDSHIP_EVOLUTION)
        {
            return false;
        }
        
        // Verificar si existe evolució·´·n disponible
        var evolutions = GetEvolutionsForDigimon(digimon.DigimonID);
        
        return evolutions.Count > 0;
    }
    
    private static int GetMinLevelForStage(EvolutionStage stage)
    {
        switch (stage)
        {
            case EvolutionStage.Champion:
                return GameConstants.MIN_LEVEL_CHAMPION;
            case EvolutionStage.Ultimate:
                return GameConstants.MIN_LEVEL_ULTIMATE;
            case EvolutionStage.Mega:
                return GameConstants.MIN_LEVEL_MEGA;
            default:
                return 1;
        }
    }
    
    private static List<EvolutionPath> GetEvolutionsForDigimon(int digimonID)
    {
        // TODO: Implementar con datos reales
        return new List<EvolutionPath>();
    }
    #endregion
}

/// <summary>
/// Stats base de un Digimon.
/// </summary>
[System.Serializable]
public class DigimonBaseStats
{
    public int HP;
    public int MP;
    public int Attack;
    public int Defense;
    public int Speed;
    
    public static DigimonBaseStats Default => new DigimonBaseStats
    {
        HP = 50,
        MP = 20,
        Attack = 50,
        Defense = 40,
        Speed = 50
    };
}

/// <summary>
/// Ataque aprendible por nivel.
/// </summary>
[System.Serializable]
public class LearnableAttack
{
    public int Level;
    public int AttackID;
}

/// <summary>
/// Ruta de evolució·´·n.
/// </summary>
[System.Serializable]
public class EvolutionPath
{
    public int TargetDigimonID;
    public int MinLevel;
    public int MinFriendship;
    public List<int> RequiredItems;
    public string SpecialCondition;
}
