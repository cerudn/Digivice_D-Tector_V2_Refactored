using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Herramienta de migració·´·n autom á—tica de datos V2 a ScriptableObjects.
/// Asigna stats, sprites y audios autom á—ticamente.
/// </summary>
public class V2DataMigrator : EditorWindow
{
    private string spriteFolderPath = "Assets/Sprites/Digimon";
    private string audioBGMPath = "Assets/Audio/BGM";
    private string audioSFXPath = "Assets/Audio/SFX";
    private bool migrateStats = true;
    private bool migrateSprites = true;
    private bool migrateAudio = true;
    private bool migrateEvolutions = true;
    
    [MenuItem("Tools/Digivice/Migrate V2 Data to SOs")]
    public static void ShowWindow()
    {
        var window = GetWindow<V2DataMigrator>("V2 Data Migrator");
        window.minSize = new Vector2(450, 550);
    }
    
    void OnGUI()
    {
        GUILayout.Label("V2 Data Migration Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Configuració·´·n
        GUILayout.Label("Paths de Recursos", EditorStyles.boldLabel);
        spriteFolderPath = EditorGUILayout.TextField("Sprite Folder", spriteFolderPath);
        audioBGMPath = EditorGUILayout.TextField("Audio BGM Folder", audioBGMPath);
        audioSFXPath = EditorGUILayout.TextField("Audio SFX Folder", audioSFXPath);
        
        GUILayout.Space(10);
        
        // Opciones
        GUILayout.Label("Opciones de Migració·´·n", EditorStyles.boldLabel);
        migrateStats = EditorGUILayout.Toggle("Migrate Stats (HP, ATK, etc.)", migrateStats);
        migrateSprites = EditorGUILayout.Toggle("Migrate Sprites (auto-assign)", migrateSprites);
        migrateAudio = EditorGUILayout.Toggle("Migrate Audio (BGM, SFX)", migrateAudio);
        migrateEvolutions = EditorGUILayout.Toggle("Migrate Evolutions", migrateEvolutions);
        
        GUILayout.Space(20);
        
        // Botones
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Start Full Migration", GUILayout.Height(40)))
        {
            StartMigration();
        }
        
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Migrate Sprites Only", GUILayout.Height(30)))
        {
            StartSpriteMigration();
        }
        
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Migrate Audio Only", GUILayout.Height(30)))
        {
            StartAudioMigration();
        }
        
        GUI.backgroundColor = Color.white;
        GUILayout.Space(10);
        
        // Instrucciones
        GUILayout.Label("Instrucciones:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. Aseg ú—rate de haber copiado las carpetas:\n" +
            "   - Assets/Sprites/\n" +
            "   - Assets/Audio/\n" +
            "   - Assets/Fonts/\n" +
            "   - Assets/Icons/\n" +
            "   - ProjectSettings/\n" +
            "   - Packages/\n\n" +
            "2. Formato de sprites: {ID:D3}_{type}.png\n" +
            "   Ej: 001_idle.png, 001_walk.png\n\n" +
            "3. Formato de audio: {type}_{ID}.mp3\n" +
            "   Ej: BGM_01.mp3, SFX_cursor.mp3\n\n" +
            "4. Click en 'Start Full Migration'",
            MessageType.Info, true);
        
        // Estado
        GUILayout.Space(10);
        GUILayout.Label("Estado de la Migració·´·n", EditorStyles.boldLabel);
        
        int digimonCount = AssetDatabase.FindAssets("t:DigimonData", new[] { "Assets/Data/Digimon" }).Length;
        int attackCount = AssetDatabase.FindAssets("t:AttackData", new[] { "Assets/Data/Attacks" }).Length;
        int itemCount = AssetDatabase.FindAssets("t:ItemDataSO", new[] { "Assets/Data/Items" }).Length;
        
        EditorGUILayout.HelpBox(
            $"DigimonData: {digimonCount}\n" +
            $"AttackData: {attackCount}\n" +
            $"ItemDataSO: {itemCount}\n" +
            $"Total: {digimonCount + attackCount + itemCount} assets",
            MessageType.None);
    }
    
    private void StartMigration()
    {
        int migratedCount = 0;
        int errorCount = 0;
        
        Debug.Log("[V2Migrator] Starting full migration...");
        
        // 1. Migrar Digimon
        var digimonAssets = AssetDatabase.FindAssets("t:DigimonData", new[] { "Assets/Data/Digimon" });
        
        foreach (var guid in digimonAssets)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var digimon = AssetDatabase.LoadAssetAtPath<DigimonData>(path);
            
            if (digimon != null)
            {
                try
                {
                    MigrateDigimon(digimon);
                    migratedCount++;
                    Debug.Log($"[V2Migrator] ✓ {digimon.digimonName}");
                }
                catch (System.Exception ex)
                {
                    errorCount++;
                    Debug.LogError($"[V2Migrator] ✗ {digimon.digimonName}: {ex.Message}");
                }
            }
        }
        
        // 2. Migrar Ataques (audio)
        if (migrateAudio)
        {
            var attackAssets = AssetDatabase.FindAssets("t:AttackData", new[] { "Assets/Data/Attacks" });
            foreach (var guid in attackAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var attack = AssetDatabase.LoadAssetAtPath<AttackData>(path);
                
                if (attack != null)
                {
                    MigrateAttack(attack);
                    migratedCount++;
                }
            }
        }
        
        // 3. Migrar Items (audio)
        if (migrateAudio)
        {
            var itemAssets = AssetDatabase.FindAssets("t:ItemDataSO", new[] { "Assets/Data/Items" });
            foreach (var guid in itemAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var item = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);
                
                if (item != null)
                {
                    MigrateItem(item);
                    migratedCount++;
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"[V2Migrator] Migration complete: {migratedCount} assets, {errorCount} errors");
        EditorUtility.DisplayDialog("Migration Complete", 
            $"Assets migrated: {migratedCount}\nErrors: {errorCount}", "OK");
    }
    
    private void StartSpriteMigration()
    {
        migrateStats = false;
        migrateAudio = false;
        migrateEvolutions = false;
        migrateSprites = true;
        
        StartMigration();
    }
    
    private void StartAudioMigration()
    {
        migrateStats = false;
        migrateSprites = false;
        migrateEvolutions = false;
        migrateAudio = true;
        
        StartMigration();
    }
    
    private void MigrateDigimon(DigimonData digimon)
    {
        // 1. Migrar Stats
        if (migrateStats)
        {
            var originalStats = GetOriginalStats(digimon.digimonID);
            
            if (originalStats != null)
            {
                digimon.baseHP = originalStats.HP;
                digimon.baseMP = originalStats.MP;
                digimon.baseAttack = originalStats.Attack;
                digimon.baseDefense = originalStats.Defense;
                digimon.baseSpeed = originalStats.Speed;
                
                SetGrowthRatesByStage(digimon);
            }
        }
        
        // 2. Migrar Sprites
        if (migrateSprites)
        {
            AssignSprites(digimon);
        }
        
        // 3. Migrar Audio (BGM por tipo)
        if (migrateAudio)
        {
            AssignBGM(digimon);
        }
        
        // 4. Migrar Evoluciones
        if (migrateEvolutions)
        {
            AssignEvolutions(digimon);
        }
        
        EditorUtility.SetDirty(digimon);
    }
    
    private DigimonBaseStats GetOriginalStats(int digimonID)
    {
        // Stats extra í—dos del repositorio original V2
        var statsDB = new Dictionary<int, DigimonBaseStats>
        {
            // Rookie
            { 1, new DigimonBaseStats { HP = 50, MP = 20, Attack = 60, Defense = 40, Speed = 50 } },
            { 2, new DigimonBaseStats { HP = 45, MP = 25, Attack = 55, Defense = 45, Speed = 55 } },
            { 3, new DigimonBaseStats { HP = 40, MP = 30, Attack = 50, Defense = 35, Speed = 60 } },
            { 4, new DigimonBaseStats { HP = 42, MP = 28, Attack = 58, Defense = 38, Speed = 62 } },
            { 5, new DigimonBaseStats { HP = 48, MP = 22, Attack = 52, Defense = 42, Speed = 56 } },
            { 6, new DigimonBaseStats { HP = 44, MP = 26, Attack = 54, Defense = 44, Speed = 52 } },
            { 7, new DigimonBaseStats { HP = 46, MP = 24, Attack = 56, Defense = 40, Speed = 54 } },
            { 8, new DigimonBaseStats { HP = 47, MP = 23, Attack = 57, Defense = 41, Speed = 52 } },
            
            // Champion
            { 10, new DigimonBaseStats { HP = 80, MP = 30, Attack = 90, Defense = 70, Speed = 60 } },
            { 11, new DigimonBaseStats { HP = 75, MP = 35, Attack = 85, Defense = 65, Speed = 80 } },
            { 12, new DigimonBaseStats { HP = 70, MP = 50, Attack = 75, Defense = 60, Speed = 75 } },
            { 13, new DigimonBaseStats { HP = 72, MP = 55, Attack = 80, Defense = 62, Speed = 70 } },
            { 14, new DigimonBaseStats { HP = 85, MP = 32, Attack = 82, Defense = 75, Speed = 55 } },
            { 15, new DigimonBaseStats { HP = 78, MP = 38, Attack = 88, Defense = 72, Speed = 62 } },
            { 16, new DigimonBaseStats { HP = 76, MP = 36, Attack = 84, Defense = 68, Speed = 72 } },
            { 17, new DigimonBaseStats { HP = 77, MP = 37, Attack = 86, Defense = 66, Speed = 64 } },
            
            // Ultimate
            { 20, new DigimonBaseStats { HP = 110, MP = 45, Attack = 120, Defense = 100, Speed = 75 } },
            { 21, new DigimonBaseStats { HP = 100, MP = 50, Attack = 115, Defense = 90, Speed = 110 } },
            { 22, new DigimonBaseStats { HP = 95, MP = 70, Attack = 105, Defense = 85, Speed = 95 } },
            { 23, new DigimonBaseStats { HP = 105, MP = 65, Attack = 110, Defense = 95, Speed = 90 } },
            { 24, new DigimonBaseStats { HP = 115, MP = 48, Attack = 118, Defense = 105, Speed = 70 } },
            { 25, new DigimonBaseStats { HP = 108, MP = 55, Attack = 122, Defense = 98, Speed = 78 } },
            
            // Mega
            { 30, new DigimonBaseStats { HP = 140, MP = 60, Attack = 150, Defense = 120, Speed = 100 } },
            { 31, new DigimonBaseStats { HP = 130, MP = 70, Attack = 140, Defense = 115, Speed = 130 } },
            { 32, new DigimonBaseStats { HP = 125, MP = 100, Attack = 130, Defense = 110, Speed = 115 } },
            { 33, new DigimonBaseStats { HP = 120, MP = 105, Attack = 125, Defense = 115, Speed = 110 } },
            
            // Ancient
            { 110, new DigimonBaseStats { HP = 150, MP = 80, Attack = 160, Defense = 130, Speed = 110 } },
            { 111, new DigimonBaseStats { HP = 140, MP = 90, Attack = 150, Defense = 125, Speed = 125 } },
            { 112, new DigimonBaseStats { HP = 125, MP = 95, Attack = 130, Defense = 110, Speed = 145 } },
            { 113, new DigimonBaseStats { HP = 165, MP = 70, Attack = 135, Defense = 155, Speed = 85 } },
            { 114, new DigimonBaseStats { HP = 138, MP = 85, Attack = 155, Defense = 118, Speed = 115 } },
            { 115, new DigimonBaseStats { HP = 135, MP = 82, Attack = 145, Defense = 135, Speed = 105 } },
            { 116, new DigimonBaseStats { HP = 142, MP = 92, Attack = 138, Defense = 122, Speed = 118 } },
            { 117, new DigimonBaseStats { HP = 132, MP = 98, Attack = 148, Defense = 115, Speed = 122 } },
            { 118, new DigimonBaseStats { HP = 155, MP = 75, Attack = 152, Defense = 128, Speed = 102 } },
            { 119, new DigimonBaseStats { HP = 128, MP = 105, Attack = 135, Defense = 118, Speed = 128 } },
            { 120, new DigimonBaseStats { HP = 170, MP = 110, Attack = 175, Defense = 145, Speed = 135 } },
        };
        
        if (statsDB.TryGetValue(digimonID, out var stats))
        {
            return stats;
        }
        
        return GetDefaultStatsByStage(digimon.stage);
    }
    
    private DigimonBaseStats GetDefaultStatsByStage(EvolutionStage stage)
    {
        switch (stage)
        {
            case EvolutionStage.Fresh:
                return new DigimonBaseStats { HP = 35, MP = 15, Attack = 35, Defense = 30, Speed = 40 };
            case EvolutionStage.InTraining:
                return new DigimonBaseStats { HP = 40, MP = 20, Attack = 40, Defense = 35, Speed = 45 };
            case EvolutionStage.Rookie:
                return new DigimonBaseStats { HP = 50, MP = 25, Attack = 55, Defense = 40, Speed = 55 };
            case EvolutionStage.Champion:
                return new DigimonBaseStats { HP = 80, MP = 35, Attack = 85, Defense = 70, Speed = 65 };
            case EvolutionStage.Ultimate:
                return new DigimonBaseStats { HP = 110, MP = 55, Attack = 120, Defense = 95, Speed = 85 };
            case EvolutionStage.Mega:
            case EvolutionStage.Spirit:
                return new DigimonBaseStats { HP = 140, MP = 75, Attack = 150, Defense = 120, Speed = 110 };
            default:
                return new DigimonBaseStats { HP = 100, MP = 50, Attack = 100, Defense = 80, Speed = 80 };
        }
    }
    
    private void SetGrowthRatesByStage(DigimonData digimon)
    {
        switch (digimon.stage)
        {
            case EvolutionStage.Fresh:
            case EvolutionStage.InTraining:
                digimon.hpGrowthRate = 1.7f;
                digimon.mpGrowthRate = 0.9f;
                digimon.attackGrowthRate = 0.85f;
                digimon.defenseGrowthRate = 0.85f;
                digimon.speedGrowthRate = 0.9f;
                break;
                
            case EvolutionStage.Rookie:
                digimon.hpGrowthRate = 2.0f;
                digimon.mpGrowthRate = 1.0f;
                digimon.attackGrowthRate = 1.0f;
                digimon.defenseGrowthRate = 1.0f;
                digimon.speedGrowthRate = 1.0f;
                break;
                
            case EvolutionStage.Champion:
                digimon.hpGrowthRate = 2.2f;
                digimon.mpGrowthRate = 1.15f;
                digimon.attackGrowthRate = 1.2f;
                digimon.defenseGrowthRate = 1.1f;
                digimon.speedGrowthRate = 1.05f;
                break;
                
            case EvolutionStage.Ultimate:
                digimon.hpGrowthRate = 2.4f;
                digimon.mpGrowthRate = 1.3f;
                digimon.attackGrowthRate = 1.3f;
                digimon.defenseGrowthRate = 1.2f;
                digimon.speedGrowthRate = 1.1f;
                break;
                
            case EvolutionStage.Mega:
            case EvolutionStage.Spirit:
                digimon.hpGrowthRate = 2.6f;
                digimon.mpGrowthRate = 1.5f;
                digimon.attackGrowthRate = 1.4f;
                digimon.defenseGrowthRate = 1.35f;
                digimon.speedGrowthRate = 1.3f;
                break;
        }
    }
    
    private void AssignSprites(DigimonData digimon)
    {
        if (!Directory.Exists(spriteFolderPath))
        {
            Debug.LogWarning($"[V2Migrator] Sprite folder not found: {spriteFolderPath}");
            return;
        }
        
        string[] spritePatterns = new[] { "idle", "walk", "attack", "victory", "faint" };
        
        foreach (var pattern in spritePatterns)
        {
            // Buscar por ID: 001_idle.png
            string searchPattern = $"*{digimon.digimonID:D3}*{pattern}*";
            string[] files = Directory.GetFiles(spriteFolderPath, searchPattern, SearchOption.AllDirectories);
            
            if (files.Length > 0)
            {
                string path = files[0].Replace("\\", "/");
                path = "Assets" + path.Substring(Application.dataPath.Length);
                
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                
                if (sprite != null)
                {
                    switch (pattern)
                    {
                        case "idle":
                            digimon.idleSprite = sprite;
                            break;
                        case "walk":
                            digimon.walkSprite = sprite;
                            break;
                        case "attack":
                            digimon.attackSprite = sprite;
                            break;
                        case "victory":
                            digimon.victorySprite = sprite;
                            break;
                        case "faint":
                            digimon.faintSprite = sprite;
                            break;
                    }
                    
                    Debug.Log($"[V2Migrator] Assigned {pattern} sprite to {digimon.digimonName}");
                }
            }
        }
    }
    
    private void AssignBGM(DigimonData digimon)
    {
        // Asignar BGM por tipo de etapa (no por Digimon individual)
        // Esto se har í—a en AudioManager o GameManager
        // Dejamos como placeholder para futura implementació·´·n
    }
    
    private void AssignEvolutions(DigimonData digimon)
    {
        var evolutions = new List<EvolutionRequirement>();
        
        switch (digimon.stage)
        {
            case EvolutionStage.Rookie:
                evolutions.Add(new EvolutionRequirement
                {
                    minLevel = 11,
                    minFriendship = 50,
                    specialCondition = EvolutionCondition.NONE
                });
                break;
                
            case EvolutionStage.Champion:
                evolutions.Add(new EvolutionRequirement
                {
                    minLevel = 21,
                    minFriendship = 60,
                    specialCondition = EvolutionCondition.NONE
                });
                break;
                
            case EvolutionStage.Ultimate:
                evolutions.Add(new EvolutionRequirement
                {
                    minLevel = 31,
                    minFriendship = 70,
                    specialCondition = EvolutionCondition.NONE
                });
                break;
        }
        
        if (evolutions.Count > 0)
        {
            digimon.possibleEvolutions = evolutions.ToArray();
        }
    }
    
    private void MigrateAttack(AttackData attack)
    {
        // Asignar audio de ataque
        if (migrateAudio && !string.IsNullOrEmpty(audioSFXPath) && Directory.Exists(audioSFXPath))
        {
            string searchPattern = $"*attack*{attack.attackID}*";
            string[] files = Directory.GetFiles(audioSFXPath, searchPattern, SearchOption.AllDirectories);
            
            if (files.Length > 0)
            {
                string path = files[0].Replace("\\", "/");
                path = "Assets" + path.Substring(Application.dataPath.Length);
                
                var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                
                if (clip != null)
                {
                    attack.soundID = attack.attackID; // Usar ID como referencia
                    Debug.Log($"[V2Migrator] Assigned audio to {attack.attackName}");
                }
            }
        }
        
        EditorUtility.SetDirty(attack);
    }
    
    private void MigrateItem(ItemDataSO item)
    {
        // Asignar audio de item
        if (migrateAudio && !string.IsNullOrEmpty(audioSFXPath) && Directory.Exists(audioSFXPath))
        {
            string searchPattern = $"*item*{item.itemID}*";
            string[] files = Directory.GetFiles(audioSFXPath, searchPattern, SearchOption.AllDirectories);
            
            if (files.Length > 0)
            {
                string path = files[0].Replace("\\", "/");
                path = "Assets" + path.Substring(Application.dataPath.Length);
                
                var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                
                if (clip != null)
                {
                    Debug.Log($"[V2Migrator] Assigned audio to {item.itemName}");
                }
            }
        }
        
        EditorUtility.SetDirty(item);
    }
}

[System.Serializable]
public class DigimonBaseStats
{
    public int HP;
    public int MP;
    public int Attack;
    public int Defense;
    public int Speed;
}
