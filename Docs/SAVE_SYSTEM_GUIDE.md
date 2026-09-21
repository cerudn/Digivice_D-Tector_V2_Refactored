# Guía del Sistema de Guardado

## Visió·´·n General

El sistema de guardado del Digivice D-Tector V2 Refactored incluye:
- **Versionado autom á—tico** de schema
- **Migració·´·n entre versiones** (V2 → V3 → V4)
- **Checksum** para validació·´·n de integridad
- **Encriptació·´·n** b á—sica de datos
- **Auto-guardado** peri ó—dico

## Arquitectura

```
SavedGame (MonoBehaviour)
    ├── ISaveSystem (interfaz)
    ├── SaveData (datos)
    ├── EncryptedPlayerPrefs (cifrado)
    └── Migration System (migració·´·n)
```

## Uso B á—sico

### Inicializació·´·n

```csharp
// El sistema se inicializa autom á—ticamente en Awake
// Pero puedes verificar el estado:
if (SavedGame.Instance.HasSaveData())
{
    // Existe partida guardada
}
```

### Guardar Partida

```csharp
// Obtener datos actuales
SaveData currentData = GetCurrentSaveData();

// Guardar
SavedGame.Instance.Save(currentData);

// Suscribirse a eventos
SavedGame.Instance.OnSaveCompleted += () => {
    Console.WriteLine("Partida guardada correctamente");
};
```

### Cargar Partida

```csharp
// Cargar partida
SaveData loadedData = SavedGame.Instance.Load();

if (loadedData != null)
{
    // Usar datos cargados
    PlayerData player = loadedData.player;
    List<DigimonSaveData> team = loadedData.digimonTeam;
}
```

### Eliminar Partida

```csharp
SavedGame.Instance.DeleteSave();
```

## Estructura de Datos

### SaveData

```csharp
public class SaveData
{
    public int version;              // Versió·´·n del schema
    public string checksum;          // SHA-256 checksum
    public PlayerData player;        // Datos del jugador
    public List<DigimonSaveData> digimonTeam;  // Equipo
    public WorldData worldProgress;  // Progreso
    public List<ItemData> inventory; // Inventario
    public float totalTimePlayed;    // Tiempo total
    public DateTime lastSaveDate;    // Fecha ú—ltimo guardado
}
```

### PlayerData

```csharp
public class PlayerData
{
    public string playerName;        // Nombre
    public int playerID;             // ID ú—nico
    public int level;                // Nivel
    public int experience;           // Experiencia
    public int steps;                // Pasos dados
    public int money;                // Dinero
    public int currentWorldID;       // Mundo actual
    public Vector2Int currentPosition; // Posició·´·n
}
```

### DigimonSaveData

```csharp
public class DigimonSaveData
{
    public int digimonID;            // ID del Digimon
    public string nickname;          // Apodo
    public int level;                // Nivel
    public int experience;           // Experiencia
    public int hp;                   // HP actual
    public int mp;                   // MP actual
    public int attack, defense, speed; // Stats
    public int friendship;           // Amistad
    public List<int> attacks;        // IDs de ataques
    public EvolutionStage stage;     // Etapa
}
```

## Migració·´·n de Versiones

### C ó—mo Funciona

El sistema detecta autom á—ticamente la versió·´·n del guardado y lo migra a la versió·´·n actual:

```csharp
// En SavedGame.Load()
int savedVersion = PlayerPrefs.GetInt(VERSION_KEY, 0);

if (savedVersion < CurrentVersion)
{
    data = MigrateSaveData(data, savedVersion, CurrentVersion);
}
```

### Implementar Nueva Versió·´·n

Para a ñ—adir una nueva versió·´·n (ej. V5):

1. **Actualizar constante:**
```csharp
public static class GameConstants
{
    public const int SAVE_VERSION = 5; // Actualizar de 4 a 5
}
```

2. **A ñ—adir m é—todo de migració·´·n:**
```csharp
private SaveData MigrateV4toV5(SaveData v4Data)
{
    var v5Data = new SaveData
    {
        version = 5,
        player = v4Data.player,
        digimonTeam = v4Data.digimonTeam,
        // ... copiar resto de campos
        
        // Nuevos campos de V5
        newFeatureData = new NewFeatureData()
    };
    
    // Migrar datos existentes
    foreach (var digimon in v4Data.digimonTeam)
    {
        // Actualizar IDs si es necesario
        digimon.digimonID = MigrateDigimonID(digimon.digimonID, 4, 5);
    }
    
    return v5Data;
}

// Actualizar MigrateSaveData
private SaveData MigrateSaveData(SaveData oldData, int fromVersion, int toVersion)
{
    SaveData newData = oldData;
    
    if (fromVersion <= 2 && toVersion >= 3)
        newData = MigrateV2toV3(newData);
    
    if (fromVersion <= 3 && toVersion >= 4)
        newData = MigrateV3toV4(newData);
    
    if (fromVersion <= 4 && toVersion >= 5) // NUEVO
        newData = MigrateV4toV5(newData);
    
    newData.version = toVersion;
    
    return newData;
}
```

## Auto-Guardado

El sistema realiza auto-guardado autom á—tico en:
- Cada 60 segundos (configurable en `GameConstants.AUTO_SAVE_INTERVAL`)
- Después de ganar una batalla
- Después de una evolució·´·n importante (Champion+)

### Deshabilitar Auto-Guardado

```csharp
// En tu configuració·´·n
GameConstants.AUTO_SAVE_INTERVAL = 0; // Deshabilitar
```

## Seguridad

### Encriptació·´·n

Los datos se encriptan usando AES-256:

```csharp
// En EncryptedPlayerPrefs
using (Aes aes = Aes.Create())
{
    var keyDerivation = new Rfc2898DeriveBytes(
        ENCRYPTION_KEY,
        SALT,
        1000,
        HashAlgorithmName.SHA256
    );
    
    aes.Key = keyDerivation.GetBytes(32);
    aes.IV = keyDerivation.GetBytes(16);
    
    // ... encriptar/desencriptar
}
```

### Checksum

Se usa SHA-256 para validar integridad:

```csharp
private string CalculateChecksum(SaveData data)
{
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
```

## Debugging

### Ver Informació·´·n de Guardado

```csharp
// En Unity Editor, click derecho en SavedGame
SavedGame.Instance.ShowSaveInfo();

// Output:
// === SAVE INFO ===
// Has Save: True
// Current Version: 4
// Saved Version: 4
// Player: Entrenador
// Digimon Count: 3
// Total Time: 3600s
// ================
```

### Forzar Migració·´·n

Para testear migració·´·n:

```csharp
// Guardar con versió·´·n antigua
PlayerPrefs.SetInt("digivice_save_version", 2);

// Cargar (deber í—a migrar autom á—ticamente)
SaveData data = SavedGame.Instance.Load();
Console.WriteLine($"Versió·´·n actual: {data.version}"); // Deber í—a ser 4
```

## Mejores Pr á—cticas

### 1. Guardar Frecuentemente
```csharp
// Guardar en puntos clave:
- Despu é—s de batallas importantes
- Despu é—s de evoluciones
- Al entrar/salir de dungeons
- Antes de cerrar el juego
```

### 2. Validar Datos
```csharp
SaveData data = SavedGame.Instance.Load();

if (data != null && data.Validate())
{
    // Datos v á—lidos
}
else
{
    // Datos corruptos - crear nueva partida
    data = CreateNewSave();
}
```

### 3. Backup
```csharp
// Crear backup antes de migrar
string backupKey = "digivice_backup_" + DateTime.Now.Ticks;
PlayerPrefs.SetString(backupKey, PlayerPrefs.GetString("digivice_save_data"));
```

### 4. Manejar Errores
```csharp
try
{
    SavedGame.Instance.Save(data);
}
catch (Exception ex)
{
    Console.Error($"Error guardando: {ex.Message}");
    // Mostrar mensaje al usuario
}
```

## Troubleshooting

### Problema: Datos Corruptos

**S í—ntomas:**
- `OnSaveError` se dispara
- Checksum inv á—lido

**Solució·´·n:**
```csharp
SavedGame.Instance.OnSaveError += (message) => {
    Console.Error($"Error: {message}");
    
    // Intentar cargar backup
    if (PlayerPrefs.HasKey("digivice_backup"))
    {
        // Restaurar backup
    }
    else
    {
        // Crear nueva partida
        SavedGame.Instance.DeleteSave();
    }
};
```

### Problema: Migració·´·n Fallida

**S í—ntomas:**
- Exception durante Load()
- Datos inconsistentes

**Solució·´·n:**
```csharp
// Verificar logs
Console.WriteLine($"Migrando de {fromVersion} a {toVersion}");

// Asegurar que todos los campos se copian
// Verificar MigrateDigimonID() para IDs v á—lidos
```

## Referencias

- [Unity PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
- [AES Encryption](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes)
- [SHA256 Hash](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256)

---

**Ú·—ltima actualizació·´·n:** 21 de septiembre de 2026  
**Versió·´·n del sistema:** 4
