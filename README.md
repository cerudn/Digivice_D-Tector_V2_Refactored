# Digivice D-Tector V2 Refactored

[![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0+-blue?logo=csharp)](https://docs.microsoft.com/es-es/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/Status-FASE%201%20COMPLETADA-brightgreen)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored)
[![Version](https://img.shields.io/github/v/tag/cerudn/Digivice_D-Tector_V2_Refactored?label=version)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/releases)
[![Last Commit](https://img.shields.io/github/last-commit/cerudn/Digivice_D-Tector_V2_Refactored)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/commits/main)
[![Lines of Code](https://img.shields.io/badge/lines-5000+-blue)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored)

## 📖 Descripción

**Versó·´·n refactorizada y depurada** del proyecto **Digivice D-Tector V2** en Unity. Este repositorio contiene una arquitectura mejorada preparada para la futura migracó·´·n a **V4**.

> ✨ **FASE 1 COMPLETADA** - Arquitectura refactorizada, interfaces extra í—das, patrones aplicados y listo para migrar a V4.

## 🎯 Objetivos de la Refactorizacó·´·n

- ✅ **Extraer interfaces** para todos los managers principales
- ✅ **Centralizar constants** y eliminar hardcoding
- ✅ **Implementar patr ó—n Observer** con eventos globales
- ✅ **Migrar datos** a ScriptableObjects
- ✅ **Agregar versionado** al sistema de guardado
- ✅ **Refactorizar LogicManager** con patr ó—n State
- ✅ **Optimizar SpriteDatabase** con cacheo y carga asíncrona
- ✅ **Implementar input** multi-plataforma

## 📊 Estado del Proyecto

### Fases Completadas

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 1A** - Auditor í—a V2 | ✅ COMPLETADA | 100% | [Informe](Docs/AUDIT_REPORT.md) |
| **FASE 1B** - Refactorizacó·´·n | ✅ COMPLETADA | 100% | [Log](Docs/REFACTORING_LOG.md) |

### Fases Pendientes

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 2** - Preparacó·´·n V4 | ⚪ PENDIENTE | 0% | [Plan](Docs/MIGRATION_V4_PLAN.md) |
| **FASE 3** - Migracó·´·n Datos | ⚪ PENDIENTE | 0% | - |
| **FASE 4** - Migracó·´·n Mec á—nicas | ⚪ PENDIENTE | 0% | - |
| **FASE 5** - Testing | ⚪ PENDIENTE | 0% | - |

## 🏗️ Arquitectura Refactorizada

### Principios Aplicados

```
┌─────────────────────────────────────────┐
│         SOLID Principles                │
├─────────────────────────────────────────┤
│ S - Single Responsibility               │
│ O - Open/Closed                         │
│ L - Liskov Substitution                 │
│ I - Interface Segregation               │
│ D - Dependency Inversion                │
└─────────────────────────────────────────┘
```

### Design Patterns

- **State Pattern** - Má—quina de estados (5 estados)
- **Observer Pattern** - Eventos globales (18 eventos)
- **Singleton Pattern** - GameManager thread-safe
- **Repository Pattern** - Database como repositorio
- **Strategy Pattern** - Input multi-plataforma

## 📁 Estructura del Proyecto

```
Digivice_D-Tector_V2_Refactored/
├── Assets/
│   ├── Scripts/
│   │   ├── Interfaces/           ✅ 7 interfaces
│   │   ├── Constants/            ✅ 5 archivos
│   │   ├── Events/               ✅ 18 eventos
│   │   ├── Core/                 ✅ GameManager
│   │   ├── Logic/                ✅ 5 estados
│   │   ├── Models/               ✅ 3 modelos
│   │   ├── Data/                 ✅ 5 ScriptableObjects
│   │   ├── SavedGame/            ✅ Sistema versionado
│   │   ├── Input/                ✅ InputManager
│   │   └── Utility/              ✅ Utilidades
│   └── Data/                     ✅ Assets de ejemplo
├── Editor/                       ✅ Custom Editors
├── Docs/                         ✅ 9 documentos
├── CHANGELOG.md                  ✅ Historial
└── README.md                     ✅ Este archivo
```

## 🚀 Caracter í—sticas

### Arquitectura
- ✅ **Inyeccó·´·n de dependencias** vía interfaces
- ✅ **Patr ó—n State** para má—quina de estados
- ✅ **Patr ó—n Observer** para comunicacó·´·n
- ✅ **SOLID principles** aplicados

### Sistema de Estados
- ✅ **ExplorationState** - Movimiento y exploracó·´·n
- ✅ **BattleState** - Combate por turnos
- ✅ **EvolutionState** - Proceso de evolucó·´·n
- ✅ **MenuState** - Navegacó·´·n de men ú—s
- ✅ **AppState** - Apps del Digivice

### Sistema de Guardado
- ✅ **Versionado** de schema (V2→V3→V4)
- ✅ **Checksum** para integridad (SHA-256)
- ✅ **Encriptacó·´·n** b á—sica (AES-256)
- ✅ **Migracó·´·n autom á—tica** entre versiones
- ✅ **Auto-guardado** peri ó—dico

### Sistema de Datos
- ✅ **ScriptableObjects** para Digimon, Attacks, Items
- ✅ **DatabaseSO** para carga desde assets
- ✅ **Custom Editors** para edicó·´·n visual
- ✅ **Sin hardcoding** - todo en assets

### Input
- ✅ **Multi-plataforma** (teclado, tá—ctil, gamepad)
- ✅ **Input contextual** (movimiento, men ú—)
- ✅ **Cooldowns** configurables
- ✅ **Eventos** de input

### Rendimiento
- ✅ **Cacheo inteligente** de datos
- ✅ **B ú—squeda O(1)** con diccionarios
- ✅ **Lazy loading** para recursos

## 📚 Documentacó·´·n

| Documento | Descripcó·´·n | Estado |
|-----------|----------------|--------|
| [ARCHITECTURE.md](Docs/ARCHITECTURE.md) | Visó·´·n general de arquitectura | ✅ |
| [REFACTORING_LOG.md](Docs/REFACTORING_LOG.md) | Registro de refactorizacó·´·n | ✅ |
| [MIGRATION_V4_PLAN.md](Docs/MIGRATION_V4_PLAN.md) | Plan de migracó·´·n a V4 | ✅ |
| [EXECUTIVE_SUMMARY.md](Docs/EXECUTIVE_SUMMARY.md) | Resumen ejecutivo | ✅ |
| [SAVE_SYSTEM_GUIDE.md](Docs/SAVE_SYSTEM_GUIDE.md) | Guía del sistema de guardado | ✅ |
| [SCRIPTABLEOBJECTS_GUIDE.md](Docs/SCRIPTABLEOBJECTS_GUIDE.md) | Guía de ScriptableObjects | ✅ |
| [STATE_MACHINE_GUIDE.md](Docs/STATE_MACHINE_GUIDE.md) | Guía de la má—quina de estados | ✅ |
| [PROJECT_SUMMARY.md](Docs/PROJECT_SUMMARY.md) | Resumen completo del proyecto | ✅ |
| [CHANGELOG.md](CHANGELOG.md) | Historial de cambios | ✅ |

## 🛠️ Requisitos

- **Unity**: 2021.3 LTS o superior
- **C#**: 9.0+
- **Plataforma**: iOS (original), multiplataforma (refactored)

## 🚀 Quick Start

### 1. Clonar

```bash
git clone https://github.com/cerudn/Digivice_D-Tector_V2_Refactored.git
cd Digivice_D-Tector_V2_Refactored
```

### 2. Importar a Unity

1. Abrir Unity Hub
2. Add > Add project from disk
3. Seleccionar carpeta
4. Abrir con Unity 2021.3+

### 3. Configurar

1. Crear GameObject "Database" en escena
2. A ñ—adir componente `DatabaseSO`
3. Asignar arrays de DigimonData, AttackData, ItemDataSO

### 4. Crear Digimon

1. Right-click > Create > Digimon > Create Digimon
2. Nombrar (ej: "Agumon")
3. Rellenar datos en Inspector
4. ¡Listo!

## 📊 Mé—tricas

| Categor í—a | Cantidad |
|-------------|----------|
| **Interfaces** | 7 |
| **Constants** | 5 archivos |
| **Eventos** | 18 |
| **Estados** | 5 |
| **Modelos** | 3 |
| **ScriptableObjects** | 5 clases |
| **Documentos** | 9 |
| **L í—neas de Có—digo** | ~5,000+ |

## 🔗 Enlaces

- **Repositorio Original V2**: https://github.com/cerudn/Digivice_D-tector-V2_Unity-
- **Repositorio de Referencia**: https://github.com/kaisadilla/D-Tector-v2
- **Issue Tracker**: https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/issues

## 📄 Licencia

MIT License - ver archivo [LICENSE](LICENSE) para detalles.

---

**Desarrollado con ❤️ por cerudn**  
*Ú·—ltima actualizacó·´·n: 21 de septiembre de 2026*  
*Versó·´·n: 1.0.0 - FASE 1 COMPLETADA*
