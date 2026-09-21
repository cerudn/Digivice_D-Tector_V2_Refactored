# Guía de Migració·´·n de Assets F í—sicos

## ⚠️ IMPORTANTE: Leer antes de continuar

Este repositorio contiene TODO el có—digo refactorizado y los ScriptableObjects, pero **NO** contiene los recursos multimedia originales (sprites, audio, etc.) por limitaciones de GitHub.

**DEBES copiar manualmente** los siguientes archivos desde el repositorio original.

---

## 📂 PASO 1: Clonar Repositorio Original

```bash
# Clona el repositorio original en una carpeta temporal
git clone https://github.com/cerudn/Digivice_D-tector-V2_Unity-.git v2_original
```

---

## 📂 PASO 2: Copiar Carpetas Cr í—ticas

### Windows (PowerShell):

```powershell
# Navega a la carpeta del repositorio refactorizado
cd Digivice_D-Tector_V2_Refactored

# Copiar Sprites
Copy-Item -Path "../v2_original/Assets/Sprites" -Destination "./Assets/" -Recurse -Force

# Copiar Audio
Copy-Item -Path "../v2_original/Assets/Audio" -Destination "./Assets/" -Recurse -Force

# Copiar Fonts
Copy-Item -Path "../v2_original/Assets/Fonts" -Destination "./Assets/" -Recurse -Force

# Copiar Icons
Copy-Item -Path "../v2_original/Assets/Icons" -Destination "./Assets/" -Recurse -Force

# Copiar Resources (opcional pero recomendado)
Copy-Item -Path "../v2_original/Assets/Resources" -Destination "./Assets/" -Recurse -Force

# Copiar Prefab (opcional)
Copy-Item -Path "../v2_original/Assets/Prefab" -Destination "./Assets/" -Recurse -Force

# Copiar Scenes (opcional)
Copy-Item -Path "../v2_original/Assets/Scenes" -Destination "./Assets/" -Recurse -Force

# Copiar configuració·´·n de Unity
Copy-Item -Path "../v2_original/ProjectSettings" -Destination "./" -Recurse -Force
Copy-Item -Path "../v2_original/Packages" -Destination "./" -Recurse -Force
```

### Mac/Linux (Bash):

```bash
# Navega a la carpeta del repositorio refactorizado
cd Digivice_D-Tector_V2_Refactored

# Copiar Sprites
cp -r ../v2_original/Assets/Sprites ./Assets/

# Copiar Audio
cp -r ../v2_original/Assets/Audio ./Assets/

# Copiar Fonts
cp -r ../v2_original/Assets/Fonts ./Assets/

# Copiar Icons
cp -r ../v2_original/Assets/Icons ./Assets/

# Copiar Resources
cp -r ../v2_original/Assets/Resources ./Assets/

# Copiar Prefab
cp -r ../v2_original/Assets/Prefab ./Assets/

# Copiar Scenes
cp -r ../v2_original/Assets/Scenes ./Assets/

# Copiar configuració·´·n de Unity
cp -r ../v2_original/ProjectSettings ./
cp -r ../v2_original/Packages ./
```

### Windows (CMD):

```cmd
REM Navega a la carpeta del repositorio refactorizado
cd Digivice_D-Tector_V2_Refactored

REM Copiar Sprites
xcopy /E /I /Y ..\v2_original\Assets\Sprites .\Assets\Sprites

REM Copiar Audio
xcopy /E /I /Y ..\v2_original\Assets\Audio .\Assets\Audio

REM Copiar Fonts
xcopy /E /I /Y ..\v2_original\Assets\Fonts .\Assets\Fonts

REM Copiar Icons
xcopy /E /I /Y ..\v2_original\Assets\Icons .\Assets\Icons

REM Copiar Resources
xcopy /E /I /Y ..\v2_original\Assets\Resources .\Assets\Resources

REM Copiar configuració·´·n de Unity
xcopy /E /I /Y ..\v2_original\ProjectSettings .\ProjectSettings
xcopy /E /I /Y ..\v2_original\Packages .\Packages
```

---

## 📂 PASO 3: Verificar Estructura Final

Despu é—s de copiar, tu estructura debe verse así:

```
Digivice_D-Tector_V2_Refactored/
├── Assets/
│   ├── Audio/                    # ← COPIADO
│   ├── Data/                     # ✅ YA EXISTE
│   │   ├── Digimon/
│   │   ├── Attacks/
│   │   └── Items/
│   ├── Editor/                   # ✅ YA EXISTE
│   ├── Fonts/                    # ← COPIADO
│   ├── Icons/                    # ← COPIADO
│   ├── Prefab/                   # ← COPIADO (opcional)
│   ├── Resources/                # ← COPIADO (opcional)
│   ├── Scenes/                   # ← COPIADO (opcional)
│   ├── Scripts/                  # ✅ YA EXISTE
│   └── Sprites/                  # ← COPIADO
├── Docs/                         # ✅ YA EXISTE
├── Packages/                     # ← COPIADO
├── ProjectSettings/              # ← COPIADO
└── README.md                     # ✅ YA EXISTE
```

---

## 📂 PASO 4: Commit y Push

```bash
# A ñ—adir todos los archivos copiados
git add Assets/Audio Assets/Sprites Assets/Fonts Assets/Icons Assets/Resources Assets/Prefab Assets/Scenes
git add ProjectSettings Packages

# Commit
git commit -m "feat: Copiar assets multimedia desde V2 original"

# Push
git push origin main
```

---

## 📂 PASO 5: Ejecutar V2DataMigrator

1. Abre Unity con el proyecto refactorizado
2. Ve a `Tools > Digivice > Migrate V2 Data to SOs`
3. Click en "Start Full Migration"
4. Verifica en la consola que se asignaron los sprites

---

## ⚠️ NOTAS IMPORTANTES

### Sprites

Los sprites en el repositorio original están en **spritesheets** (im á—genes grandes con m ú—ltiples sprites), NO en archivos individuales por Digimon.

**Archivos principales:**
- `characters.png` - Sprites de Digimon
- `animations.png` - Animaciones
- `menus.png` - UI
- `font_*.png` - Fuentes

**NO esperes encontrar:**
- `001_idle.png` (no existe)
- `001_walk.png` (no existe)

**El migrator NO podr á— asignar sprites autom á—ticamente** porque no hay archivos individuales. Tendr á—s que:
1. O usar los spritesheets originales directamente
2. O crear un script personalizado para extraer sprites del spritesheet

### Audio

El audio S Í— está en archivos individuales:
- `BGM_01.mp3`, `BGM_02.mp3`, etc.
- `SFX_cursor.mp3`, `SFX_confirm.mp3`, etc.

El migrator S Í— podr á— asignar estos autom á—ticamente.

---

## ✅ CHECKLIST FINAL

```bash
[ ] Clonar repositorio original
[ ] Copiar Assets/Sprites/
[ ] Copiar Assets/Audio/
[ ] Copiar Assets/Fonts/
[ ] Copiar Assets/Icons/
[ ] Copiar Assets/Resources/ (opcional)
[ ] Copiar Assets/Prefab/ (opcional)
[ ] Copiar Assets/Scenes/ (opcional)
[ ] Copiar ProjectSettings/
[ ] Copiar Packages/
[ ] Hacer git add, commit, push
[ ] Abrir Unity
[ ] Ejecutar V2DataMigrator
[ ] Verificar que no hay errores en consola
[ ] Dar Play y probar
```

---

## 🎯 RESUMEN R Á—PIDO

**Comando ú—nico (Mac/Linux):**
```bash
cp -r v2_original/Assets/{Sprites,Audio,Fonts,Icons,Resources,Prefab,Scenes} refactored/Assets/
cp -r v2_original/{ProjectSettings,Packages} refactored/
```

**Comando ú—nico (Windows PowerShell):**
```powershell
Copy-Item -Path "../v2_original/Assets/*" -Destination "./Assets/" -Include Sprites,Audio,Fonts,Icons,Resources,Prefab,Scenes -Recurse -Force
Copy-Item -Path "../v2_original/ProjectSettings" -Destination "./" -Recurse -Force
Copy-Item -Path "../v2_original/Packages" -Destination "./" -Recurse -Force
```

---

**Ú·—ltima actualizació·´·n:** 21 de septiembre de 2026  
**Versió·´·n:** 1.0
