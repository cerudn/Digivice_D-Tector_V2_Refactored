using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Herramienta para auto-recortar spritesheets en sprites individuales.
/// </summary>
public class SpriteSheetAutoSplitter : EditorWindow
{
    private string spriteSheetPath = "Assets/Sprites/characters.png";
    private string outputFolder = "Assets/Sprites/Digimon";
    private int cellWidth = 64;
    private int cellHeight = 64;
    private int columns = 8;
    private int rows = 8;
    
    [MenuItem("Tools/Digivice/Split Sprite Sheets")]
    public static void ShowWindow()
    {
        var window = GetWindow<SpriteSheetAutoSplitter>("Sprite Sheet Splitter");
        window.minSize = new Vector2(400, 500);
    }
    
    void OnGUI()
    {
        GUILayout.Label("Sprite Sheet Auto-Splitter", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "Esta herramienta recorta autom á—ticamente spritesheets en sprites individuales.\n\n" +
            "Funciona con:\n" +
            "- characters.png (Digimon)\n" +
            "- animations.png (Animaciones)\n" +
            "- menus.png (UI)\n\n" +
            "Los sprites se guardar á—n como {ID}_{type}.png",
            MessageType.Info);
        
        GUILayout.Space(10);
        
        spriteSheetPath = EditorGUILayout.TextField("Sprite Sheet Path", spriteSheetPath);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        
        GUILayout.Space(10);
        
        GUILayout.Label("Configuració·´·n de Celda", EditorStyles.boldLabel);
        cellWidth = EditorGUILayout.IntField("Cell Width (pixels)", cellWidth);
        cellHeight = EditorGUILayout.IntField("Cell Height (pixels)", cellHeight);
        columns = EditorGUILayout.IntField("Columns", columns);
        rows = EditorGUILayout.IntField("Rows", rows);
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Split Sprite Sheet", GUILayout.Height(40)))
        {
            SplitSpriteSheet();
        }
        
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // Instrucciones
        GUILayout.Label("Instrucciones:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. Aseg ú—rate de tener el spritesheet en la ruta especificada\n" +
            "2. Ajusta el tama ñ—o de celda seg ú—n el spritesheet\n" +
            "3. Especifica columnas y filas\n" +
            "4. Click en 'Split Sprite Sheet'\n" +
            "5. Los sprites se guardar á—n en la carpeta de output",
            MessageType.None);
    }
    
    private void SplitSpriteSheet()
    {
        if (!File.Exists(spriteSheetPath))
        {
            EditorUtility.DisplayDialog("Error", 
                $"Sprite sheet no encontrado en:\n{spriteSheetPath}\n\n" +
                "Aseg ú—rate de haber copiado la carpeta Assets/Sprites/ del repositorio original.", 
                "OK");
            return;
        }
        
        // Crear carpeta de output si no existe
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            Debug.Log($"[SpriteSplitter] Created output folder: {outputFolder}");
        }
        
        // Cargar textura
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spriteSheetPath);
        
        if (texture == null)
        {
            EditorUtility.DisplayDialog("Error", 
                "No se pudo cargar el spritesheet.\nVerifica que es un archivo PNG v á—lido.", 
                "OK");
            return;
        }
        
        Debug.Log($"[SpriteSplitter] Splitting {texture.width}x{texture.height} sprite sheet...");
        
        int splitCount = 0;
        
        // Recortar sprites
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int digimonID = row * columns + col + 1;
                
                // Coordenadas del sprite
                int x = col * cellWidth;
                int y = texture.height - (row + 1) * cellHeight; // Unity usa Y invertido
                
                // Extraer sprite
                var spriteTexture = ExtractSprite(texture, x, y, cellWidth, cellHeight);
                
                if (spriteTexture != null)
                {
                    // Guardar sprite
                    string spritePath = $"{outputFolder}/{digimonID:D3}_idle.png";
                    SaveSprite(spriteTexture, spritePath);
                    splitCount++;
                    
                    Debug.Log($"[SpriteSplitter] Extracted sprite {digimonID:D3}");
                }
            }
        }
        
        AssetDatabase.Refresh();
        
        Debug.Log($"[SpriteSplitter] Split complete: {splitCount} sprites");
        EditorUtility.DisplayDialog("Split Complete", 
            $"Sprites extra í—dos: {splitCount}\n\nCarpeta: {outputFolder}", 
            "OK");
    }
    
    private Texture2D ExtractSprite(Texture2D source, int x, int y, int width, int height)
    {
        var pixels = source.GetPixels(x, y, width, height);
        
        var spriteTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        spriteTexture.SetPixels(pixels);
        spriteTexture.Apply();
        
        return spriteTexture;
    }
    
    private void SaveSprite(Texture2D sprite, string path)
    {
        // Convertir a PNG
        byte[] pngData = sprite.EncodeToPNG();
        
        // Guardar archivo
        File.WriteAllBytes(path, pngData);
        
        // Importar como sprite
        AssetDatabase.ImportAsset(path);
        
        // Configurar como sprite
        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        
        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.filterMode = FilterMode.Point; // Pixel art
            textureImporter.compressionQuality = 100;
            
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}
