@echo off
REM Script de migració·´·n autom á—tica de assets desde repositorio original
REM Uso: MigrateAssets.bat [path_al_original] [path_al_refactorizado]

echo ========================================
echo Digivice V2 Asset Migration Tool
echo ========================================
echo.

REM Paths por defecto
set ORIGINAL_PATH=..\Digivice_D-tector-V2_Unity-
set REFACTORED_PATH=.

REM Verificar si se pasaron como argumentos
if not "%1"=="" set ORIGINAL_PATH=%1
if not "%2"=="" set REFACTORED_PATH=%2

echo Original: %ORIGINAL_PATH%
echo Refactored: %REFACTORED_PATH%
echo.

REM Verificar que el repositorio original existe
if not exist "%ORIGINAL_PATH%" (
    echo ERROR: No se encontró el repositorio original en %ORIGINAL_PATH%
    echo.
    echo Por favor, clona el repositorio original primero:
    echo git clone https://github.com/cerudn/Digivice_D-tector-V2_Unity- %ORIGINAL_PATH%
    pause
    exit /b 1
)

echo Copiando assets multimedia...
echo.

REM Copiar Sprites
if exist "%ORIGINAL_PATH%\Assets\Sprites" (
    echo [1/7] Copiando Sprites...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Sprites" "%REFACTORED_PATH%\Assets\Sprites"
    echo ✓ Sprites copiados
) else (
    echo ✗ Sprites no encontrados
)

REM Copiar Audio
if exist "%ORIGINAL_PATH%\Assets\Audio" (
    echo [2/7] Copiando Audio...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Audio" "%REFACTORED_PATH%\Assets\Audio"
    echo ✓ Audio copiado
) else (
    echo ✗ Audio no encontrado
)

REM Copiar Fonts
if exist "%ORIGINAL_PATH%\Assets\Fonts" (
    echo [3/7] Copiando Fonts...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Fonts" "%REFACTORED_PATH%\Assets\Fonts"
    echo ✓ Fonts copiados
) else (
    echo ✗ Fonts no encontrados
)

REM Copiar Icons
if exist "%ORIGINAL_PATH%\Assets\Icons" (
    echo [4/7] Copiando Icons...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Icons" "%REFACTORED_PATH%\Assets\Icons"
    echo ✓ Icons copiados
) else (
    echo ✗ Icons no encontrados
)

REM Copiar Prefab
if exist "%ORIGINAL_PATH%\Assets\Prefab" (
    echo [5/7] Copiando Prefab...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Prefab" "%REFACTORED_PATH%\Assets\Prefab"
    echo ✓ Prefab copiados
) else (
    echo ✗ Prefab no encontrados
)

REM Copiar Resources
if exist "%ORIGINAL_PATH%\Assets\Resources" (
    echo [6/7] Copiando Resources...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Resources" "%REFACTORED_PATH%\Assets\Resources"
    echo ✓ Resources copiados
) else (
    echo ✗ Resources no encontrados
)

REM Copiar Scenes
if exist "%ORIGINAL_PATH%\Assets\Scenes" (
    echo [7/7] Copiando Scenes...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Assets\Scenes" "%REFACTORED_PATH%\Assets\Scenes"
    echo ✓ Scenes copiados
) else (
    echo ✗ Scenes no encontrados
)

echo.
echo Copiando configuració·´·n de Unity...
echo.

REM Copiar ProjectSettings
if exist "%ORIGINAL_PATH%\ProjectSettings" (
    echo [1/2] Copiando ProjectSettings...
    xcopy /E /I /Y "%ORIGINAL_PATH%\ProjectSettings" "%REFACTORED_PATH%\ProjectSettings"
    echo ✓ ProjectSettings copiados
) else (
    echo ✗ ProjectSettings no encontrados
)

REM Copiar Packages
if exist "%ORIGINAL_PATH%\Packages" (
    echo [2/2] Copiando Packages...
    xcopy /E /I /Y "%ORIGINAL_PATH%\Packages" "%REFACTORED_PATH%\Packages"
    echo ✓ Packages copiados
) else (
    echo ✗ Packages no encontrados
)

echo.
echo ========================================
echo ¡Migració·´·n completada!
echo ========================================
echo.
echo Pr ó—ximos pasos:
echo 1. Abre Unity con el proyecto refactorizado
echo 2. Ve a Tools > Digivice > Migrate V2 Data to SOs
echo 3. Click en "Start Full Migration"
echo 4. ¡Listo!
echo.
pause
