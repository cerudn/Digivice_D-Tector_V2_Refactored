using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor personalizado para DigimonData.
/// Mejora la experiencia de creació·´·n y edició·´·n de Digimon.
/// </summary>
[CustomEditor(typeof(DigimonData))]
public class DigimonDataEditor : Editor
{
    private bool showBasicInfo = true;
    private bool showStats = true;
    private bool showGrowth = true;
    private bool showSprites = true;
    private bool showAttacks = true;
    private bool showEvolution = true;
    private bool showMetadata = true;
    
    public override void OnInspectorGUI()
    {
        DigimonData digimon = (DigimonData)target;
        
        // Header
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"{digimon.digimonName}", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"ID: {digimon.digimonID} | Stage: {digimon.stage} | Element: {digimon.element}");
        EditorGUILayout.Space();
        
        // Basic Info
        showBasicInfo = EditorGUILayout.Foldout(showBasicInfo, "Informació·´·n B á—sica", true);
        if (showBasicInfo)
        {
            EditorGUI.indentLevel++;
            digimon.digimonID = EditorGUILayout.IntField("Digimon ID", digimon.digimonID);
            digimon.digimonName = EditorGUILayout.TextField("Nombre", digimon.digimonName);
            digimon.description = EditorGUILayout.TextArea(digimon.description, GUILayout.Height(60));
            digimon.stage = (EvolutionStage)EditorGUILayout.EnumPopup("Etapa", digimon.stage);
            digimon.element = (ElementType)EditorGUILayout.EnumPopup("Elemento", digimon.element);
            EditorGUI.indentLevel--;
        }
        
        // Stats
        showStats = EditorGUILayout.Foldout(showStats, "Stats Base", true);
        if (showStats)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Stats a Nivel 1:", EditorStyles.miniLabel);
            
            digimon.baseHP = EditorGUILayout.IntSlider("HP", digimon.baseHP, 10, 200);
            digimon.baseMP = EditorGUILayout.IntSlider("MP", digimon.baseMP, 5, 100);
            digimon.baseAttack = EditorGUILayout.IntSlider("Attack", digimon.baseAttack, 20, 150);
            digimon.baseDefense = EditorGUILayout.IntSlider("Defense", digimon.baseDefense, 20, 150);
            digimon.baseSpeed = EditorGUILayout.IntSlider("Speed", digimon.baseSpeed, 20, 150);
            
            EditorGUI.indentLevel--;
        }
        
        // Growth Rates
        showGrowth = EditorGUILayout.Foldout(showGrowth, "Tasas de Crecimiento", true);
        if (showGrowth)
        {
            EditorGUI.indentLevel++;
            digimon.hpGrowthRate = EditorGUILayout.Slider("HP Growth", digimon.hpGrowthRate, 1f, 3f);
            digimon.mpGrowthRate = EditorGUILayout.Slider("MP Growth", digimon.mpGrowthRate, 0.5f, 2f);
            digimon.attackGrowthRate = EditorGUILayout.Slider("Attack Growth", digimon.attackGrowthRate, 0.5f, 2f);
            digimon.defenseGrowthRate = EditorGUILayout.Slider("Defense Growth", digimon.defenseGrowthRate, 0.5f, 2f);
            digimon.speedGrowthRate = EditorGUILayout.Slider("Speed Growth", digimon.speedGrowthRate, 0.5f, 2f);
            EditorGUI.indentLevel--;
        }
        
        // Sprites
        showSprites = EditorGUILayout.Foldout(showSprites, "Sprites", true);
        if (showSprites)
        {
            EditorGUI.indentLevel++;
            digimon.idleSprite = (Sprite)EditorGUILayout.ObjectField("Idle Sprite", digimon.idleSprite, typeof(Sprite), false);
            digimon.walkSprite = (Sprite)EditorGUILayout.ObjectField("Walk Sprite", digimon.walkSprite, typeof(Sprite), false);
            digimon.attackSprite = (Sprite)EditorGUILayout.ObjectField("Attack Sprite", digimon.attackSprite, typeof(Sprite), false);
            digimon.victorySprite = (Sprite)EditorGUILayout.ObjectField("Victory Sprite", digimon.victorySprite, typeof(Sprite), false);
            digimon.faintSprite = (Sprite)EditorGUILayout.ObjectField("Faint Sprite", digimon.faintSprite, typeof(Sprite), false);
            EditorGUI.indentLevel--;
        }
        
        // Attacks
        showAttacks = EditorGUILayout.Foldout(showAttacks, "Ataques", true);
        if (showAttacks)
        {
            EditorGUI.indentLevel++;
            
            EditorGUILayout.LabelField("Starting Attacks:");
            EditorGUI.indentLevel++;
            digimon.startingAttacks = (AttackData[])EditorGUILayout.ObjectField(digimon.startingAttacks, typeof(AttackData), true);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Learnable Attacks:");
            
            if (digimon.learnableAttacks == null)
            {
                digimon.learnableAttacks = new LearnableAttackData[0];
            }
            
            EditorGUI.indentLevel++;
            for (int i = 0; i < digimon.learnableAttacks.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Level {digimon.learnableAttacks[i].level}:", GUILayout.Width(80));
                digimon.learnableAttacks[i].attack = (AttackData)EditorGUILayout.ObjectField(digimon.learnableAttacks[i].attack, typeof(AttackData), false);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
            
            EditorGUI.indentLevel--;
        }
        
        // Evolution
        showEvolution = EditorGUILayout.Foldout(showEvolution, "Evolució·´·n", true);
        if (showEvolution)
        {
            EditorGUI.indentLevel++;
            digimon.preEvolution = (DigimonData)EditorGUILayout.ObjectField("Pre-Evolution", digimon.preEvolution, typeof(DigimonData), false);
            
            EditorGUILayout.LabelField("Possible Evolutions:");
            EditorGUI.indentLevel++;
            
            if (digimon.possibleEvolutions == null)
            {
                digimon.possibleEvolutions = new EvolutionRequirement[0];
            }
            
            for (int i = 0; i < digimon.possibleEvolutions.Length; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Evolution {i + 1}", EditorStyles.boldLabel);
                digimon.possibleEvolutions[i].targetDigimon = (DigimonData)EditorGUILayout.ObjectField(
                    "Target", digimon.possibleEvolutions[i].targetDigimon, typeof(DigimonData), false);
                digimon.possibleEvolutions[i].minLevel = EditorGUILayout.IntField("Min Level", digimon.possibleEvolutions[i].minLevel);
                digimon.possibleEvolutions[i].minFriendship = EditorGUILayout.IntSlider("Min Friendship", 
                    digimon.possibleEvolutions[i].minFriendship, 0, 100);
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
        
        // Metadata
        showMetadata = EditorGUILayout.Foldout(showMetadata, "Metadata", true);
        if (showMetadata)
        {
            EditorGUI.indentLevel++;
            digimon.height = EditorGUILayout.FloatField("Altura (m)", digimon.height);
            digimon.weight = EditorGUILayout.FloatField("Peso (kg)", digimon.weight);
            digimon.family = EditorGUILayout.TextField("Familia", digimon.family);
            digimon.firstAppearanceYear = EditorGUILayout.IntField("Primera Aparició·´·n", digimon.firstAppearanceYear);
            EditorGUI.indentLevel--;
        }
        
        // Apply changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    
    [MenuItem("Digimon/Create New Digimon")]
    private static void CreateNewDigimon()
    {
        DigimonData digimon = ScriptableObject.CreateInstance<DigimonData>();
        
        string path = AssetDatabase.GenerateAssetPath("Assets/Data/Digimon/", "NewDigimon");
        AssetDatabase.CreateAsset(digimon, path);
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = digimon;
        
        Debug.Log($"Digimon creado en: {path}");
    }
}
