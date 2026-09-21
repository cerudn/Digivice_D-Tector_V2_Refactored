using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Composition Root autom Ã¡â€”tico del juego.
/// Se ejecuta antes de que cargue la primera escena.
/// </summary>
public class GameBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        Debug.Log("[GameBootstrapper] Initializing before scene load...");
        
        // Crear GameObject de managers si no existe
        var managersGO = GameObject.Find("Managers");
        
        if (managersGO == null)
        {
            managersGO = new GameObject("Managers");
            Object.DontDestroyOnLoad(managersGO);
            Debug.Log("[GameBootstrapper] Created 'Managers' GameObject");
        }
        
        // A Ã±â€”adir componentes en orden correcto
        var databaseSO = GetOrAddComponent<DatabaseSO>(managersGO);
        var savedGame = GetOrAddComponent<SavedGame>(managersGO);
        var inputManager = GetOrAddComponent<InputManager>(managersGO);
        var logicManager = GetOrAddComponent<LogicManager>(managersGO);
        var screenManager = GetOrAddComponent<ScreenManager>(managersGO);
        var audioManager = GetOrAddComponent<AudioManager>(managersGO);
        var worldManager = GetOrAddComponent<UnityEngine.XR.WSA.WorldManager>(managersGO);
        var gameManager = GetOrAddComponent<GameManager>(managersGO);
        
        Debug.Log("[GameBootstrapper] All managers added");
        
        // Cargar DatabaseSO desde Resources
        LoadDatabaseSO(databaseSO);
        
        // Inyectar dependencias
        InjectDependencies(
            gameManager,
            logicManager,
            screenManager,
            savedGame,
            audioManager,
            inputManager,
            worldManager,
            databaseSO
        );
        
        Debug.Log("[GameBootstrapper] Dependencies injected");
        
        // Inicializar GameManager (que inicializar Ã¡â€” el resto)
        gameManager.InitializeManually(
            logicManager,
            screenManager,
            savedGame,
            audioManager,
            databaseSO,
            inputManager,
            worldManager
        );
        
        Debug.Log("[GameBootstrapper] Initialization complete");
    }
    
    private static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        
        if (component == null)
        {
            component = go.AddComponent<T>();
            Debug.Log($"[GameBootstrapper] Added {typeof(T).Name}");
        }
        else
        {
            Debug.Log($"[GameBootstrapper] Found existing {typeof(T).Name}");
        }
        
        return component;
    }
    
    private static void LoadDatabaseSO(DatabaseSO databaseSO)
    {
        if (databaseSO == null) return;
        
        // Cargar todos los DigimonData desde Resources
        var allDigimon = Resources.LoadAll<DigimonData>("Digimon");
        var allAttacks = Resources.LoadAll<AttackData>("Attacks");
        var allItems = Resources.LoadAll<ItemDataSO>("Items");
        
        if (allDigimon.Length > 0)
        {
            databaseSO.allDigimon = allDigimon;
            Debug.Log($"[GameBootstrapper] Loaded {allDigimon.Length} Digimon from Resources");
        }
        
        if (allAttacks.Length > 0)
        {
            databaseSO.allAttacks = allAttacks;
            Debug.Log($"[GameBootstrapper] Loaded {allAttacks.Length} Attacks from Resources");
        }
        
        if (allItems.Length > 0)
        {
            databaseSO.allItems = allItems;
            Debug.Log($"[GameBootstrapper] Loaded {allItems.Length} Items from Resources");
        }
        
        // Si no hay nada en Resources, buscar en Assets/Data
        if (allDigimon.Length == 0)
        {
            // Buscar en Assets/Data/Digimon
            var guids = UnityEditor.AssetDatabase.FindAssets("t:DigimonData", new[] { "Assets/Data/Digimon" });
            var digimonList = new List<DigimonData>();
            
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var digimon = UnityEditor.AssetDatabase.LoadAssetAtPath<DigimonData>(path);
                
                if (digimon != null)
                {
                    digimonList.Add(digimon);
                }
            }
            
            if (digimonList.Count > 0)
            {
                databaseSO.allDigimon = digimonList.ToArray();
                Debug.Log($"[GameBootstrapper] Loaded {digimonList.Count} Digimon from Assets/Data");
            }
        }
    }
    
    private static void InjectDependencies(
        GameManager gameManager,
        LogicManager logicManager,
        ScreenManager screenManager,
        SavedGame savedGame,
        AudioManager audioManager,
        InputManager inputManager,
        UnityEngine.XR.WSA.WorldManager worldManager,
        DatabaseSO databaseSO)
    {
        // GameManager ya tiene las referencias asignadas por InitializeManually
        
        // LogicManager necesita ScreenManager e InputManager
        logicManager?.Initialize(screenManager, inputManager, databaseSO);
        
        // ScreenManager necesita Audio Manager
        screenManager?.Initialize(audioManager);
        
        // SavedGame ya es singleton
        
        // Audio Manager ya es singleton
        
        // Input Manager ya es singleton
        
        // World Manager necesita DatabaseSO
        worldManager?.Initialize(databaseSO);
        
        Debug.Log("[GameBootstrapper] All dependencies injected");
    }
}

/// <summary>
/// ExtensiÃ³Â·Â´Â·n para GameManager para permitir inicializaciÃ³Â·Â´Â·n manual.
/// </summary>
public static partial class GameManagerExtensions
{
    public static void InitializeManually(
        this GameManager gameManager,
        ILogicManager logicManager,
        IScreenManager screenManager,
        ISaveSystem saveSystem,
        IAudioManager audioManager,
        ISpriteDatabase spriteDatabase,
        IInputManager inputManager,
        IWorldManager worldManager)
    {
        if (gameManager == null) return;
        
        // Usar reflexiÃ³Â·Â´Â·n o m Ã©â€”todo p Ãºâ€”blico para asignar dependencias
        var gameManagerType = gameManager.GetType();
        
        // Asignar campos privados
        var logicField = gameManagerType.GetField("logicManager", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var screenField = gameManagerType.GetField("screenManager", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var saveField = gameManagerType.GetField("savedGame", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var audioField = gameManagerType.GetField("audioManager", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var spriteField = gameManagerType.GetField("spriteDatabase", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var inputField = gameManagerType.GetField("inputManager", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var worldField = gameManagerType.GetField("worldManager", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (logicField != null) logicField.SetValue(gameManager, logicManager);
        if (screenField != null) screenField.SetValue(gameManager, screenManager);
        if (saveField != null) saveField.SetValue(gameManager, saveSystem);
        if (audioField != null) audioField.SetValue(gameManager, audioManager);
        if (spriteField != null) spriteField.SetValue(gameManager, spriteDatabase);
        if (inputField != null) inputField.SetValue(gameManager, inputManager);
        if (worldField != null) worldField.SetValue(gameManager, worldManager);
        
        Debug.Log("[GameManagerExtensions] Dependencies assigned via reflection");
    }
}

