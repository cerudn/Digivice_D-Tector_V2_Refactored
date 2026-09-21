#!/usr/bin/env python3
"""
Digivice V2 Asset Migration Tool

Script de migració·´·n autom á—tica de assets desde el repositorio original
al repositorio refactorizado.

Uso:
    python migrate_assets.py [path_original] [path_refactorizado]

Ejemplos:
    python migrate_assets.py
    python migrate_assets.py ../Digivice_D-tector-V2_Unity- .
"""

import os
import sys
import shutil
from pathlib import Path


class AssetMigrator:
    """Migrador autom á—tico de assets de Unity."""
    
    def __init__(self, original_path: str, refactored_path: str):
        self.original_path = Path(original_path)
        self.refactored_path = Path(refactored_path)
        self.copied_count = 0
        self.skipped_count = 0
        self.error_count = 0
    
    def run(self):
        """Ejecuta la migració·´·n completa."""
        print("=" * 60)
        print("Digivice V2 Asset Migration Tool")
        print("=" * 60)
        print()
        print(f"Original: {self.original_path.absolute()}")
        print(f"Refactored: {self.refactored_path.absolute()}")
        print()
        
        # Verificar que el repositorio original existe
        if not self.original_path.exists():
            print(f"ERROR: No se encontró el repositorio original en {self.original_path}")
            print()
            print("Por favor, clona el repositorio original primero:")
            print(f"git clone https://github.com/cerudn/Digivice_D-tector-V2_Unity- {self.original_path}")
            sys.exit(1)
        
        # Crear estructura de carpetas en el destino
        self.refactored_path.mkdir(parents=True, exist_ok=True)
        
        # Migrar assets multimedia
        print("Copiando assets multimedia...")
        print()
        
        self.copy_folder("Assets/Sprites", "Assets/Sprites")
        self.copy_folder("Assets/Audio", "Assets/Audio")
        self.copy_folder("Assets/Fonts", "Assets/Fonts")
        self.copy_folder("Assets/Icons", "Assets/Icons")
        self.copy_folder("Assets/Prefab", "Assets/Prefab")
        self.copy_folder("Assets/Resources", "Assets/Resources")
        self.copy_folder("Assets/Scenes", "Assets/Scenes")
        
        print()
        print("Copiando configuració·´·n de Unity...")
        print()
        
        # Migrar configuració·´·n
        self.copy_folder("ProjectSettings", "ProjectSettings")
        self.copy_folder("Packages", "Packages")
        
        # Resumen
        print()
        print("=" * 60)
        print(f"✓ Migració·´·n completada!")
        print("=" * 60)
        print()
        print(f"Archivos copiados: {self.copied_count}")
        print(f"Archivos saltados: {self.skipped_count}")
        print(f"Errores: {self.error_count}")
        print()
        print("Pr ó—ximos pasos:")
        print("1. Abre Unity con el proyecto refactorizado")
        print("2. Ve a Tools > Digivice > Migrate V2 Data to SOs")
        print("3. Click en \"Start Full Migration\"")
        print("4. ¡Listo!")
        print()
    
    def copy_folder(self, src_rel: str, dest_rel: str):
        """Copia una carpeta desde el origen al destino."""
        src = self.original_path / src_rel
        dest = self.refactored_path / dest_rel
        
        if src.exists() and src.is_dir():
            print(f"[{self.get_folder_name(src_rel)}] Copiando {src_rel}...")
            
            try:
                # Si el destino ya existe, lo eliminamos primero
                if dest.exists():
                    shutil.rmtree(dest)
                    self.skipped_count += 1
                
                # Copiar carpeta
                shutil.copytree(src, dest)
                
                # Contar archivos copiados
                file_count = sum(1 for _ in dest.rglob('*') if _.is_file())
                self.copied_count += file_count
                
                print(f"  ✓ {src_rel} copiado ({file_count} archivos)")
            
            except Exception as e:
                print(f"  ✗ Error copiando {src_rel}: {e}")
                self.error_count += 1
        else:
            print(f"  ✗ {src_rel} no encontrado")
            self.skipped_count += 1
    
    def get_folder_name(self, path: str) -> str:
        """Obtiene el nombre de la carpeta para mostrar."""
        return path.split('/')[-1].split('\\')[-1]


def main():
    """Funció·´·n principal."""
    # Paths por defecto
    original_path = "../Digivice_D-tector-V2_Unity-"
    refactored_path = "."
    
    # Verificar argumentos
    if len(sys.argv) >= 2:
        original_path = sys.argv[1]
    
    if len(sys.argv) >= 3:
        refactored_path = sys.argv[2]
    
    # Crear migrador y ejecutar
    migrator = AssetMigrator(original_path, refactored_path)
    migrator.run()


if __name__ == "__main__":
    main()
