using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Database refactorizado para usar ScriptableObjects.
/// Reemplaza el Database.cs estático hardcodeado.
/// </summary>
public class DatabaseSO : MonoBehaviour
{
    #region Singleton
    private static DatabaseSO _instance;
    
    public static DatabaseSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DatabaseSO>();
                
                if (_instance == null)
                {
                    Debug.LogError("[DatabaseSO] No se encontró DatabaseSO en la escena!");
                }
            }
            
            return _instance;
        }
    }
    #endregion
    
    #region Data Arrays
    [Header("Digimon Data")]
    [Tooltip("Array de todos los DigimonData disponibles")]
    public DigimonData[] allDigimon;
    
    [Header("Attack Data")]
    [Tooltip("Array de todos los AttackData disponibles")]
    public AttackData[] allAttacks;
    
    [Header("Item Data")]
    [Tooltip("Array de todos los ItemData disponibles")]
    public ItemDataSO[] allItems;
    #endregion
    
    #region Caches
    private Dictionary<int, DigimonData> digimonCache;
    private Dictionary<int, AttackData> attackCache;
    private Dictionary<int, ItemDataSO> itemCache;
    
    private bool isInitialized;
    #endregion
    
    #region Unity Lifecycle
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        Initialize();
    }
    #endregion
    
    #region Initialization
    private void Initialize()
    {
        if (isInitialized) return;
        
        Console.WriteLine("[DatabaseSO] Inicializando caches...");
        
        // Construir diccionarios para b ú—squeda O(1)
        digimonCache = new Dictionary<int, DigimonData>();
        attackCache = new Dictionary<int, AttackData>();
        itemCache = new Dictionary<int, ItemDataSO>();
        
        // Cargar Digimon
        if (allDigimon != null)
        {
            foreach (var digimon in allDigimon)
            {
                if (digimon != null && !digimonCache.ContainsKey(digimon.digimonID))
                {
                    digimonCache[digimon.digimonID] = digimon;
                }
            }
            Console.WriteLine($"[DatabaseSO] {digimonCache.Count} Digimon cargados");
        }
        
        // Cargar Attacks
        if (allAttacks != null)
        {
            foreach (var attack in allAttacks)
            {
                if (attack != null && !attackCache.ContainsKey(attack.attackID))
                {
                    attackCache[attack.attackID] = attack;
                }
            }
            Console.WriteLine($"[DatabaseSO] {attackCache.Count} Attacks cargados");
        }
        
        // Cargar Items
        if (allItems != null)
        {
            foreach (var item in allItems)
            {
                if (item != null && !itemCache.ContainsKey(item.itemID))
                {
                    itemCache[item.itemID] = item;
                }
            }
            Console.WriteLine($"[DatabaseSO] {itemCache.Count} Items cargados");
        }
        
        isInitialized = true;
    }
    #endregion
    
    #region Digimon Methods
    /// <summary>
    /// Obtiene DigimonData por ID.
    /// </summary>
    public DigimonData GetDigimon(int digimonID)
    {
        if (!isInitialized) Initialize();
        
        if (digimonCache.TryGetValue(digimonID, out var digimon))
        {
            return digimon;
        }
        
        Console.Warning($"[DatabaseSO] Digimon ID {digimonID} no encontrado");
        return null;
    }
    
    /// <summary>
    /// Obtiene todos los Digimon de una etapa.
    /// </summary>
    public DigimonData[] GetDigimonByStage(EvolutionStage stage)
    {
        if (!isInitialized) Initialize();
        
        var result = new List<DigimonData>();
        
        foreach (var digimon in digimonCache.Values)
        {
            if (digimon.stage == stage)
            {
                result.Add(digimon);
            }
        }
        
        return result.ToArray();
    }
    
    /// <summary>
    /// Obtiene todos los Digimon de un elemento.
    /// </summary>
    public DigimonData[] GetDigimonByElement(ElementType element)
    {
        if (!isInitialized) Initialize();
        
        var result = new List<DigimonData>();
        
        foreach (var digimon in digimonCache.Values)
        {
            if (digimon.element == element)
            {
                result.Add(digimon);
            }
        }
        
        return result.ToArray();
    }
    
    /// <summary>
    /// Verifica si un ID de Digimon existe.
    /// </summary>
    public bool DigimonExists(int digimonID)
    {
        if (!isInitialized) Initialize();
        
        return digimonCache.ContainsKey(digimonID);
    }
    #endregion
    
    #region Attack Methods
    /// <summary>
    /// Obtiene AttackData por ID.
    /// </summary>
    public AttackData GetAttack(int attackID)
    {
        if (!isInitialized) Initialize();
        
        if (attackCache.TryGetValue(attackID, out var attack))
        {
            return attack;
        }
        
        Console.Warning($"[DatabaseSO] Attack ID {attackID} no encontrado");
        return null;
    }
    
    /// <summary>
    /// Obtiene todos los ataques de un tipo.
    /// </summary>
    public AttackData[] GetAttacksByType(AttackType type)
    {
        if (!isInitialized) Initialize();
        
        var result = new List<AttackData>();
        
        foreach (var attack in attackCache.Values)
        {
            if (attack.attackType == type)
            {
                result.Add(attack);
            }
        }
        
        return result.ToArray();
    }
    
    /// <summary>
    /// Obtiene todos los ataques de un elemento.
    /// </summary>
    public AttackData[] GetAttacksByElement(ElementType element)
    {
        if (!isInitialized) Initialize();
        
        var result = new List<AttackData>();
        
        foreach (var attack in attackCache.Values)
        {
            if (attack.element == element)
            {
                result.Add(attack);
            }
        }
        
        return result.ToArray();
    }
    #endregion
    
    #region Item Methods
    /// <summary>
    /// Obtiene ItemData por ID.
    /// </summary>
    public ItemDataSO GetItem(int itemID)
    {
        if (!isInitialized) Initialize();
        
        if (itemCache.TryGetValue(itemID, out var item))
        {
            return item;
        }
        
        Console.Warning($"[DatabaseSO] Item ID {itemID} no encontrado");
        return null;
    }
    
    /// <summary>
    /// Obtiene todos los items de un tipo.
    /// </summary>
    public ItemDataSO[] GetItemsByType(ItemType type)
    {
        if (!isInitialized) Initialize();
        
        var result = new List<ItemDataSO>();
        
        foreach (var item in itemCache.Values)
        {
            if (item.itemType == type)
            {
                result.Add(item);
            }
        }
        
        return result.ToArray();
    }
    #endregion
    
    #region Debug
    [ContextMenu("Debug/Show Database Info")]
    private void ShowDatabaseInfo()
    {
        Console.WriteLine("=== DATABASE INFO ===");
        Console.WriteLine($"Digimon: {digimonCache?.Count ?? 0}");
        Console.WriteLine($"Attacks: {attackCache?.Count ?? 0}");
        Console.WriteLine($"Items: {itemCache?.Count ?? 0}");
        Console.WriteLine($"Initialized: {isInitialized}");
        Console.WriteLine("====================");
    }
    
    [ContextMenu("Debug/Rebuild Caches")]
    private void RebuildCaches()
    {
        isInitialized = false;
        Initialize();
    }
    #endregion
}

/// <summary>
/// ItemData como ScriptableObject.
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Digimon/Create Item", order = 3)]
public class ItemDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public int itemID;
    public string itemName;
    
    [TextArea(2, 4)]
    public string description;
    
    public ItemType itemType;
    
    [Header("Stats")]
    public int buyPrice;
    public int sellPrice;
    
    [Header("Effect")]
    public ItemEffect itemEffect;
    public int effectValue;
    
    [Header("Sprite")]
    public Sprite itemSprite;
}
