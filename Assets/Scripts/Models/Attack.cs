using System;

/// <summary>
/// Modelo de datos de un ataque.
/// </summary>
[System.Serializable]
public class Attack
{
    /// <summary>
    /// ID ú—nico del ataque.
    /// </summary>
    public int ID;
    
    /// <summary>
    /// Nombre del ataque.
    /// </summary>
    public string Name;
    
    /// <summary>
    /// Descripció·´·n del efecto.
    /// </summary>
    public string Description;
    
    /// <summary>
    /// Poder base del ataque.
    /// </summary>
    public int Power;
    
    /// <summary>
    /// Precisió·´·n (0-100).
    /// </summary>
    public int Accuracy;
    
    /// <summary>
    /// Coste de MP.
    /// </summary>
    public int MPCost;
    
    /// <summary>
    /// Tipo elemental.
    /// </summary>
    public ElementType Element;
    
    /// <summary>
    /// Tipo de ataque (fí·´­sico, especial, estado).
    /// </summary>
    public AttackType Type;
    
    /// <summary>
    /// Prioridad (-5 a +5).
    /// </summary>
    public int Priority;
    
    /// <summary>
    /// Probabilidad de efectos secundarios (0-1).
    /// </summary>
    public float SecondaryEffectChance;
    
    /// <summary>
    /// Efecto secundario (si aplica).
    /// </summary>
    public AttackEffect SecondaryEffect;
    
    /// <summary>
    /// Número de golpes (para ataques multi-hit).
    /// </summary>
    public int HitCount;
    
    /// <summary>
    /// Indica si es un ataque de estado.
    /// </summary>
    public bool IsStatusAttack => Type == AttackType.STATUS;
    
    /// <summary>
    /// Indica si es un ataque fí·´­sico.
    /// </summary>
    public bool IsPhysical => Type == AttackType.PHYSICAL;
    
    /// <summary>
    /// Indica si es un ataque especial.
    /// </summary>
    public bool IsSpecial => Type == AttackType.SPECIAL;
}

/// <summary>
/// Tipos de ataque.
/// </summary>
public enum AttackType
{
    PHYSICAL = 0,
    SPECIAL = 1,
    STATUS = 2
}

/// <summary>
/// Efectos secundarios posibles.
/// </summary>
public enum AttackEffect
{
    NONE = 0,
    BURN = 1,
    PARALYZE = 2,
    POISON = 3,
    FREEZE = 4,
    SLEEP = 5,
    CONFUSE = 6,
    STAT_UP_ATTACK = 7,
    STAT_DOWN_DEFENSE = 8,
    STAT_UP_SPEED = 9,
    STAT_DOWN_SPEED = 10
}
