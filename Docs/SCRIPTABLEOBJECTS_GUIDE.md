# Guía de ScriptableObjects

## Visió·´·n General

Los ScriptableObjects reemplazan el Database.cs hardcodeado y permiten:
- **Editar datos** desde Unity Inspector
- **A ñ—adir contenido** sin recompilar
- **Organizar assets** en carpetas
- **Referencias seguras** entre assets

## Estructura de Assets

```
Assets/
└── Data/
    ├── Digimon/
    │   ├── Agumon.asset
    │   ├── Gabumon.asset
    │   ├── Greymon.asset
    │   └── ...
    ├── Attacks/
    │   ├── Scratch.asset
    │   ├── PepperBreath.asset
    │   ├── BlueBlaster.asset
    │   └── ...
    ├── Items/
    │   ├── Potion.asset
    │   ├── Antidote.asset
    │   ├── Digivice.asset
    │   └── ...
    └── Evolutions/
        ├── AgumonToGreymon.asset
        └── ...
```

## Crear ScriptableObjects

### M é—todo 1: Unity Editor

1. **Click derecho en Project**
2. **Create > Digimon > Create Digimon**
3. **Nombrar asset** (ej: "Agumon")
4. **Rellenar datos** en Inspector

### M é—todo 2: C ó—digo

```csharp
[MenuItem("Digimon/Create New Digimon")]
private static void CreateNewDigimon()
{
    DigimonData digimon = ScriptableObject.CreateInstance<DigimonData>();
    
    string path = AssetDatabase.GenerateAssetPath("Assets/Data/Digimon/", "NewDigimon");
    AssetDatabase.CreateAsset(digimon, path);
    AssetDatabase.SaveAssets();
    
    Selection.activeObject = digimon;
}
```

## DigimonData

### Campos Principales

#### Informació·´·n B á—sica
- `digimonID`: ID ú—nico (usar constants de DigimonIDs)
- `digimonName`: Nombre visible
- `description`: Descripció·´·n lore
- `stage`: Etapa (Rookie, Champion, etc.)
- `element`: Tipo elemental

#### Stats
- `baseHP`, `baseMP`, `baseAttack`, `baseDefense`, `baseSpeed`: Stats a nivel 1
- `hpGrowthRate`, etc.: Multiplicadores de crecimiento

#### Sprites
- `idleSprite`, `walkSprite`, `attackSprite`, etc.
- `animationFrames`: Array para animaciones

#### Ataques
- `startingAttacks`: Ataques iniciales (nivel 1)
- `learnableAttacks`: Ataques por nivel

#### Evolució·´·n
- `preEvolution`: Digimon anterior
- `possibleEvolutions`: Array de evoluciones posibles

### Uso en Có—digo

```csharp
// Obtener desde DatabaseSO
DigimonData agumon = DatabaseSO.Instance.GetDigimon(DigimonIDs.AGUMON);

// Calcular stats a nivel 50
DigimonStats stats = agumon.GetStatsAtLevel(50);
Console.WriteLine($"HP: {stats.HP}, Attack: {stats.Attack}");

// Verificar evolució·´·n
if (agumon.CanEvolve(digimon))
{
    DigimonData nextForm = agumon.GetEvolution(digimon);
    Console.WriteLine($"Puede evolucionar a: {nextForm.digimonName}");
}
```

## AttackData

### Campos Principales

- `attackID`: ID ú—nico
- `attackName`: Nombre
- `description`: Descripció·´·n
- `power`: Poder base
- `accuracy`: Precisió·´·n (0-100)
- `mpCost`: Coste de MP
- `element`: Tipo
- `attackType`: Physical, Special, Status
- `secondaryEffect`: Efecto secundario
- `secondaryEffectChance`: Probabilidad (0-1)

### Uso en Có—digo

```csharp
AttackData pepperBreath = DatabaseSO.Instance.GetAttack(2);

if (pepperBreath.IsSpecial())
{
    int damage = pepperBreath.CalculateBaseDamage(specialAttack, specialDefense);
    Console.WriteLine($"Dañ·´·o base: {damage}");
}
```

## EvolutionRequirement

### Campos Principales

- `targetDigimon`: Digimon objetivo
- `minLevel`: Nivel mí—nimo
- `minFriendship`: Friendship mí—nimo
- `requiredItemIDs`: Items necesarios
- `specialCondition`: Condició·´·n especial
- `conditionValue`: Valor de la condició·´·n
- `successRate`: Probabilidad de é—xito

### Condiciones Especiales

```csharp
public enum EvolutionCondition
{
    NONE = 0,
    TIME_OF_DAY = 1,      // D í—a/Noche
    BATTLES_WON = 2,      // Batallas ganadas
    STEPS_WALKED = 3,     // Pasos caminados
    SPIRIT_TYPE = 4,      // Tipo de esp í—ritu
    PARTNER_DIGIMON = 5,  // Digimon partner espec í—fico
}
```

### Uso en Có—digo

```csharp
EvolutionRequirement evo = digimonData.possibleEvolutions[0];

if (evo.MeetsRequirements(currentDigimon))
{
    Console.WriteLine($"Puede evolucionar a {evo.targetDigimon.digimonName}");
    Console.WriteLine($"Requisitos: {evo.GetRequirementsDescription()}");
}
```

## DatabaseSO

### Configuració·´·n

1. **Crear GameObject** en escena (ej: "Database")
2. **A ñ—adir componente** `DatabaseSO`
3. **Asignar arrays** en Inspector:
   - `allDigimon`: Array con todos los DigimonData
   - `allAttacks`: Array con todos los AttackData
   - `allItems`: Array con todos los ItemDataSO

### Uso

```csharp
// Inicializació·´·n autom á—tica en Awake
// No necesitas llamar a nada

// Obtener Digimon
DigimonData digimon = DatabaseSO.Instance.GetDigimon(DigimonIDs.AGUMON);

// B ú—squedas
DigimonData[] fireTypes = DatabaseSO.Instance.GetDigimonByElement(ElementType.Fire);
AttackData[] physicalAttacks = DatabaseSO.Instance.GetAttacksByType(AttackType.PHYSICAL);

// Verificar existencia
bool exists = DatabaseSO.Instance.DigimonExists(DigimonIDs.WAR_GREYMON);
```

## Migració·´·n desde Database.cs

### Antes (Database.cs)

```csharp
// Hardcodeado
public static DigimonBaseStats GetDigimonBaseStats(int digimonID)
{
    if (digimonID == DigimonIDs.AGUMON)
    {
        return new DigimonBaseStats { HP = 50, Attack = 60, ... };
    }
    // ... má—s ifs
}
```

### Después (DatabaseSO + ScriptableObjects)

```csharp
// Cargado desde asset
DigimonData digimon = DatabaseSO.Instance.GetDigimon(DigimonIDs.AGUMON);
DigimonStats stats = digimon.GetStatsAtLevel(level);
```

## Ventajas

### 1. **Sin Recompilar**
- A ñ—adir nuevo Digimon = Crear asset
- Balancear stats = Editar en Inspector

### 2. **Referencias Visuales**
- Ver sprites en Inspector
- Arrastrar y soltar referencias

### 3. **Validació·´·n Autom á—tica**
- Unity verifica tipos
- No más IDs inv á—lidos

### 4. **Organizació·´·n**
- Assets en carpetas
- F á—cil de navegar

## Custom Editor

El `DigimonDataEditor.cs` proporciona:
- **Foldouts** para organizar campos
- **Sliders** para stats
- **Botones** para acciones
- **Validació·´·n** en tiempo real

### Uso

1. **Seleccionar DigimonData** en Project
2. **Inspector personalizado** aparece autom á—ticamente
3. **Editar campos** visualmente

## Mejores Pr á—cticas

### 1. **Nomenclatura**
```
Assets/Data/Digimon/Agumon.asset
Assets/Data/Attacks/PepperBreath.asset
```

### 2. **IDs Ú—nicos**
- Usar `DigimonIDs.cs` constants
- Verificar duplicados

### 3. **Referencias**
- Asignar en Inspector
- Verificar null references

### 4. **Version Control**
- Assets son YAML legible
- Merge conflicts manejables

## Troubleshooting

### Problema: Asset no aparece

**Solució·´·n:**
```csharp
// Verificar que el script tiene [CreateAssetMenu]
[CreateAssetMenu(fileName = "New Digimon", menuName = "Digimon/Create Digimon")]
public class DigimonData : ScriptableObject { ... }
```

### Problema: Referencias rotas

**Solució·´·n:**
- Reasignar en Inspector
- Verificar que el asset existe
- Reimportar asset

### Problema: DatabaseSO.Instance es null

**Solució·´·n:**
```csharp
// Asegurar que hay un GameObject con DatabaseSO en escena
// O crearlo program á—ticamente:
var db = new GameObject("Database").AddComponent<DatabaseSO>();
DontDestroyOnLoad(db.gameObject);
```

## Ejemplo Completo

### Crear Agumon

1. **Right-click > Create > Digimon > Create Digimon**
2. **Nombrar** "Agumon"
3. **Rellenar**:
   - ID: 1
   - Name: Agumon
   - Stage: Rookie
   - Element: Fire
   - Base HP: 50
   - Base Attack: 60
   - etc.
4. **Asignar sprites**
5. **A ñ—adir ataques**
6. **Configurar evoluciones**
7. **Guardar**

### Usar en Có—digo

```csharp
void Start()
{
    // Cargar Agumon
    DigimonData agumon = DatabaseSO.Instance.GetDigimon(DigimonIDs.AGUMON);
    
    // Crear instancia de juego
    Digimon playerDigimon = new Digimon(agumon.digimonID, level: 1);
    
    // Calcular stats
    DigimonStats stats = agumon.GetStatsAtLevel(playerDigimon.Level);
    playerDigimon.MaxHP = stats.HP;
    playerDigimon.Attack = stats.Attack;
    
    Console.WriteLine($"{agumon.digimonName} creado con {stats.HP} HP");
}
```

## Referencias

- [Unity ScriptableObject Docs](https://docs.unity3d.com/ScriptReference/ScriptableObject.html)
- [Custom Editor Guide](https://docs.unity3d.com/Manual/EditingCustomInspector.html)

---

**Ú·—ltima actualizació·´·n:** 21 de septiembre de 2026  
**Versió·´·n:** 1.0
