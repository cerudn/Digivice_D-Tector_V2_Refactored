using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// Sistema de guardado y carga de partidas con versionado.
/// Implementa migració·´·n autom á—tica entre versiones de schema.
/// </summary>
public class SavedGame : MonoBehaviour, ISaveSystem
{
    #region Singleton
    private static SavedGame _instance;
    
    public static SavedGame Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SavedGame>();
                
                if (_instance == null)
                {
                    var go = new GameObject("SavedGame");
                    _instance = go.AddComponent<SavedGame>();
                    DontDestroyOnLoad(go);
                }
            }
            
            return _instance;
        }
    }
    #endregion
    
    #region ISaveSystem Implementation
    public int CurrentVersion => GameConstants.SAVE_VERSION;
    
    public event Action OnSaveCompleted;
    public event Action OnLoadCompleted;
    public event Action<string> OnSaveError;
    #endregion
    
    #region Constants
    private const string SAVE_KEY = "digivice_save_data";
    private const string VERSION_KEY = "digivice_save_version";
    private const string CHECKSUM_KEY = "digivice_save_checksum";
    #endregion
    
    #region State
    private SaveData currentSave;
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
    
    #region ISaveSystem Methods
    public void Initialize()
    {
        Console.WriteLine("[SavedGame] Inicializando sistema de guardado...");
        
        // Suscribirse a eventos de auto-guardado
        GameEvents.OnGameSaved += AutoSave;
        GameEvents.OnBattleWon += AutoSave;
        GameEvents.OnEvolutionCompleted += OnDigimonEvolved;
        
        isInitialized = true;
        Console.WriteLine("[SavedGame] Sistema inicializado");
    }
    
    public void Save(SaveData data)
    {
        if (!isInitialized)
        {
            Console.Error("[SavedGame] Error: Sistema no inicializado");
            OnSaveError?.Invoke("Sistema de guardado no inicializado");
            return;
        }
        
        try
        {
            // Actualizar versió·´·n
            data.version = CurrentVersion;
            
            // Calcular checksum
            data.checksum = CalculateChecksum(data);
            
            // Serializar a JSON
            string json = JsonUtility.ToJson(data, true);
            
            // Encriptar
            string encrypted = Encrypt(json);
            
            // Guardar en PlayerPrefs
            PlayerPrefs.SetString(SAVE_KEY, encrypted);
            PlayerPrefs.SetInt(VERSION_KEY, CurrentVersion);
            PlayerPrefs.SetString(CHECKSUM_KEY, data.checksum);
            PlayerPrefs.Save();
            
            currentSave = data;
            
            Console.WriteLine($"[SavedGame] Partida guardada (Versió·´·n {CurrentVersion})");
            OnSaveCompleted?.Invoke();
            GameEvents.TriggerGameSaved();
        }
        catch (Exception ex)
        {
            Console.Error($"[SavedGame] Error al guardar: {ex.Message}");
            OnSaveError?.Invoke(ex.Message);
        }
    }
    
    public SaveData Load()
    {
        if (!isInitialized)
        {
            Console.Error("[SavedGame] Error: Sistema no inicializado");
            return null;
        }
        
        try
        {
            // Verificar si existe guardado
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                Console.WriteLine("[SavedGame] No existe partida guardada");
                return CreateNewSave();
            }
            
            // Obtener versió·´·n
            int savedVersion = PlayerPrefs.GetInt(VERSION_KEY, 0);
            
            if (savedVersion == 0)
            {
                Console.WriteLine("[SavedGame] Versió·´·n de guardado inv á—lida");
                return CreateNewSave();
            }
            
            // Cargar datos encriptados
            string encrypted = PlayerPrefs.GetString(SAVE_KEY);
            
            // Desencriptar
            string json = Decrypt(encrypted);
            
            // Deserializar
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            // Validar checksum
            if (!ValidateChecksum(data))
            {
                Console.Warning("[SavedGame] Checksum inv á—lido - datos corruptos");
                OnSaveError?.Invoke("Datos de guardado corruptos");
                return CreateNewSave();
            }
            
            // Migrar si es necesario
            if (data.version < CurrentVersion)
            {
                Console.WriteLine($"[SavedGame] Migrando de versió·´·n {data.version} a {CurrentVersion}");
                data = MigrateSaveData(data, data.version, CurrentVersion);
            }
            
            currentSave = data;
            
            Console.WriteLine($"[SavedGame] Partida cargada (Versió·´·n {data.version})");
            OnLoadCompleted?.Invoke();
            GameEvents.TriggerGameLoaded();
            
            return data;
        }
        catch (Exception ex)
        {
            Console.Error($"[SavedGame] Error al cargar: {ex.Message}");
            OnSaveError?.Invoke(ex.Message);
            return CreateNewSave();
        }
    }
    
    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY) && PlayerPrefs.HasKey(VERSION_KEY);
    }
    
    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.DeleteKey(VERSION_KEY);
        PlayerPrefs.DeleteKey(CHECKSUM_KEY);
        PlayerPrefs.Save();
        
        currentSave = null;
        
        Console.WriteLine("[SavedGame] Partida eliminada");
    }
    #endregion
    
    #region Auto Save
    private float lastAutoSaveTime;
    
    void Update()
    {
        if (!isInitialized) return;
        
        // Auto-guardado cada 60 segundos
        if (Time.time - lastAutoSaveTime >= GameConstants.AUTO_SAVE_INTERVAL)
        {
            AutoSave();
            lastAutoSaveTime = Time.time;
        }
    }
    
    private void AutoSave()
    {
        if (currentSave != null)
        {
            Save(currentSave);
        }
    }
    
    private void OnDigimonEvolved(Digimon digimon)
    {
        // Guardar después de evolució·´·n importante
        if (digimon.Stage >= EvolutionStage.Champion)
        {
            Invoke(nameof(AutoSave), 2f); // Pequeñ·´·o delay para que termine la animació·´·n
        }
    }
    #endregion
    
    #region Checksum
    private string CalculateChecksum(SaveData data)
    {
        // Serializar sin checksum para calcularlo
        string json = JsonUtility.ToJson(data);
        
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
            
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            
            return builder.ToString();
        }
    }
    
    private bool ValidateChecksum(SaveData data)
    {
        string storedChecksum = data.checksum;
        string calculatedChecksum = CalculateChecksum(data);
        
        return storedChecksum == calculatedChecksum;
    }
    #endregion
    
    #region Encryption
    private string Encrypt(string plainText)
    {
        // Implementació·´·n simple - usar algoritmo má—s seguro en producció·´·n
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        
        // XOR simple con key (mejorar para producció·´·n)
        byte[] key = Encoding.UTF8.GetBytes("DigiviceV2SaveKey");
        
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
        }
        
        return Convert.ToBase64String(bytes);
    }
    
    private string Decrypt(string encryptedText)
    {
        byte[] bytes = Convert.FromBase64String(encryptedText);
        
        byte[] key = Encoding.UTF8.GetBytes("DigiviceV2SaveKey");
        
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
        }
        
        return Encoding.UTF8.GetString(bytes);
    }
    #endregion
    
    #region Migration
    private SaveData MigrateSaveData(SaveData oldData, int fromVersion, int toVersion)
    {
        SaveData newData = oldData;
        
        // Migració·´·n V2 → V3
        if (fromVersion <= 2 && toVersion >= 3)
        {
            newData = MigrateV2toV3(newData);
        }
        
        // Migració·´·n V3 → V4
        if (fromVersion <= 3 && toVersion >= 4)
        {
            newData = MigrateV3toV4(newData);
        }
        
        newData.version = toVersion;
        
        return newData;
    }
    
    private SaveData MigrateV2toV3(SaveData v2Data)
    {
        Console.WriteLine("[SavedGame] Migrando V2 → V3");
        
        var v3Data = new SaveData
        {
            version = 3,
            player = v2Data.player,
            totalTimePlayed = v2Data.totalTimePlayed,
            lastSaveDate = DateTime.Now,
            digimonTeam = new System.Collections.Generic.List<DigimonSaveData>(),
            inventory = new System.Collections.Generic.List<ItemData>(),
            worldProgress = v2Data.worldProgress
        };
        
        // Migrar Digimon (posibles cambios de ID)
        foreach (var digimon in v2Data.digimonTeam)
        {
            var newDigimon = new DigimonSaveData
            {
                digimonID = MigrateDigimonID(digimon.digimonID, 2, 3),
                nickname = digimon.nickname,
                level = digimon.level,
                experience = digimon.experience,
                hp = digimon.hp,
                mp = digimon.mp,
                attack = digimon.attack,
                defense = digimon.defense,
                speed = digimon.speed,
                friendship = digimon.friendship,
                attacks = digimon.attacks
            };
            
            v3Data.digimonTeam.Add(newDigimon);
        }
        
        // Migrar inventory
        v3Data.inventory = new System.Collections.Generic.List<ItemData>(v2Data.inventory);
        
        return v3Data;
    }
    
    private SaveData MigrateV3toV4(SaveData v3Data)
    {
        Console.WriteLine("[SavedGame] Migrando V3 → V4");
        
        var v4Data = new SaveData
        {
            version = 4,
            player = v3Data.player,
            totalTimePlayed = v3Data.totalTimePlayed,
            lastSaveDate = DateTime.Now,
            digimonTeam = new System.Collections.Generic.List<DigimonSaveData>(),
            inventory = new System.Collections.Generic.List<ItemData>(),
            worldProgress = v3Data.worldProgress
        };
        
        // Migrar Digimon (nuevos IDs de V4)
        foreach (var digimon in v3Data.digimonTeam)
        {
            var newDigimon = new DigimonSaveData
            {
                digimonID = MigrateDigimonID(digimon.digimonID, 3, 4),
                nickname = digimon.nickname,
                level = digimon.level,
                experience = digimon.experience,
                hp = digimon.hp,
                mp = digimon.mp,
                attack = digimon.attack,
                defense = digimon.defense,
                speed = digimon.speed,
                friendship = digimon.friendship,
                attacks = digimon.attacks
            };
            
            v4Data.digimonTeam.Add(newDigimon);
        }
        
        // Migrar inventory
        v4Data.inventory = new System.Collections.Generic.List<ItemData>(v3Data.inventory);
        
        // Nuevos campos de V4
        v4Data.worldProgress.unlockedWorlds.Add(WorldIDs.FOLDER_CONTINENT); // Desbloquear nuevo mundo
        
        return v4Data;
    }
    
    private int MigrateDigimonID(int oldID, int fromVersion, int toVersion)
    {
        // Mapeo de IDs entre versiones
        // La mayorí·´­a de IDs se mantienen, pero algunos cambian
        
        // Ejemplo: Si en V4 añ·´·adimos Digimon antes del ID 10,
        // los IDs >= 10 deben incrementarse
        
        // Placeholder - implementar segó·´·n cambios reales
        return oldID;
    }
    #endregion
    
    #region Utility
    private SaveData CreateNewSave()
    {
        Console.WriteLine("[SavedGame] Creando nueva partida");
        
        var newSave = new SaveData
        {
            version = CurrentVersion,
            player = new PlayerData
            {
                playerName = "Entrenador",
                playerID = 1,
                level = 1,
                experience = 0,
                steps = 0,
                money = 0,
                playTime = 0
            },
            digimonTeam = new System.Collections.Generic.List<DigimonSaveData>(),
            inventory = new System.Collections.Generic.List<ItemData>(),
            worldProgress = new WorldData(),
            totalTimePlayed = 0,
            lastSaveDate = DateTime.Now
        };
        
        // Dar Digimon inicial
        newSave.digimonTeam.Add(new DigimonSaveData
        {
            digimonID = DigimonIDs.AGUMON,
            nickname = "Agumon",
            level = 1,
            experience = 0,
            hp = 50,
            mp = 20,
            attack = 60,
            defense = 40,
            speed = 50,
            friendship = 50,
            attacks = new System.Collections.Generic.List<int> { 1, 2 }
        });
        
        return newSave;
    }
    
    [ContextMenu("Debug/Show Save Info")]
    private void ShowSaveInfo()
    {
        Console.WriteLine("=== SAVE INFO ===");
        Console.WriteLine($"Has Save: {HasSaveData()}");
        Console.WriteLine($"Current Version: {CurrentVersion}");
        
        if (HasSaveData())
        {
            int savedVersion = PlayerPrefs.GetInt(VERSION_KEY, 0);
            Console.WriteLine($"Saved Version: {savedVersion}");
            
            if (currentSave != null)
        {
                Console.WriteLine($"Player: {currentSave.player.playerName}");
                Console.WriteLine($"Digimon Count: {currentSave.digimonTeam.Count}");
                Console.WriteLine($"Total Time: {currentSave.totalTimePlayed}s");
            }
        }
        
        Console.WriteLine("================");
    }
    #endregion
}
