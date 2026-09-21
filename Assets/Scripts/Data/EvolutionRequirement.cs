using UnityEngine;
using System;

/// <summary>
/// Requisito de evolució·´·n para Digimon.
/// Usado en DigimonData.possibleEvolutions.
/// </summary>
[System.Serializable]
public class EvolutionRequirement
{
    [Header("Objetivo")]
    [Tooltip("Digimon al que evoluciona")]
    public DigimonData targetDigimon;
    
    [Header("Requisitos B á—sicos")]
    [Tooltip("Nivel mí—nimo requerido")]
    public int minLevel = 1;
    
    [Tooltip("Friendship mí—nimo requerido (0-100)")]
    [Range(0, 100)]
    public int minFriendship = 0;
    
    [Header("Items Requeridos")]
    [Tooltip("Items necesarios para evolucionar")]
    public int[] requiredItemIDs;
    
    [Tooltip("Consumir items al evolucionar")]
    public bool consumeItems = true;
    
    [Header("Condiciones Especiales")]
    [Tooltip("Tipo de condició·´·n especial")]
    public EvolutionCondition specialCondition;
    
    [Tooltip("Valor requerido para la condició·´·n")]
    public int conditionValue;
    
    [Tooltip("Descripció·´·n de la condició·´·n especial")]
    [TextArea(2, 4)]
    public string conditionDescription;
    
    [Header("Probabilidad")]
    [Tooltip("Probabilidad de é—xito (0-1)")]
    [Range(0f, 1f)]
    public float successRate = 1f;
    
    [Header("M é—todo de Evolució·´·n")]
    [Tooltip("M é—todo necesario (nivel, item, friendship, etc.)")]
    public EvolutionMethod evolutionMethod;
    
    /// <summary>
    /// Verifica si un Digimon cumple los requisitos.
    /// </summary>
    public bool MeetsRequirements(Digimon digimon)
    {
        // Verificar nivel
        if (digimon.Level < minLevel)
        {
            return false;
        }
        
        // Verificar friendship
        if (digimon.Friendship < minFriendship)
        {
            return false;
        }
        
        // Verificar items
        if (requiredItemIDs != null && requiredItemIDs.Length > 0)
        {
            // TODO: Verificar inventario del jugador
            // Placeholder - asumir que tiene los items
        }
        
        // Verificar condició·´·n especial
        if (specialCondition != EvolutionCondition.NONE)
        {
            if (!MeetsSpecialCondition(digimon))
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Verifica si cumple la condició·´·n especial.
    /// </summary>
    private bool MeetsSpecialCondition(Digimon digimon)
    {
        switch (specialCondition)
        {
            case EvolutionCondition.NONE:
                return true;
                
            case EvolutionCondition.TIME_OF_DAY:
                // conditionValue: 0 = Any, 1 = Day, 2 = Night
                int currentHour = DateTime.Now.Hour;
                bool isDay = currentHour >= 6 && currentHour < 18;
                
                if (conditionValue == 1) // Day
                    return isDay;
                else if (conditionValue == 2) // Night
                    return !isDay;
                return true;
                
            case EvolutionCondition.BATTLES_WON:
                // Verificar si gan ó— suficientes batallas
                // TODO: Implementar tracking de batallas
                return true;
                
            case EvolutionCondition.STEPS_WALKED:
                // Verificar si camin ó— suficientes pasos
                // conditionValue = pasos requeridos
                // TODO: Verificar pedometer
                return true;
                
            case EvolutionCondition.SPIRIT_TYPE:
                // Verificar tipo de esp í—ritu
                // conditionValue = tipo requerido
                // TODO: Verificar esp í—ritus del jugador
                return true;
                
            case EvolutionCondition.PARTNER_DIGIMON:
                // Verificar si tiene Digimon espec í—fico en equipo
                // conditionValue = ID del Digimon requerido
                // TODO: Verificar equipo
                return true;
                
            default:
                return true;
        }
    }
    
    /// <summary>
    /// Obtiene la descripció·´·n de los requisitos.
    /// </summary>
    public string GetRequirementsDescription()
    {
        var sb = new System.Text.StringBuilder();
        
        sb.Append($"Nivel {minLevel}");
        
        if (minFriendship > 0)
        {
            sb.Append($", Friendship {minFriendship}");
        }
        
        if (specialCondition != EvolutionCondition.NONE && !string.IsNullOrEmpty(conditionDescription))
        {
            sb.Append($", {conditionDescription}");
        }
        
        return sb.ToString();
    }
}

/// <summary>
/// Condiciones especiales de evolució·´·n.
/// </summary>
public enum EvolutionCondition
{
    NONE = 0,
    TIME_OF_DAY = 1,
    BATTLES_WON = 2,
    STEPS_WALKED = 3,
    SPIRIT_TYPE = 4,
    PARTNER_DIGIMON = 5,
    SPECIFIC_ITEM = 6,
    HIGH_CRIT_RATE = 7,
    LOW_HP = 8,
    STAT_CONDITION = 9
}

/// <summary>
/// M é—todos de evolució·´·n.
/// </summary>
public enum EvolutionMethod
{
    LEVEL = 0,
    ITEM = 1,
    FRIENDSHIP = 2,
    SPIRIT_EVOLUTION = 3,
    DOUBLE_SPIRIT = 4,
    DNA_DIGIVOLVE = 5,
    MODE_CHANGE = 6,
    SPECIAL = 7
}
