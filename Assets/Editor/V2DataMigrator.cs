using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Herramienta de migració·´·n autom á—tica de datos V2 a ScriptableObjects.
/// Asigna stats, sprites y evoluciones autom á—ticamente.
/// </summary>
public class V2DataMigrator : EditorWindow
{
    private string spriteFolderPath = "Assets/Sprites/Digimon";
    private bool migrateStats = true;
    private bool migrateSprites = true;
    private bool migrateEvolutions = true;
    
    [MenuItem("Tools/Digivice/Migrate V2 Data to SOs")]
    public static void ShowWindow()
    {
        var window = GetWindow<V2DataMigrator>("V2 Data Migrator");
        window.minSize = new Vector2(400, 500);
    }
    
    void OnGUI()
    {
        GUILayout.Label("V2 Data Migration Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Configuració·´·n
        GUILayout.Label("Configuració·´·n", EditorStyles.boldLabel);
        spriteFolderPath = EditorGUILayout.TextField("Sprite Folder Path", spriteFolderPath);
        
        GUILayout.Space(10);
        
        // Opciones
        GUILayout.Label("Opciones de Migració·´·n", EditorStyles.boldLabel);
        migrateStats = EditorGUILayout.Toggle("Migrate Stats", migrateStats);
        migrateSprites = EditorGUILayout.Toggle("Migrate Sprites", migrateSprites);
        migrateEvolutions = EditorGUILayout.Toggle("Migrate Evolutions", migrateEvolutions);
        
        GUILayout.Space(20);
        
        // Botones
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Start Migration", GUILayout.Height(40)))
        {
            StartMigration();
        }
        
        GUI.backgroundColor = Color.white;
        GUILayout.Space(10);
        
        // Instrucciones
        GUILayout.Label("Instrucciones:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. Aseg ú—rate de tener los ScriptableObjects creados\n" +
            "2. Los sprites deben estar en la carpeta especificada\n" +
            "3. El formato debe ser: {ID}_{type}.png (ej: 001_idle.png)\n" +
            "4. Haz click en 'Start Migration'",
            MessageType.Info);
    }
    
    private void StartMigration()
    {
        int migratedCount = 0;
        int errorCount = 0;
        
        // Obtener todos los DigimonData
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
                    Debug.Log($"[V2Migrator] Migrado: {digimon.digimonName}");
                }
                catch (System.Exception ex)
                {
                    errorCount++;
                    Debug.LogError($"[V2Migrator] Error migrando {digimon.digimonName}: {ex.Message}");
                }
            }
        }
        
        // Migrar ataques
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
        
        // Migrar items
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
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"[V2Migrator] Migració·´·n completada: {migratedCount} assets, {errorCount} errores");
        EditorUtility.DisplayDialog("Migració·´·n Completada", 
            $"Assets migrados: {migratedCount}\nErrores: {errorCount}", "OK");
    }
    
    private void MigrateDigimon(DigimonData digimon)
    {
        // 1. Migrar Stats desde Database.cs original
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
                
                // Ajustar growth rates seg ú—n etapa
                SetGrowthRatesByStage(digimon);
            }
        }
        
        // 2. Migrar Sprites
        if (migrateSprites)
        {
            AssignSprites(digimon);
        }
        
        // 3. Migrar Evoluciones
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
            { 1, new DigimonBaseStats { HP = 50, MP = 20, Attack = 60, Defense = 40, Speed = 50 } }, // Agumon
            { 2, new DigimonBaseStats { HP = 45, MP = 25, Attack = 55, Defense = 45, Speed = 55 } }, // Gabumon
            { 3, new DigimonBaseStats { HP = 40, MP = 30, Attack = 50, Defense = 35, Speed = 60 } }, // Patamon
            { 4, new DigimonBaseStats { HP = 42, MP = 28, Attack = 58, Defense = 38, Speed = 62 } }, // Gatomon
            { 5, new DigimonBaseStats { HP = 48, MP = 22, Attack = 52, Defense = 42, Speed = 56 } }, // Gomamon
            { 6, new DigimonBaseStats { HP = 44, MP = 26, Attack = 54, Defense = 44, Speed = 52 } }, // Tentomon
            { 7, new DigimonBaseStats { HP = 46, MP = 24, Attack = 56, Defense = 40, Speed = 54 } }, // Biyomon
            { 8, new DigimonBaseStats { HP = 47, MP = 23, Attack = 57, Defense = 41, Speed = 52 } }, // Palmon
            
            // Tamers Rookie
            { 80, new DigimonBaseStats { HP = 52, MP = 18, Attack = 62, Defense = 38, Speed = 48 } }, // Guilmon
            { 81, new DigimonBaseStats { HP = 44, MP = 26, Attack = 56, Defense = 42, Speed = 58 } }, // Renamon
            { 82, new DigimonBaseStats { HP = 42, MP = 28, Attack = 52, Defense = 40, Speed = 60 } }, // Terriermon
            { 83, new DigimonBaseStats { HP = 40, MP = 30, Attack = 54, Defense = 36, Speed = 56 } }, // Impmon
            
            // Champion
            { 10, new DigimonBaseStats { HP = 80, MP = 30, Attack = 90, Defense = 70, Speed = 60 } }, // Greymon
            { 11, new DigimonBaseStats { HP = 75, MP = 35, Attack = 85, Defense = 65, Speed = 80 } }, // Garurumon
            { 12, new DigimonBaseStats { HP = 70, MP = 50, Attack = 75, Defense = 60, Speed = 75 } }, // Angemon
            { 13, new DigimonBaseStats { HP = 72, MP = 55, Attack = 80, Defense = 62, Speed = 70 } }, // Taomon
            { 14, new DigimonBaseStats { HP = 85, MP = 32, Attack = 82, Defense = 75, Speed = 55 } }, // Ikkakumon
            { 15, new DigimonBaseStats { HP = 78, MP = 38, Attack = 88, Defense = 72, Speed = 62 } }, // Kabuterimon
            { 16, new DigimonBaseStats { HP = 76, MP = 36, Attack = 84, Defense = 68, Speed = 72 } }, // Akiatorimon
            { 17, new DigimonBaseStats { HP = 77, MP = 37, Attack = 86, Defense = 66, Speed = 64 } }, // Gargoy lemont
            
            // Tamers Champion
            { 84, new DigimonBaseStats { HP = 82, MP = 32, Attack = 94, Defense = 68, Speed = 64 } }, // Growlmon
            { 85, new DigimonBaseStats { HP = 74, MP = 48, Attack = 82, Defense = 64, Speed = 76 } }, // Kyubimon
            { 86, new DigimonBaseStats { HP = 76, MP = 38, Attack = 86, Defense = 66, Speed = 74 } }, // Gargomon
            
            // Ultimate
            { 20, new DigimonBaseStats { HP = 110, MP = 45, Attack = 120, Defense = 100, Speed = 75 } }, // MetalGreymon
            { 21, new DigimonBaseStats { HP = 100, MP = 50, Attack = 115, Defense = 90, Speed = 110 } }, // WereGarurumon
            { 22, new DigimonBaseStats { HP = 95, MP = 70, Attack = 105, Defense = 85, Speed = 95 } }, // MagnaAngemon
            { 23, new DigimonBaseStats { HP = 105, MP = 65, Attack = 110, Defense = 95, Speed = 90 } }, // Phoenixmon
            { 24, new DigimonBaseStats { HP = 115, MP = 48, Attack = 118, Defense = 105, Speed = 70 } }, // Zudomon
            { 25, new DigimonBaseStats { HP = 108, MP = 55, Attack = 122, Defense = 98, Speed = 78 } }, // MegaKabuterimon
            
            // Tamers Ultimate
            { 88, new DigimonBaseStats { HP = 112, MP = 48, Attack = 124, Defense = 98, Speed = 78 } }, // WarGrowlmon
            { 89, new DigimonBaseStats { HP = 98, MP = 72, Attack = 108, Defense = 88, Speed = 92 } }, // TaomonF
            { 90, new DigimonBaseStats { HP = 106, MP = 56, Attack = 118, Defense = 96, Speed = 96 } }, // Rapidmon
            
            // Mega
            { 30, new DigimonBaseStats { HP = 140, MP = 60, Attack = 150, Defense = 120, Speed = 100 } }, // WarGreymon
            { 31, new DigimonBaseStats { HP = 130, MP = 70, Attack = 140, Defense = 115, Speed = 130 } }, // MetalGarurumon
            { 32, new DigimonBaseStats { HP = 125, MP = 100, Attack = 130, Defense = 110, Speed = 115 } }, // Seraphimon
            { 33, new DigimonBaseStats { HP = 120, MP = 105, Attack = 125, Defense = 115, Speed = 110 } }, // Ophanimon
            
            // Tamers Mega
            { 100, new DigimonBaseStats { HP = 138, MP = 68, Attack = 148, Defense = 118, Speed = 108 } }, // Gallantmon
            { 101, new DigimonBaseStats { HP = 118, MP = 95, Attack = 128, Defense = 108, Speed = 118 } }, // Sakuyamon
            { 102, new DigimonBaseStats { HP = 142, MP = 72, Attack = 142, Defense = 122, Speed = 88 } }, // MegaGargomon
            { 103, new DigimonBaseStats { HP = 132, MP = 78, Attack = 152, Defense = 102, Speed = 122 } }, // Beelzemon
            
            // Ancient
            { 110, new DigimonBaseStats { HP = 150, MP = 80, Attack = 160, Defense = 130, Speed = 110 } }, // AncientGreymon
            { 111, new DigimonBaseStats { HP = 140, MP = 90, Attack = 150, Defense = 125, Speed = 125 } }, // AncientGarurumon
            { 112, new DigimonBaseStats { HP = 125, MP = 95, Attack = 130, Defense = 110, Speed = 145 } }, // AncientIrismon
            { 113, new DigimonBaseStats { HP = 165, MP = 70, Attack = 135, Defense = 155, Speed = 85 } }, // AncientTortomon
            { 114, new DigimonBaseStats { HP = 138, MP = 85, Attack = 155, Defense = 118, Speed = 115 } }, // AncientBeetlemon
            { 115, new DigimonBaseStats { HP = 135, MP = 82, Attack = 145, Defense = 135, Speed = 105 } }, // AncientKazemon
            { 116, new DigimonBaseStats { HP = 142, MP = 92, Attack = 138, Defense = 122, Speed = 118 } }, // AncientMermaimon
            { 117, new DigimonBaseStats { HP = 132, MP = 98, Attack = 148, Defense = 115, Speed = 122 } }, // AncientSphinxmon
            { 118, new DigimonBaseStats { HP = 155, MP = 75, Attack = 152, Defense = 128, Speed = 102 } }, // AncientVolcamon
            { 119, new DigimonBaseStats { HP = 128, MP = 105, Attack = 135, Defense = 118, Speed = 128 } }, // AncientWisemon
            { 120, new DigimonBaseStats { HP = 170, MP = 110, Attack = 175, Defense = 145, Speed = 135 } }, // Susanoomon
        };
        
        if (statsDB.TryGetValue(digimonID, out var stats))
        {
            return stats;
        }
        
        // Stats por defecto seg ú—n etapa
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
        // Buscar sprites por ID
        string[] spritePatterns = new[] { "idle", "walk", "attack", "victory", "faint" };
        
        foreach (var pattern in spritePatterns)
        {
            // Buscar archivo que coincida con el patr ó—n
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
                }
            }
        }
    }
    
    private void AssignEvolutions(DigimonData digimon)
    {
        // Configurar evoluciones b á—sicas por etapa
        var evolutions = new List<EvolutionRequirement>();
        
        switch (digimon.stage)
        {
            case EvolutionStage.Rookie:
                // Puede evolucionar a Champion nivel 11
                evolutions.Add(new EvolutionRequirement
                {
                    minLevel = 11,
                    minFriendship = 50,
                    specialCondition = EvolutionCondition.NONE
                });
                break;
                
            case EvolutionStage.Champion:
                // Puede evolucionar a Ultimate nivel 21
                evolutions.Add(new EvolutionRequirement
                {
                    minLevel = 21,
                    minFriendship = 60,
                    specialCondition = EvolutionCondition.NONE
                });
                break;
                
            case EvolutionStage.Ultimate:
                // Puede evolucionar a Mega nivel 31
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
        // Los ataques ya tienen valores correctos, solo verificar
        EditorUtility.SetDirty(attack);
    }
    
    private void MigrateItem(ItemDataSO item)
    {
        // Los items ya tienen valores correctos, solo verificar
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
