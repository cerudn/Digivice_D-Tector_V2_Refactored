# Digivice D-Tector V2 Refactored

[![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0+-blue?logo=csharp)](https://docs.microsoft.com/es-es/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

## 📖 Descripción

Versión refactorizada y depurada del proyecto **Digivice D-Tector V2** en Unity. Este repositorio contiene una arquitectura mejorada preparada para la futura migración a **V4**.

## 🎯 Objetivos de la Refactorización

- ✅ **Extraer interfaces** para todos los managers principales
- ✅ **Centralizar constants** y eliminar hardcoding
- ✅ **Implementar patrón Observer** con eventos globales
- ✅ **Migrar datos** a ScriptableObjects
- ✅ **Agregar versionado** al sistema de guardado
- ✅ **Refactorizar LogicManager** con patrón State
- ✅ **Optimizar SpriteDatabase** con cacheo y carga asíncrona

## 📁 Estructura del Proyecto

```
Assets/
├── Scripts/
│   ├── Interfaces/           # Contratos para sistemas
│   ├── Constants/            # Valores centralizados
│   ├── Events/               # Sistema de eventos global
│   ├── Core/                 # Implementaciones de managers
│   ├── Logic/                # Lógica con patrón State
│   ├── Data/                 # ScriptableObjects
│   ├── SavedGame/            # Sistema versionado
│   └── SpriteSystem/         # Optimizado con cacheo
├── Data/                     # Assets de ScriptableObjects
├── Scenes/                   # Escenas del juego
└── Resources/                # Recursos cargables
```

## 🚀 Características

### Arquitectura
- **Inyección de dependencias** vía interfaces
- **Patrón State** para máquina de estados
- **Patrón Observer** para comunicación entre sistemas
- **SOLID principles** aplicados

### Sistema de Guardado
- **Versionado** de schema
- **Checksum** para integridad
- **Migración automática** entre versiones

### Rendimiento
- **Cacheo inteligente** de sprites
- **Carga asíncrona** sin bloquear hilo principal
- **Lazy loading** para recursos

## 📋 Estado del Proyecto

### ✅ Fase 1A - Auditoría Completada
- [x] Mapeo de arquitectura V2
- [x] Identificación de deuda técnica
- [x] Documentación de flujo de datos

### 🔄 Fase 1B - Refactorización en Curso
- [x] Extracción de interfaces
- [x] Constants centralizados
- [x] Sistema de eventos global
- [ ] Implementación de clases concretas
- [ ] Patrón State en LogicManager
- [ ] ScriptableObjects para datos

### ⚪ Fase 2 - Migración a V4 (Pendiente)
- [ ] Mapeo V2 → V4
- [ ] Migración de datos
- [ ] Migración de mecánicas
- [ ] Testing y validación

## 🛠️ Requisitos

- **Unity**: 2021.3 LTS o superior
- **C#**: 9.0+
- **Plataforma**: iOS (original), multiplataforma (refactored)

## 📚 Documentación

- [Arquitectura del Proyecto](Docs/ARCHITECTURE.md)
- [Registro de Refactorización](Docs/REFACTORING_LOG.md)
- [Plan de Migración a V4](Docs/MIGRATION_V4_PLAN.md)

## 🤝 Contribución

Este proyecto es una refactorización personal. Si encuentras issues o tienes sugerencias, por favor crea un issue en el repositorio.

## 📄 Licencia

MIT License - ver archivo [LICENSE](LICENSE) para detalles.

## 🔗 Enlaces

- [Repositorio Original V2](https://github.com/cerudn/Digivice_D-tector-V2_Unity-)
- [Repositorio de Referencia](https://github.com/kaisadilla/D-Tector-v2)

---

**Desarrollado con ❤️ por cerudn**
