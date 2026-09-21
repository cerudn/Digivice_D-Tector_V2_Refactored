#!/bin/bash
# Script de migració·´·n autom á—tica de assets desde repositorio original
# Uso: ./migrate-assets.sh [path_al_original] [path_al_refactorizado]

echo "========================================"
echo "Digivice V2 Asset Migration Tool"
echo "========================================"
echo ""

# Paths por defecto
ORIGINAL_PATH="../Digivice_D-tector-V2_Unity-"
REFACTORED_PATH="."

# Verificar si se pasaron como argumentos
if [ ! -z "$1" ]; then
    ORIGINAL_PATH="$1"
fi

if [ ! -z "$2" ]; then
    REFACTORED_PATH="$2"
fi

echo "Original: $ORIGINAL_PATH"
echo "Refactored: $REFACTORED_PATH"
echo ""

# Verificar que el repositorio original existe
if [ ! -d "$ORIGINAL_PATH" ]; then
    echo "ERROR: No se encontró el repositorio original en $ORIGINAL_PATH"
    echo ""
    echo "Por favor, clona el repositorio original primero:"
    echo "git clone https://github.com/cerudn/Digivice_D-tector-V2_Unity- $ORIGINAL_PATH"
    exit 1
fi

echo "Copiando assets multimedia..."
echo ""

# Copiar Sprites
if [ -d "$ORIGINAL_PATH/Assets/Sprites" ]; then
    echo "[1/7] Copiando Sprites..."
    cp -r "$ORIGINAL_PATH/Assets/Sprites" "$REFACTORED_PATH/Assets/"
    echo "✓ Sprites copiados"
else
    echo "✗ Sprites no encontrados"
fi

# Copiar Audio
if [ -d "$ORIGINAL_PATH/Assets/Audio" ]; then
    echo "[2/7] Copiando Audio..."
    cp -r "$ORIGINAL_PATH/Assets/Audio" "$REFACTORED_PATH/Assets/"
    echo "✓ Audio copiado"
else
    echo "✗ Audio no encontrado"
fi

# Copiar Fonts
if [ -d "$ORIGINAL_PATH/Assets/Fonts" ]; then
    echo "[3/7] Copiando Fonts..."
    cp -r "$ORIGINAL_PATH/Assets/Fonts" "$REFACTORED_PATH/Assets/"
    echo "✓ Fonts copiados"
else
    echo "✗ Fonts no encontrados"
fi

# Copiar Icons
if [ -d "$ORIGINAL_PATH/Assets/Icons" ]; then
    echo "[4/7] Copiando Icons..."
    cp -r "$ORIGINAL_PATH/Assets/Icons" "$REFACTORED_PATH/Assets/"
    echo "✓ Icons copiados"
else
    echo "✗ Icons no encontrados"
fi

# Copiar Prefab
if [ -d "$ORIGINAL_PATH/Assets/Prefab" ]; then
    echo "[5/7] Copiando Prefab..."
    cp -r "$ORIGINAL_PATH/Assets/Prefab" "$REFACTORED_PATH/Assets/"
    echo "✓ Prefab copiados"
else
    echo "✗ Prefab no encontrados"
fi

# Copiar Resources
if [ -d "$ORIGINAL_PATH/Assets/Resources" ]; then
    echo "[6/7] Copiando Resources..."
    cp -r "$ORIGINAL_PATH/Assets/Resources" "$REFACTORED_PATH/Assets/"
    echo "✓ Resources copiados"
else
    echo "✗ Resources no encontrados"
fi

# Copiar Scenes
if [ -d "$ORIGINAL_PATH/Assets/Scenes" ]; then
    echo "[7/7] Copiando Scenes..."
    cp -r "$ORIGINAL_PATH/Assets/Scenes" "$REFACTORED_PATH/Assets/"
    echo "✓ Scenes copiados"
else
    echo "✗ Scenes no encontrados"
fi

echo ""
echo "Copiando configuració·´·n de Unity..."
echo ""

# Copiar ProjectSettings
if [ -d "$ORIGINAL_PATH/ProjectSettings" ]; then
    echo "[1/2] Copiando ProjectSettings..."
    cp -r "$ORIGINAL_PATH/ProjectSettings" "$REFACTORED_PATH/"
    echo "✓ ProjectSettings copiados"
else
    echo "✗ ProjectSettings no encontrados"
fi

# Copiar Packages
if [ -d "$ORIGINAL_PATH/Packages" ]; then
    echo "[2/2] Copiando Packages..."
    cp -r "$ORIGINAL_PATH/Packages" "$REFACTORED_PATH/"
    echo "✓ Packages copiados"
else
    echo "✗ Packages no encontrados"
fi

echo ""
echo "========================================"
echo "✓ ¡Migració·´·n completada!"
echo "========================================"
echo ""
echo "Pr ó—ximos pasos:"
echo "1. Abre Unity con el proyecto refactorizado"
echo "2. Ve a Tools > Digivice > Migrate V2 Data to SOs"
echo "3. Click en \"Start Full Migration\""
echo "4. ¡Listo!"
echo ""
