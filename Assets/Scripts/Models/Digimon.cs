using System;
using System.Collections.Generic;

/// <summary>
/// Modelo de datos de un Digimon.
/// Representa un Digimon en memoria durante el juego.
/// </summary>
[System.Serializable]
public class Digimon
{
    #region Basic Info
    /// <summary>
    /// ID ú—nico del Digimon (desde DigimonIDs).
    /// </summary>
    public int DigimonID;
    
    /// <summary>
    /// Nombre personalizado (puede ser null).
    /// </summary>
    public string Nickname;
    
    /// <summary>
    /// Nombre base del Digimon.
    /// </summary>
    public string BaseName;
    
    /// <summary>
    /// Etapa de evolució·´·n actual.
    /// </summary>
    public EvolutionStage Stage;
    
    /// <summary>
    /// Tipo elemental.
    /// </summary>
    public ElementType Element;
    #endregion
    
    #region Stats
    /// <summary>
    /// Nivel actual (1-99).
    /// </summary>
    public int Level;
    
    /// <summary>
    /// Experiencia actual.
    /// </summary>
    public int Experience;
    
    /// <summary>
    /// Experiencia necesaria para el siguiente nivel.
    /// </summary>
    public int ExperienceToNextLevel;
    
    /// <summary>
    /// Puntos de vida actuales.
    /// </summary>
    public int HP;
    
    /// <summary>
    /// Má—ximo de HP.
    /// </summary>
    public int MaxHP;
    
    /// <summary>
    /// Puntos de mana actuales.
    /// </summary>
    public int MP;
    
    /// <summary>
    /// Má—ximo de MP.
    /// </summary>
    public int MaxMP;
    
    /// <summary>
    /// Stat de ataque.
    /// </summary>
    public int Attack;
    
    /// <summary>
    /// Stat de defensa.
    /// </summary>
    public int Defense;
    
    /// <summary>
    /// Stat de velocidad.
    /// </summary>
    public int Speed;
    #endregion
    
    #region Progression
    /// <summary>
    /// Nivel de amistad (0-100).
    /// </summary>
    public int Friendship;
    
    /// <summary>
    /// Ataques aprendidos.
    /// </summary>
    public List<Attack> LearnedAttacks;
    
    /// <summary>
    /// Items equipados.
    /// </summary>
    public List<Item> EquippedItems;
    #endregion
    
    #region State
    /// <summary>
    /// Indica si está debilitado.
    /// </summary>
    public bool IsFainted => HP <= 0;
    
    /// <summary>
    /// Indica si puede evolucionar.
    /// </summary>
    public bool CanEvolve => EvolutionManager.CanEvolve(this);
    
    /// <summary>
    /// Porcentaje de HP actual (0-1).
    /// </summary>
    public float HPPercentage => (float)HP / MaxHP;
    
    /// <summary>
    /// Porcentaje de MP actual (0-1).
    /// </summary>
    public float MPPercentage => (float)MP / MaxMP;
    #endregion
    
    #region Constructor
    public Digimon()
    {
        LearnedAttacks = new List<Attack>();
        EquippedItems = new List<Item>();
    }
    
    public Digimon(int digimonID, int level = 1)
    {
        DigimonID = digimonID;
        Level = level;
        LearnedAttacks = new List<Attack>();
        EquippedItems = new List<Item>();
        
        InitializeStats();
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Inicializa los stats base segó·´·n nivel y especie.
    /// </summary>
    private void InitializeStats()
    {
        // Obtener stats base desde Database o ScriptableObject
        var baseStats = Database.GetDigimonBaseStats(DigimonID);
        
        // Calcular stats segó·´·n fó—rmula de nivel
        MaxHP = CalculateStat(baseStats.HP, Level, 2);
        MaxMP = CalculateStat(baseStats.MP, Level, 1);
        Attack = CalculateStat(baseStats.Attack, Level, 1);
        Defense = CalculateStat(baseStats.Defense, Level, 1);
        Speed = CalculateStat(baseStats.Speed, Level, 1);
        
        HP = MaxHP;
        MP = MaxMP;
        
        Experience = CalculateExperienceToLevel(Level);
        ExperienceToNextLevel = CalculateExperienceToLevel(Level + 1) - Experience;
    }
    
    /// <summary>
    /// Fó—rmula de cálculo de stat.
    /// </summary>
    private int CalculateStat(int baseStat, int level, float growthMultiplier)
    {
        // Fó—rmula simplificada tipo Pokémon
        return Mathf.FloorToInt((baseStat * 2 * level / 100f + 5) * growthMultiplier);
    }
    
    /// <summary>
    /// Calcula la experiencia total para alcanzar un nivel.
    /// </summary>
    private int CalculateExperienceToLevel(int level)
    {
        // Curva de experiencia có·´·bica (ajustar segó·´·n balance)
        return Mathf.FloorToInt(level * level * level);
    }
    
    /// <summary>
    /// Añ·´·ade experiencia y verifica subidas de nivel.
    /// </summary>
    public bool AddExperience(int exp)
    {
        Experience += exp;
        
        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Sube el nivel y actualiza stats.
    /// </summary>
    private void LevelUp()
    {
        Level++;
        
        // Recalcular stats
        int newMaxHP = CalculateStat(Database.GetDigimonBaseStats(DigimonID).HP, Level, 2);
        int newMaxMP = CalculateStat(Database.GetDigimonBaseStats(DigimonID).MP, Level, 1);
        
        // Ajustar HP/MP actuales proporcionalmente
        HP += (newMaxHP - MaxHP);
        MP += (newMaxMP - MaxMP);
        
        MaxHP = newMaxHP;
        MaxMP = newMaxMP;
        
        Attack = CalculateStat(Database.GetDigimonBaseStats(DigimonID).Attack, Level, 1);
        Defense = CalculateStat(Database.GetDigimonBaseStats(DigimonID).Defense, Level, 1);
        Speed = CalculateStat(Database.GetDigimonBaseStats(DigimonID).Speed, Level, 1);
        
        ExperienceToNextLevel = CalculateExperienceToLevel(Level + 1) - CalculateExperienceToLevel(Level);
        
        // Disparar evento
        GameEvents.TriggerLevelUp(Level);
        
        // Verificar nuevos ataques
        CheckLearnableAttacks();
    }
    
    /// <summary>
    /// Verifica y aprende nuevos ataques.
    /// </summary>
    private void CheckLearnableAttacks()
    {
        var learnableAttacks = Database.GetDigimonLearnableAttacks(DigimonID);
        
        foreach (var attackData in learnableAttacks)
        {
            if (attackData.Level == Level && !LearnedAttacks.Exists(a => a.ID == attackData.AttackID))
            {
                LearnAttack(attackData.AttackID);
            }
        }
    }
    
    /// <summary>
    /// Aprende un nuevo ataque.
    /// </summary>
    public void LearnAttack(int attackID)
    {
        var attack = Database.GetAttack(attackID);
        
        if (attack != null && LearnedAttacks.Count < 4)
        {
            LearnedAttacks.Add(attack);
            Console.WriteLine($"[{BaseName}] aprendió·´· {attack.Name}!");
        }
    }
    
    /// <summary>
    /// Recupera HP.
    /// </summary>
    public void Heal(int amount)
    {
        HP = Math.Min(MaxHP, HP + amount);
    }
    
    /// <summary>
    /// Recupera MP.
    /// </summary>
    public void RestoreMP(int amount)
    {
        MP = Math.Min(MaxMP, MP + amount);
    }
    
    /// <summary>
    /// Recupera completamente.
    /// </summary>
    public void FullyHeal()
    {
        HP = MaxHP;
        MP = MaxMP;
    }
    #endregion
}

/// <summary>
/// Etapas de evolució·´·n.
/// </summary>
public enum EvolutionStage
{
    Fresh = 0,
    InTraining = 1,
    Rookie = 2,
    Champion = 3,
    Ultimate = 4,
    Mega = 5,
    Spirit = 6
}

/// <summary>
/// Tipos elementales.
/// </summary>
public enum ElementType
{
    None = 0,
    Fire = 1,
    Water = 2,
    Wind = 3,
    Earth = 4,
    Thunder = 5,
    Light = 6,
    Dark = 7,
    Steel = 8,
    Nature = 9
}
