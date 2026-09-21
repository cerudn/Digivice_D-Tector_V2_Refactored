using UnityEngine;

/// <summary>
/// ScriptableObject para datos de un Digimon.
/// Reemplaza el Database.cs hardcodeado.
/// </summary>
[CreateAssetMenu(fileName = "New Digimon", menuName = "Digimon/Create Digimon", order = 1)]
public class DigimonData : ScriptableObject
{
    #region Basic Info
    [Header("Informació·´·n B á—sica")]
    [Tooltip("ID ú—nico del Digimon (usar DigimonIDs constants)")]
    public int digimonID;
    
    [Tooltip("Nombre base del Digimon")]
    public string digimonName;
    
    [Tooltip("Descripció·´·n del Digimon")]
    [TextArea(3, 5)]
    public string description;
    
    [Tooltip("Etapa de evolució·´·n actual")]
    public EvolutionStage stage;
    
    [Tooltip("Tipo elemental")]
    public ElementType element;
    #endregion
    
    #region Base Stats
    [Header("Stats Base (Nivel 1)")]
    [Tooltip("HP base")]
    [Range(10, 200)]
    public int baseHP = 50;
    
    [Tooltip("MP base")]
    [Range(5, 100)]
    public int baseMP = 20;
    
    [Tooltip("Ataque base")]
    [Range(20, 150)]
    public int baseAttack = 60;
    
    [Tooltip("Defensa base")]
    [Range(20, 150)]
    public int baseDefense = 40;
    
    [Tooltip("Velocidad base")]
    [Range(20, 150)]
    public int baseSpeed = 50;
    #endregion
    
    #region Growth Rates
    [Header("Tasas de Crecimiento")]
    [Tooltip("Multiplicador de crecimiento de HP")]
    [Range(1.0f, 3.0f)]
    public float hpGrowthRate = 2.0f;
    
    [Tooltip("Multiplicador de crecimiento de MP")]
    [Range(0.5f, 2.0f)]
    public float mpGrowthRate = 1.0f;
    
    [Tooltip("Multiplicador de crecimiento de Attack")]
    [Range(0.5f, 2.0f)]
    public float attackGrowthRate = 1.0f;
    
    [Tooltip("Multiplicador de crecimiento de Defense")]
    [Range(0.5f, 2.0f)]
    public float defenseGrowthRate = 1.0f;
    
    [Tooltip("Multiplicador de crecimiento de Speed")]
    [Range(0.5f, 2.0f)]
    public float speedGrowthRate = 1.0f;
    #endregion
    
    #region Sprites
    [Header("Sprites")]
    [Tooltip("Sprite en reposo")]
    public Sprite idleSprite;
    
    [Tooltip("Sprite caminando")]
    public Sprite walkSprite;
    
    [Tooltip("Sprite atacando")]
    public Sprite attackSprite;
    
    [Tooltip("Sprite de victoria")]
    public Sprite victorySprite;
    
    [Tooltip("Sprite debilitado")]
    public Sprite faintSprite;
    
    [Tooltip("Array de frames de animació·´·n")]
    public Sprite[] animationFrames;
    #endregion
    
    #region Attacks
    [Header("Ataques Aprendibles")]
    [Tooltip("Lista de ataques que puede aprender por nivel")]
    public LearnableAttackData[] learnableAttacks;
    
    [Tooltip("Ataques iniciales (nivel 1)")]
    public AttackData[] startingAttacks;
    #endregion
    
    #region Evolution
    [Header("Evoluciones")]
    [Tooltip("Lista de evoluciones posibles desde este Digimon")]
    public EvolutionRequirement[] possibleEvolutions;
    
    [Tooltip("Digimon del que evoluciona (pre-evolució·´·n)")]
    public DigimonData preEvolution;
    #endregion
    
    #region Metadata
    [Header("Metadata")]
    [Tooltip("Altura en metros")]
    public float height;
    
    [Tooltip("Peso en kilogramos")]
    public float weight;
    
    [Tooltip("Familia (ej: Dragon, Beast, etc.)")]
    public string family;
    
    [Tooltip("A ñ—o de primera aparició·´·n")]
    public int firstAppearanceYear;
    #endregion
    
    #region Methods
    /// <summary>
    /// Calcula los stats para un nivel dado.
    /// </summary>
    public DigimonStats GetStatsAtLevel(int level)
    {
        var stats = new DigimonStats
        {
            HP = CalculateStat(baseHP, level, hpGrowthRate),
            MP = CalculateStat(baseMP, level, mpGrowthRate),
            Attack = CalculateStat(baseAttack, level, attackGrowthRate),
            Defense = CalculateStat(baseDefense, level, defenseGrowthRate),
            Speed = CalculateStat(baseSpeed, level, speedGrowthRate)
        };
        
        return stats;
    }
    
    /// <summary>
    /// Fó—rmula de cálculo de stat.
    /// </summary>
    private int CalculateStat(int baseStat, int level, float growthMultiplier)
    {
        // Fó—rmula tipo Pokémon simplificada
        return Mathf.FloorToInt((baseStat * 2 * level / 100f + 5) * growthMultiplier);
    }
    
    /// <summary>
    /// Obtiene los ataques aprendibles en un nivel.
    /// </summary>
    public AttackData[] GetAttacksForLevel(int level)
    {
        if (learnableAttacks == null) return new AttackData[0];
        
        var attacks = new System.Collections.Generic.List<AttackData>();
        
        foreach (var learnable in learnableAttacks)
        {
            if (learnable.level == level && learnable.attack != null)
            {
                attacks.Add(learnable.attack);
            }
        }
        
        return attacks.ToArray();
    }
    
    /// <summary>
    /// Verifica si puede evolucionar.
    /// </summary>
    public bool CanEvolve(Digimon digimon)
    {
        if (possibleEvolutions == null || possibleEvolutions.Length == 0)
        {
            return false;
        }
        
        foreach (var evolution in possibleEvolutions)
        {
            if (evolution.MeetsRequirements(digimon))
            {
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// Obtiene la evolució·´·n disponible.
    /// </summary>
    public DigimonData GetEvolution(Digimon digimon)
    {
        if (possibleEvolutions == null) return null;
        
        foreach (var evolution in possibleEvolutions)
        {
            if (evolution.MeetsRequirements(digimon))
            {
                return evolution.targetDigimon;
            }
        }
        
        return null;
    }
    #endregion
}

/// <summary>
/// Stats calculados para un nivel espec í—fico.
/// </summary>
[System.Serializable]
public class DigimonStats
{
    public int HP;
    public int MP;
    public int Attack;
    public int Defense;
    public int Speed;
}

/// <summary>
/// Ataque aprendible por nivel.
/// </summary>
[System.Serializable]
public class LearnableAttackData
{
    [Tooltip("Nivel en que aprende el ataque")]
    public int level;
    
    [Tooltip("Referencia al ScriptableObject del ataque")]
    public AttackData attack;
}
