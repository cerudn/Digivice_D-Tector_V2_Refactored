using UnityEngine;

/// <summary>
/// ScriptableObject para datos de un ataque.
/// </summary>
[CreateAssetMenu(fileName = "New Attack", menuName = "Digimon/Create Attack", order = 2)]
public class AttackData : ScriptableObject
{
    #region Basic Info
    [Header("Informació·´·n B á—sica")]
    [Tooltip("ID ú—nico del ataque")]
    public int attackID;
    
    [Tooltip("Nombre del ataque")]
    public string attackName;
    
    [Tooltip("Descripció·´·n del efecto")]
    [TextArea(2, 4)]
    public string description;
    #endregion
    
    #region Combat Stats
    [Header("Stats de Combate")]
    [Tooltip("Poder base del ataque")]
    [Range(10, 150)]
    public int power = 40;
    
    [Tooltip("Precisió·´·n (0-100)")]
    [Range(50, 100)]
    public int accuracy = 100;
    
    [Tooltip("Coste de MP")]
    [Range(0, 50)]
    public int mpCost = 0;
    
    [Tooltip("Prioridad (-5 a +5)")]
    [Range(-5, 5)]
    public int priority = 0;
    #endregion
    
    #region Type
    [Header("Tipo")]
    [Tooltip("Tipo elemental del ataque")]
    public ElementType element;
    
    [Tooltip("Tipo de ataque (f í—sico, especial, estado)")]
    public AttackType attackType;
    #endregion
    
    #region Effects
    [Header("Efectos")]
    [Tooltip("Tipo de efecto secundario")]
    public AttackEffect secondaryEffect;
    
    [Tooltip("Probabilidad de efecto secundario (0-1)")]
    [Range(0f, 1f)]
    public float secondaryEffectChance = 0f;
    
    [Tooltip("Valor del efecto (ej: stat a subir/bajar)")]
    public int effectValue;
    #endregion
    
    #region Animation
    [Header("Animació·´·n")]
    [Tooltip("Sprite del ataque (para UI)")]
    public Sprite attackSprite;
    
    [Tooltip("Prefabs de part í—culas")]
    public GameObject[] particlePrefabs;
    
    [Tooltip("Sonido del ataque")]
    public int soundID;
    #endregion
    
    #region Metadata
    [Header("Metadata")]
    [Tooltip("Generació·´·n en que se introdujo")]
    public int generation;
    
    [Tooltip("Es atacable por Mirror Coat")]
    public bool isReflectable => attackType == AttackType.SPECIAL;
    
    [Tooltip("Es bloqueable por Magic Coat")]
    public bool isBounceable => attackType == AttackType.STATUS;
    
    [Tooltip("Es robable por Snatch")]
    public bool isSnatchable => attackType == AttackType.STATUS;
    #endregion
    
    #region Methods
    /// <summary>
    /// Verifica si el ataque es de estado.
    /// </summary>
    public bool IsStatusAttack() => attackType == AttackType.STATUS;
    
    /// <summary>
    /// Verifica si el ataque es f í—sico.
    /// </summary>
    public bool IsPhysical() => attackType == AttackType.PHYSICAL;
    
    /// <summary>
    /// Verifica si el ataque es especial.
    /// </summary>
    public bool IsSpecial() => attackType == AttackType.SPECIAL;
    
    /// <summary>
    /// Calcula el daño base.
    /// </summary>
    public int CalculateBaseDamage(int attackerAttack, int defenderDefense)
    {
        if (IsStatusAttack())
        {
            return 0; // Ataques de estado no hacen daño
        }
        
        // Fó—rmula simplificada
        float baseDamage = (float)power * ((float)attackerAttack / defenderDefense);
        
        return Mathf.RoundToInt(baseDamage);
    }
    
    /// <summary>
    /// Verifica si es súper efectivo contra un tipo.
    /// </summary>
    public float GetTypeEffectiveness(ElementType defenderElement)
    {
        // Implementar tabla de tipos
        // Placeholder - siempre neutral
        return 1f;
    }
    #endregion
}
