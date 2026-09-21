# Plan de Migració·´·n a V4

## Visió·´·n General

Este documento describe el plan detallado para migrar el proyecto refactorizado V2 a la versió·´·n V4 del Digivice D-Tector.

## Estado Actual

### ✅ FASE 1A - Auditorí·´­a V2 (COMPLETADA)
- [x] Mapeo de arquitectura V2
- [x] Identificació·´·n de deuda té—cnica
- [x] Documentació·´·n de flujo de datos
- [x] An á—lisis de scripts principales

### ✅ FASE 1B - Refactorizació·´·n V2 (COMPLETADA)
- [x] Extracció·´·n de interfaces (7 interfaces)
- [x] Constants centralizados (5 archivos)
- [x] Sistema de eventos global (18 eventos)
- [x] GameManager refactorizado
- [x] Patró·´·n State implementado
- [x] Modelos de datos creados
- [x] Database.cs con cacheo

### ⚪ FASE 2 - Preparació·´·n V4 (PENDIENTE)
- [ ] An á—lisis de diferencias V2 vs V4
- [ ] Mapeo de entidades V2 → V4
- [ ] Identificació·´·n de mec á—nicas obsoletas
- [ ] Diseñ·´·o de schema de guardado V4

### ⚪ FASE 3 - Migració·´·n de Datos (PENDIENTE)
- [ ] Migrar Database.cs a ScriptableObjects
- [ ] Crear assets de Digimon V4
- [ ] Crear assets de ataques V4
- [ ] Crear assets de items V4
- [ ] Migrar datos de evolució·´·n

### ⚪ FASE 4 - Migració·´·n de Mec á—nicas (PENDIENTE)
- [ ] Actualizar sistema de combate V4
- [ ] Implementar nuevas evoluciones V4
- [ ] Actualizar sistema de pasos/pedometro
- [ ] Implementar nuevas apps V4

### ⚪ FASE 5 - Testing y Validació·´·n (PENDIENTE)
- [ ] Unit tests para ló·´·gica
- [ ] Integration tests
- [ ] Playtesting
- [ ] Bug fixing

## Roadmap Detallado

### Semana 1-2: An á—lisis y Diseñ·´·o

#### Tareas:
1. **An á—lisis comparativo V2 vs V4**
   - Listar todos los Digimon de V2
   - Listar todos los Digimon de V4
   - Identificar nuevos Digimon en V4
   - Identificar Digimon eliminados en V4

2. **Mapeo de mec á—nicas**
   - Combat system V2 vs V4
   - Evolution system V2 vs V4
   - Spirit system V2 vs V4
   - App system V2 vs V4

3. **Diseñ·´·o de arquitectura V4**
   - Definir nuevos ScriptableObjects
   - Definir nuevos eventos
   - Definir nuevos estados

#### Deliverables:
- `V2_V4_COMPARISON.md`
- `V4_ARCHITECTURE_DESIGN.md`
- `SCRIPTABLEOBJECT_SPEC.md`

### Semana 3-4: Migració·´·n de Datos

#### Tareas:
1. **Crear estructura de ScriptableObjects**
```csharp
// DigimonData.cs
[CreateAssetMenu(fileName = "New Digimon", menuName = "Digimon/Create Digimon")]
public class DigimonData : ScriptableObject {
    public int digimonID;
    public string digimonName;
    public EvolutionStage stage;
    public ElementType element;
    public DigimonBaseStats baseStats;
    public List<LearnableAttackData> learnableAttacks;
    public List<EvolutionRequirement> evolutions;
}
```

2. **Migrar Database.cs**
   - Extraer todos los datos hardcodeados
   - Crear assets .asset para cada entidad
   - Actualizar Database.cs para leer desde ScriptableObjects

3. **Crear assets V4**
   - ~50-100 DigimonData assets
   - ~100-200 AttackData assets
   - ~50-100 ItemData assets
   - ~20-30 EvolutionData assets

#### Deliverables:
- Carpeta `Assets/Data/Digimon/` con todos los assets
- Carpeta `Assets/Data/Attacks/` con todos los assets
- Carpeta `Assets/Data/Items/` con todos los assets
- `Database.cs` refactorizado

### Semana 5-6: Migració·´·n de Mec á—nicas

#### Tareas:
1. **Sistema de combate V4**
   - Revisar fó—rmulas de daño V2
   - Implementar fó—rmulas V4 (si son diferentes)
   - Actualizar tipos elementales
   - Actualizar tabla de ventajas

2. **Sistema de evolució·´·n V4**
   - Implementar nuevas condiciones de evolució·´·n
   - Añ·´·adir evoluciones de V4
   - Implementar Spirit Evolution V4
   - Implementar Double Spirit (si existe en V4)

3. **Sistema de pasos V4**
   - Actualizar Pedometer para V4
   - Implementar nuevas mec á—nicas basadas en pasos
   - Añ·´·adir huevos V4

4. **Apps V4**
   - Revisar apps de V2
   - Identificar apps nuevas de V4
   - Implementar apps faltantes

#### Deliverables:
- `CombatManager_V4.cs`
- `EvolutionManager_V4.cs`
- `Pedometer_V4.cs`
- Apps actualizadas

### Semana 7-8: Testing y Optimizació·´·n

#### Tareas:
1. **Unit Testing**
   - Tests para cálculos de combate
   - Tests para evolució·´·n
   - Tests para guardado/carga
   - Tests para migració·´·n de saves

2. **Integration Testing**
   - Flujo completo de juego
   - Transiciones entre estados
   - Eventos globales

3. **Performance Testing**
   - Profiling de memoria
   - Optimizació·´·n de carga de assets
   - Reducció·´·n de GC allocs

4. **Bug Fixing**
   - Fix de bugs reportados
   - Balanceo de stats
   - Ajuste de dificultad

#### Deliverables:
- Carpeta `Tests/` con todos los tests
- `PERFORMANCE_REPORT.md`
- `BUG_FIX_LOG.md`

## Migració·´·n de Saves V2 → V4

### Estrategia

El sistema de guardado ya incluye versionado. Para migrar saves V2 a V4:

```csharp
public class SavedGame : MonoBehaviour, ISaveSystem {
    public int CurrentVersion => 4;
    
    public SaveData Load() {
        int version = PlayerPrefs.GetInt("save_version", 0);
        
        if (version == 0) {
            return CreateNewSave();
        }
        
        SaveData data = LoadSaveData();
        
        // Migració·´·n autom á—tica
        if (version < CurrentVersion) {
            data = MigrateSaveData(data, version, CurrentVersion);
        }
        
        return data;
    }
    
    private SaveData MigrateSaveData(SaveData oldData, int fromVersion, int toVersion) {
        var newData = new SaveData { version = toVersion };
        
        // V2 → V3
        if (fromVersion <= 2 && toVersion >= 3) {
            newData = MigrateV2toV3(oldData);
        }
        
        // V3 → V4
        if (fromVersion <= 3 && toVersion >= 4) {
            newData = MigrateV3toV4(newData);
        }
        
        return newData;
    }
    
    private SaveData MigrateV2toV3(SaveData v2Data) {
        var v3Data = new SaveData { version = 3 };
        
        // Migrar jugador
        v3Data.player = v2Data.player;
        
        // Migrar Digimon (posibles cambios de ID)
        v3Data.digimonTeam = new List<DigimonSaveData>();
        foreach (var digimon in v2Data.digimonTeam) {
            var newDigimon = new DigimonSaveData {
                digimonID = MigrateDigimonID(digimon.digimonID),
                level = digimon.level,
                // ... resto de campos
            };
            v3Data.digimonTeam.Add(newDigimon);
        }
        
        return v3Data;
    }
    
    private int MigrateDigimonID(int v2ID) {
        // Mapeo V2 → V4
        // Ejemplo: Agumon V2 (ID=1) → Agumon V4 (ID=1)
        // Algunos IDs pueden cambiar
        return v2ID; // Placeholder
    }
}
```

## Riesgos y Mitigaciones

### Riesgo 1: Pé—rdida de datos en migració·´·n
**Mitigació·´·n:**
- Backup autom á—tico de saves V2 antes de migrar
- Herramienta de validació·´·n de saves migrados
- Opció·´·n de revertir a V2 si hay problemas

### Riesgo 2: Incompatibilidad de mec á—nicas
**Mitigació·´·n:**
- Documentar todas las diferencias V2 vs V4
- Implementar modo compatibilidad V2
- Testing exhaustivo de cada mec á—nica

### Riesgo 3: Regresió·´·n de rendimiento
**Mitigació·´·n:**
- Profiling continuo durante migració·´·n
- Comparar mé—tricas V2 vs V4
- Optimizar antes de cada release

## Mé—tricas de É—xito

### T é—cnicas
- [ ] 0 bugs crí·´­ticos en migració·´·n
- [ ] < 1% p é—rdida de rendimiento
- [ ] 100% de tests pasando
- [ ] Saves V2 migrables a V4 sin p é—rdida

### De usuario
- [ ] Todas las mec á—nicas V2 disponibles en V4
- [ ] Nuevas features de V4 implementadas
- [ ] UI/UX mejorada respecto a V2
- [ ] Loading times reducidos

## Pr ó—ximos Pasos Inmediatos

1. **Revisar repositorio kaisadilla/D-Tector-v2**
   - Identificar todos los Digimon V4
   - Identificar nuevas mec á—nicas
   - Documentar diferencias con V2

2. **Crear Spreadsheet de mapeo**
   - Columnas: Entity, V2_ID, V4_ID, Changes, Notes
   - Filas: Todos los Digimon, ataques, items, etc.

3. **Definir prioridad de migració·´·n**
   - Digimon más populares primero
   - Mec á—nicas core primero
   - Features secundarias después

---

**Ú·—ltima actualizació·´·n:** 21 de septiembre de 2026  
**Responsable:** cerudn  
**Estado:** En revisió·´·n
