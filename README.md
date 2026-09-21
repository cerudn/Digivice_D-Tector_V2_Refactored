# Digivice D-Tector V2 Refactored

[![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0+-blue?logo=csharp)](https://docs.microsoft.com/es-es/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/Status-FASE%201%20COMPLETADA-brightgreen)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored)
[![Last Commit](https://img.shields.io/github/last-commit/cerudn/Digivice_D-Tector_V2_Refactored)](https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/commits/main)

## 📖 Descripción

**Versió·´·n refactorizada y depurada** del proyecto **Digivice D-Tector V2** en Unity. Este repositorio contiene una arquitectura mejorada preparada para la futura migració·´·n a **V4**.

> ✨ **FASE 1 COMPLETADA** - Arquitectura refactorizada, interfaces extraí·´­das, patrones aplicados y listo para migrar a V4.

## 🎯 Objetivos de la Refactorizació·´·n

- ✅ **Extraer interfaces** para todos los managers principales
- ✅ **Centralizar constants** y eliminar hardcoding
- ✅ **Implementar patr ó—n Observer** con eventos globales
- ✅ **Migrar datos** a ScriptableObjects
- ✅ **Agregar versionado** al sistema de guardado
- ✅ **Refactorizar LogicManager** con patr ó—n State
- ✅ **Optimizar SpriteDatabase** con cacheo y carga asíncrona

## 📊 Estado del Proyecto

### Fases Completadas

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 1A** - Auditorí·´­a V2 | ✅ COMPLETADA | 100% | [Informe](Docs/AUDIT_REPORT.md) |
| **FASE 1B** - Refactorizació·´·n | ✅ COMPLETADA | 100% | [Log](Docs/REFACTORING_LOG.md) |

### Fases Pendientes

| Fase | Estado | Progreso | Docs |
|------|--------|----------|------|
| **FASE 2** - Preparació·´·n V4 | ⚪ PENDIENTE | 0% | [Plan](Docs/MIGRATION_V4_PLAN.md) |
| **FASE 3** - Migració·´·n Datos | ⚪ PENDIENTE | 0% | - |
| **FASE 4** - Migració·´·n Mec á—nicas | ⚪ PENDIENTE | 0% | - |
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

- **State Pattern** - Má—quina de estados explí·´­cita
- **Observer Pattern** - Eventos globales (GameEvents)
- **Singleton Pattern** - GameManager thread-safe
- **Repository Pattern** - Database como repositorio

## 📁 Estructura del Proyecto

```
Digivice_D-Tector_V2_Refactored/
├── Assets/
│   ├── Scripts/
│   │   ├── Interfaces/           ✅ 7 interfaces
│   │   ├── Constants/            ✅ 5 archivos
│   │   ├── Events/               ✅ 18 eventos
│   │   ├── Core/                 ✅ GameManager refactorizado
│   │   ├── Logic/                ✅ Patr ó—n State
│   │   ├── Models/               ✅ Digimon, Attack, Item
│   │   ├── Data/                 ✅ ScriptableObjects
│   │   ├── SavedGame/            ✅ Sistema versionado
│   │   └── Utility/              ✅ Utilidades
│   └── Data/                     ✅ Assets de ejemplo
├── Editor/                       ✅ Custom Editors
├── Docs/                         ✅ 7 documentos
└── README.md                     ✅ Este archivo
```

## 🚀 Caracterí·´­sticas

### Arquitectura
- ✅ **Inyecció·´·n de dependencias** vía interfaces
- ✅ **Patr ó—n State** para má—quina de estados
- ✅ **Patr ó—n Observer** para comunicació·´·n
- ✅ **SOLID principles** aplicados

### Sistema de Guardado
- ✅ **Versionado** de schema (V2→V3→V4)
- ✅ **Checksum** para integridad (SHA-256)
- ✅ **Encriptació·´·n** b á—sica (AES-256)
- ✅ **Migració·´·n autom á—tica** entre versiones
- ✅ **Auto-guardado** peri ó—dico

### Sistema de Datos
- ✅ **ScriptableObjects** para Digimon, Attacks, Items
- ✅ **DatabaseSO** para carga desde assets
- ✅ **Custom Editors** para edició·´·n visual
- ✅ **Sin hardcoding** - todo en assets

### Rendimiento
- ✅ **Cacheo inteligente** de datos
- ✅ **B ú—squeda O(1)** con diccionarios
- ✅ **Lazy loading** para recursos

## 📚 Documentació·´·n

| Documento | Descripció·´·n | Estado |
|-----------|----------------|--------|
| [ARCHITECTURE.md](Docs/ARCHITECTURE.md) | Visió·´·n general de arquitectura | ✅ |
| [REFACTORING_LOG.md](Docs/REFACTORING_LOG.md) | Registro de refactorizació·´·n | ✅ |
| [MIGRATION_V4_PLAN.md](Docs/MIGRATION_V4_PLAN.md) | Plan de migració·´·n a V4 | ✅ |
| [EXECUTIVE_SUMMARY.md](Docs/EXECUTIVE_SUMMARY.md) | Resumen ejecutivo | ✅ |
| [SAVE_SYSTEM_GUIDE.md](Docs/SAVE_SYSTEM_GUIDE.md) | Guía del sistema de guardado | ✅ |
| [SCRIPTABLEOBJECTS_GUIDE.md](Docs/SCRIPTABLEOBJECTS_GUIDE.md) | Guía de ScriptableObjects | ✅ |
| [PROJECT_SUMMARY.md](Docs/PROJECT_SUMMARY.md) | Resumen completo del proyecto | ✅ |

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
| **Estados** | 2 |
| **Modelos** | 3 |
| **ScriptableObjects** | 5 clases |
| **Documentos** | 7 |
| **L í—neas de Có—digo** | ~4,500+ |

## 🔗 Enlaces

- **Repositorio Original V2**: https://github.com/cerudn/Digivice_D-tector-V2_Unity-
- **Repositorio de Referencia**: https://github.com/kaisadilla/D-Tector-v2
- **Issue Tracker**: https://github.com/cerudn/Digivice_D-Tector_V2_Refactored/issues

## 📄 Licencia

MIT License - ver archivo [LICENSE](LICENSE) para detalles.

---

**Desarrollado con ❤️ por cerudn**  
*Ú·—ltima actualizació·´·n: 21 de septiembre de 2026*  
*Versió·´·n: 1.0 - FASE 1 COMPLETADA*
