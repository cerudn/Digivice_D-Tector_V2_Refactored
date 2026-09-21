# Changelog

Todos los cambios importantes en este proyecto.

## [1.0.0] - 2026-09-21

### ✨ FASE 1 COMPLETADA

#### Añ·´·adido

**Arquitectura:**
- 7 interfaces principales (ILogicManager, IScreenManager, etc.)
- 5 archivos de constants centralizados
- 18 eventos globales en GameEvents
- GameManager refactorizado con DI
- Patr ó—n State implementado

**Estados:**
- ExplorationState - Movimiento y exploracó·´·n
- BattleState - Combate por turnos
- EvolutionState - Proceso de evolucó·´·n
- MenuState - Navegacó·´·n de men ú—s
- AppState - Apps del Digivice

**Modelos de Datos:**
- Digimon - Modelo completo de Digimon
- Attack - Modelo de ataques
- Item - Modelo de items
- SaveData - Datos de guardado
- PlayerData - Datos del jugador

**ScriptableObjects:**
- DigimonData - Datos de Digimon editables
- AttackData - Datos de ataques
- EvolutionRequirement - Requisitos de evolucó·´·n
- ItemDataSO - Datos de items
- DatabaseSO - Carga desde assets

**Sistema de Guardado:**
- Versionado V2→V3→V4
- Checksum SHA-256
- Encriptacó·´·n AES-256
- Migracó·´·n autom á—tica
- Auto-guardado peri ó—dico

**Input:**
- InputManager completo
- Soporte teclado (WASD, flechas)
- Soporte tá—ctil (swipes, taps)
- Soporte gamepad (placeholder)
- Input contextual (movimiento, men ú—)

**Utilidades:**
- Mathf - Utilidades matem á—ticas
- Console - Wrapper de logging
- SystemInfoHelper - Info del sistema

**Custom Editors:**
- DigimonDataEditor - Editor visual para Digimon

**Documentacó·´·n:**
- ARCHITECTURE.md
- REFACTORING_LOG.md
- MIGRATION_V4_PLAN.md
- EXECUTIVE_SUMMARY.md
- SAVE_SYSTEM_GUIDE.md
- SCRIPTABLEOBJECTS_GUIDE.md
- PROJECT_SUMMARY.md
- STATE_MACHINE_GUIDE.md
- CHANGELOG.md (este archivo)

**Assets de Ejemplo:**
- Agumon.asset
- Gabumon.asset

#### Cambios

- Database.cs refactorizado para usar ScriptableObjects
- GameManager ahora usa interfaces en lugar de clases concretas
- Sistema de eventos reemplaza llamadas directas entre managers
- Constants centralizados eliminan magic numbers

#### Mejoras

- Rendimiento: B ú—squeda O(1) con diccionarios
- Memoria: Cacheo inteligente de datos
- Mantenibilidad: Có—digo modular y testeable
- Seguridad: Encriptacó·´·n de saves

---

## [0.2.0] - 2026-09-21

### Añ·´·adido

- Estados EvolutionState, MenuState, AppState
- InputManager completo
- SystemInfoHelper
- STATE_MACHINE_GUIDE.md

---

## [0.1.0] - 2026-09-21

### Añ·´·adido

- Interfaces principales
- Constants
- GameEvents
- GameManager refactorizado
- BattleState, ExplorationState
- Modelos Digimon, Attack, Item
- ScriptableObjects
- Sistema de guardado
- Documentacó·´·n b á—sica

---

## [0.0.1] - 2026-09-21

### Añ·´·adido

- Creacó·´·n del repositorio
- Estructura inicial de carpetas
- README b á—sico

---

## Convenciones

### Versiones

- `MAJOR.MINOR.PATCH`
- `MAJOR`: Cambios incompatibles
- `MINOR`: Nuevas features (compatibles)
- `PATCH`: Bug fixes

### Tipos de Cambio

- `Añ·´·adido`: Nuevas features
- `Cambiado`: Cambios en existing
- `Eliminado`: Features removidas
- `Corregido`: Bug fixes
- `Mejorado`: Mejoras de rendimiento/calidad

---

**Ú·—ltima actualizacó·´·n:** 21 de septiembre de 2026
