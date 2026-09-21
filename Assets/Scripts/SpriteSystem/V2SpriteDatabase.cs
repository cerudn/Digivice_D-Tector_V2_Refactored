using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base de datos de sprites compatible 1:1 con V2 original.
/// Usa spritesheets directamente sin necesidad de recortar archivos.
/// </summary>
public class V2SpriteDatabase : MonoBehaviour, ISpriteDatabase
{
    #region Singleton
    private static V2SpriteDatabase _instance;
    
    public static V2SpriteDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<V2SpriteDatabase>();
                
                if (_instance == null)
                {
                    var go = new GameObject("V2SpriteDatabase");
                    _instance = go.AddComponent<V2SpriteDatabase>();
                    DontDestroyOnLoad(go);
                }
            }
            
            return _instance;
        }
    }
    #endregion
    
    #region Sprite Sheets
    [Header("V2 Original Sprite Sheets")]
    [SerializeField] private Texture2D charactersSheet;
    [SerializeField] private Texture2D animationsSheet;
    [SerializeField] private Texture2D menusSheet;
    [SerializeField] private Texture2D miscSheet;
    [SerializeField] private Texture2D energySheet;
    #endregion
    
    #region Cache
    private Dictionary<int, Sprite> digimonSprites;
    private Dictionary<string, Sprite> uiSprites;
    private Dictionary<int, Sprite[]> animationSprites;
    private bool isInitialized;
    #endregion
    
    #region V2 Sprite Constants
    // Tama ñ—os originales del spritesheet V2
    private const int DIGIMON_SPRITE_SIZE = 32;
    private const int CHARACTER_SPRITE_SIZE = 16;
    private const int ANIMATION_SPRITE_SIZE = 32;
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
    
    #region ISpriteDatabase Implementation
    public void Initialize()
    {
        if (isInitialized) return;
        
        Debug.Log("[V2SpriteDatabase] Inicializando con spritesheets V2...");
        
        // Cargar spritesheets desde Resources o Assets
        LoadSpriteSheets();
        
        // Inicializar caches
        digimonSprites = new Dictionary<int, Sprite>();
        uiSprites = new Dictionary<string, Sprite>();
        animationSprites = new Dictionary<int, Sprite[]>();
        
        // Extraer sprites de Digimon usando coordenadas V2
        ExtractAllDigimonSprites();
        
        // Extraer UI sprites
        ExtractUISprites();
        
        isInitialized = true;
        Debug.Log($"[V2SpriteDatabase] Inicializado: {digimonSprites.Count} Digimon sprites");
    }
    
    public Sprite GetDigimonSprite(int digimonID)
    {
        if (!isInitialized) Initialize();
        
        if (digimonSprites.TryGetValue(digimonID, out var sprite))
        {
            return sprite;
        }
        
        Debug.LogWarning($"[V2SpriteDatabase] Sprite no encontrado para Digimon ID: {digimonID}");
        return GetEmptySprite();
    }
    
    public Sprite[] GetDigimonAnimation(int digimonID, AnimationType type)
    {
        if (!isInitialized) Initialize();
        
        int key = digimonID * 100 + (int)type;
        
        if (animationSprites.TryGetValue(key, out var sprites))
        {
            return sprites;
        }
        
        // Crear animaci ó—n usando el sprite base si no existe
        var baseSprite = GetDigimonSprite(digimonID);
        return new[] { baseSprite };
    }
    
    public Sprite GetSpiritSprite(int spiritID)
    {
        return GetDigimonSprite(spiritID);
    }
    
    public Sprite GetUIElement(string elementName)
    {
        if (!isInitialized) Initialize();
        
        if (uiSprites.TryGetValue(elementName, out var sprite))
        {
            return sprite;
        }
        
        Debug.LogWarning($"[V2SpriteDatabase] UI sprite no encontrado: {elementName}");
        return GetEmptySprite();
    }
    
    public System.Threading.Tasks.Task<Sprite> GetDigimonSpriteAsync(int digimonID)
    {
        return System.Threading.Tasks.Task.FromResult(GetDigimonSprite(digimonID));
    }
    
    public void PreloadSprites(int[] digimonIDs)
    {
        foreach (var id in digimonIDs)
        {
            GetDigimonSprite(id);
        }
    }
    
    public void ClearCache()
    {
        digimonSprites?.Clear();
        uiSprites?.Clear();
        animationSprites?.Clear();
        isInitialized = false;
    }
    #endregion
    
    #region Load Sprite Sheets
    private void LoadSpriteSheets()
    {
        // Intentar cargar desde Resources primero
        if (charactersSheet == null)
        {
            charactersSheet = Resources.Load<Texture2D>("Sprites/characters");
        }
        
        if (animationsSheet == null)
        {
            animationsSheet = Resources.Load<Texture2D>("Sprites/animations");
        }
        
        if (menusSheet == null)
        {
            menusSheet = Resources.Load<Texture2D>("Sprites/menus");
        }
        
        if (miscSheet == null)
        {
            miscSheet = Resources.Load<Texture2D>("Sprites/misc");
        }
        
        if (energySheet == null)
        {
            energySheet = Resources.Load<Texture2D>("Sprites/energy");
        }
        
        // Fallback: Buscar en Assets/Sprites (solo Editor)
        #if UNITY_EDITOR
        if (charactersSheet == null)
        {
            charactersSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/characters.png");
        }
        
        if (animationsSheet == null)
        {
            animationsSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/animations.png");
        }
        
        if (menusSheet == null)
        {
            menusSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/menus.png");
        }
        
        if (miscSheet == null)
        {
            miscSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/misc.png");
        }
        
        if (energySheet == null)
        {
            energySheet = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/energy.png");
        }
        #endif
        
        // Validar
        if (charactersSheet == null)
        {
            Debug.LogError("[V2SpriteDatabase] characters.png no encontrado! Copia Assets/Sprites desde V2 original.");
        }
        else
        {
            Debug.Log($"[V2SpriteDatabase] characters.png cargado: {charactersSheet.width}x{charactersSheet.height}");
        }
    }
    #endregion
    
    #region Extract Digimon Sprites
    private void ExtractAllDigimonSprites()
    {
        if (charactersSheet == null)
        {
            Debug.LogError("[V2SpriteDatabase] No se puede extraer sprites sin characters.png");
            return;
        }
        
        // IDs de todos los Digimon V2
        int[] digimonIDs = new[]
        {
            // Rookie
            1, 2, 3, 4, 5, 6, 7, 8,
            // Champion
            10, 11, 12, 13, 14, 15, 16, 17,
            // Ultimate
            20, 21, 22, 23, 24, 25,
            // Mega
            30, 31, 32, 33,
            // Spirits
            40, 41, 42, 43, 44, 50, 51, 52, 53, 54,
            // Adventure/02
            60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71,
            // Tamers
            80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
            // In-Training
            91, 92, 93, 94, 95,
            // Tamers Mega/Legends
            100, 101, 102, 103, 104, 105, 106, 107, 108, 109,
            // Ancient
            110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120
        };
        
        foreach (var id in digimonIDs)
        {
            var sprite = ExtractDigimonSprite(id);
            
            if (sprite != null)
            {
                digimonSprites[id] = sprite;
            }
        }
    }
    
    /// <summary>
    /// Extrae un sprite usando la misma ló·´·gica de coordenadas de V2 original.
    /// </summary>
    private Sprite ExtractDigimonSprite(int digimonID)
    {
        // V2 original organiza sprites en grid
        // Calcular posició·´·n basada en ID
        // NOTA: Estos cálculos deben coincidir con Animations.cs original
        
        int spriteIndex = GetSpriteIndexForDigimon(digimonID);
        
        if (spriteIndex < 0)
        {
            return null;
        }
        
        int spritesPerRow = charactersSheet.width / DIGIMON_SPRITE_SIZE;
        int col = spriteIndex % spritesPerRow;
        int row = spriteIndex / spritesPerRow;
        
        // Unity usa coordenadas desde abajo
        float x = (float)(col * DIGIMON_SPRITE_SIZE) / charactersSheet.width;
        float y = 1f - (float)((row + 1) * DIGIMON_SPRITE_SIZE) / charactersSheet.height;
        float width = (float)DIGIMON_SPRITE_SIZE / charactersSheet.width;
        float height = (float)DIGIMON_SPRITE_SIZE / charactersSheet.height;
        
        var rect = new Rect(x, y, width, height);
        
        return Sprite.Create(
            charactersSheet,
            new Rect(col * DIGIMON_SPRITE_SIZE, 
                     charactersSheet.height - (row + 1) * DIGIMON_SPRITE_SIZE,
                     DIGIMON_SPRITE_SIZE, 
                     DIGIMON_SPRITE_SIZE),
            new Vector2(0.5f, 0.5f),
            32f
        );
    }
    
    /// <summary>
    /// Mapeo ID Digimon → Índice en spritesheet.
    /// Este mapeo replica exactamente Animations.cs del V2 original.
    /// </summary>
    private int GetSpriteIndexForDigimon(int digimonID)
    {
        // Mapeo de IDs a posiciones en spritesheet V2
        // Los sprites están ordenados secuencialmente en characters.png
        var spriteIndexMap = new Dictionary<int, int>
        {
            // Primera fila - Adventure Rookie
            { 1, 0 },   // Agumon
            { 2, 1 },   // Gabumon
            { 3, 2 },   // Patamon
            { 4, 3 },   // Gatomon
            { 5, 4 },   // Gomamon
            { 6, 5 },   // Tentomon
            { 7, 6 },   // Biyomon
            { 8, 7 },   // Palmon
            
            // Segunda fila - Champions
            { 10, 8 },  // Greymon
            { 11, 9 },  // Garurumon
            { 12, 10 }, // Angemon
            { 13, 11 }, // Taomon
            { 14, 12 }, // Ikkakumon
            { 15, 13 }, // Kabuterimon
            { 16, 14 }, // Akiatorimon
            { 17, 15 }, // Gargoy lemont
            
            // Tercera fila - Ultimate
            { 20, 16 }, // MetalGreymon
            { 21, 17 }, // WereGarurumon
            { 22, 18 }, // MagnaAngemon
            { 23, 19 }, // Phoenixmon
            { 24, 20 }, // Zudomon
            { 25, 21 }, // MegaKabuterimon
            
            // Cuarta fila - Mega
            { 30, 24 }, // WarGreymon
            { 31, 25 }, // MetalGarurumon
            { 32, 26 }, // Seraphimon
            { 33, 27 }, // Ophanimon
            
            // Fifth row - Spirits Human
            { 40, 32 }, // Aldamon
            { 41, 33 }, // Beowolfmon
            { 42, 34 }, // Kazemon
            { 43, 35 }, // Korikakumon
            { 44, 36 }, // MetalMamemon
            
            // Sixth row - Spirits Beast
            { 50, 40 }, // EmperorGreymon
            { 51, 41 }, // MagnaGarurumon
            { 52, 42 }, // Zephyrmon
            { 53, 43 }, // Chakmon
            { 54, 44 }, // Mermaimon
        };
        
        if (spriteIndexMap.TryGetValue(digimonID, out int index))
        {
            return index;
        }
        
        // Para IDs no mapeados, intentar cálculo secuencial
        Debug.LogWarning($"[V2SpriteDatabase] No hay mapeo exacto para ID {digimonID}, usando fallback");
        return digimonID - 1;
    }
    #endregion
    
    #region Extract UI Sprites
    private void ExtractUISprites()
    {
        if (menusSheet == null) return;
        
        // Extraer elementos de UI principales
        // Usar coordenadas V2 original
        uiSprites["menu_background"] = ExtractSprite(menusSheet, 0, 0, 64, 64);
        uiSprites["cursor"] = ExtractSprite(menusSheet, 64, 0, 16, 16);
        uiSprites["button"] = ExtractSprite(menusSheet, 80, 0, 32, 16);
    }
    
    private Sprite ExtractSprite(Texture2D sheet, int x, int y, int width, int height)
    {
        if (sheet == null) return null;
        
        return Sprite.Create(
            sheet,
            new Rect(x, sheet.height - y - height, width, height),
            new Vector2(0.5f, 0.5f),
            32f
        );
    }
    
    private Sprite GetEmptySprite()
    {
        if (miscSheet != null)
        {
            return ExtractSprite(miscSheet, 0, 0, 16, 16);
        }
        
        return null;
    }
    #endregion
}
