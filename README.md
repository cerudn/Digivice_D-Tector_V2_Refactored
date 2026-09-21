# Digivice D-Tector V2 Refactored

[![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0+-blue?logo=csharp)](https://docs.microsoft.com/es-es/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/Status-FASE%201%20COMPLETADA-brightgreen)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored)

## 📖 Descripción

**Versió·´·n refactorizada y depurada** del proyecto **Digivice D-Tector V2** en Unity. Este repositorio contiene una arquitectura mejorada preparada para la futura migració·´·n a **V4**.

> ✨ **FASE 1 COMPLETADA** - Arquitectura refactorizada, interfaces extra í—das, patrones aplicados y listo para migrar a V4.

## 🚀 Quick Start

### 1. Clonar Repositorios

```bash
# Clonar repositorio original (necesario para los assets)
git clone https://github.com/cerudn/Digivice_D-tector-V2_Unity- original_v2

# Clonar repositorio refactorizado
git clone https://github.com/cerudn/Digivice_D-Tector_V2_Refactored refactored

cd refactored
```

### 2. Migrar Assets Autom á—ticamente

**Opci ó—n A: Python (Recomendado)**
```bash
# Windows, Linux o Mac
python Tools/migrate_assets.py ../original_v2 .
```

**Opci ó—n B: Batch (Windows)**
```bash
Tools\MigrateAssets.bat ../original_v2 .
```

**Opci ó—n C: Bash (Linux/Mac)**
```bash
chmod +x Tools/migrate-assets.sh
./Tools/migrate-assets.sh ../original_v2 .
```

### 3. Migrar Datos a ScriptableObjects

1. Abre el proyecto en Unity
2. Ve a `Tools > Digivice > Migrate V2 Data to SOs`
3. Click en **"Start Full Migration"**
4. Espera a que termine

### 4. ¡Jugar!

1. Dale a **Play** en Unity
2. GameBootstrapper crear á— todo autom á—ticamente
3. ¡Listo!

## 📁 Estructura del Proyecto

```
Digivice_D-Tector_V2_Refactored/
├── Assets/
│   ├── Audio/                    # ← Migrado desde original
│   ├── Data/                     # ✅ ScriptableObjects (Digimon, Attacks, Items)
│   ├── Editor/                   # ✅ V2DataMigrator
│   ├── Fonts/                    # ← Migrado desde original
│   ├── Icons/                    # ← Migrado desde original
│   ├── Prefab/                   # ← Migrado desde original
│   ├── Resources/                # ← Migrado desde original
│   ├── Scenes/                   # ← Migrado desde original
│   ├── Scripts/                  # ✅ Arquitectura refactorizada
│   └── Sprites/                  # ← Migrado desde original
├── Docs/                         # ✅ Documentació·´·n
├── Tools/                        # ✅ Scripts de migració·´·n
├── Packages/                     # ← Migrado desde original
├── ProjectSettings/              # ← Migrado desde original
└── README.md                     # ✅ Este archivo
```

## 🎯 Objetivos de la Refactorizació·´·n

- ✅ **Extraer interfaces** para todos los managers principales
- ✅ **Centralizar constants** y eliminar hardcoding
- ✅ **Implementar patr ó—n Observer** con eventos globales
- ✅ **Migrar datos** a ScriptableObjects
- ✅ **Agregar versionado** al sistema de guardado
- ✅ **Refactorizar LogicManager** con patr ó—n State
- ✅ **Optimizar SpriteDatabase** con cacheo y carga asíncrona
- ✅ **Automatizar migració·´·n** de assets originales

## 📊 Estado del Proyecto

### Fases Completadas

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 1A** - Auditor í—a V2 | ✅ COMPLETADA | 100% | [Informe](Docs/AUDIT_REPORT.md) |
| **FASE 1B** - Refactorizacó·´·n | ✅ COMPLETADA | 100% | [Log](Docs/REFACTORING_LOG.md) |
| **FASE 1C** - Migració·´·n Assets | ✅ COMPLETADA | 100% | [Gu í—a](#2-migrar-assets-autom á—ticamente) |

### Fases Pendientes

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 2** - Preparacó·´·n V4 | ⚪ PENDIENTE | 0% | [Plan](Docs/MIGRATION_V4_PLAN.md) |
| **FASE 3** - Migracó·´·n Datos | ⚪ PENDIENTE | 0% | - |
| **FASE 4** - Migracó·´·n Mec á—nicas | ⚪ PENDIENTE | 0% | - |
| **FASE 5** - Testing | ⚪ PENDIENTE | 0% | - |

## 🛠️ Requisitos

- **Unity**: 2021.3 LTS o superior
- **C#**: 9.0+
- **Python**: 3.7+ (para scripts de migració·´·n)
- **Plataforma**: iOS (original), multiplataforma (refactored)

## 📚 Documentació·´·n

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

## 🔗 Enlaces

- **Repositorio Original V2**: https://github.com/cerudn/Digivice_D-tector-V2_Unity-
- **Repositorio de Referencia**: https://github.com/kaisadilla/D-Tector-v2
- **Issue Tracker**: https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/issues

## 📄 Licencia

MIT License - ver archivo [LICENSE](LICENSE) para detalles.

---

**Desarrollado con ❤️ por cerudn**  
*Ú·—ltima actualizacó·´·n: 21 de septiembre de 2026*  
*Versó·´·n: 2.0 - FASE 1 COMPLETADA*
