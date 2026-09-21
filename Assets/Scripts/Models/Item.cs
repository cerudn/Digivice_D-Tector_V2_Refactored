using System;

/// <summary>
/// Modelo de datos de un item.
/// </summary>
[System.Serializable]
public class Item
{
    /// <summary>
    /// ID ú—nico del item.
    /// </summary>
    public int ID;
    
    /// <summary>
    /// Nombre del item.
    /// </summary>
    public string Name;
    
    /// <summary>
    /// Descripció·´·n del efecto.
    /// </summary>
    public string Description;
    
    /// <summary>
    /// Tipo de item.
    /// </summary>
    public ItemType Type;
    
    /// <summary>
    /// Precio de compra.
    /// </summary>
    public int BuyPrice;
    
    /// <summary>
    /// Precio de venta.
    /// </summary>
    public int SellPrice;
    
    /// <summary>
    /// Efecto principal (si es consumible).
    /// </summary>
    public ItemEffect Effect;
    
    /// <summary>
    /// Valor del efecto.
    /// </summary>
    public int EffectValue;
    
    /// <summary>
    /// Indica si es consumible.
    /// </summary>
    public bool IsConsumable => Type == ItemType.CONSUMABLE;
    
    /// <summary>
    /// Indica si es un item clave (no consumible, ú—nico).
    /// </summary>
    public bool IsKeyItem => Type == ItemType.KEY_ITEM;
    
    /// <summary>
    /// Indica si es equipable.
    /// </summary>
    public bool IsEquipable => Type == ItemType.EQUIPMENT;
}

/// <summary>
/// Tipos de items.
/// </summary>
public enum ItemType
{
    CONSUMABLE = 0,
    EQUIPMENT = 1,
    KEY_ITEM = 2,
    MATERIAL = 3
}

/// <summary>
/// Efectos de items consumibles.
/// </summary>
public enum ItemEffect
{
    NONE = 0,
    HEAL_HP = 1,
    HEAL_MP = 2,
    HEAL_STATUS = 3,
    REVIVE = 4,
    EVOLUTION_ITEM = 5,
    STAT_BOOST = 6,
    ESCAPE = 7
}
