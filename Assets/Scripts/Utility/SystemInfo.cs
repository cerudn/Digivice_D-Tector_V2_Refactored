using UnityEngine;

/// <summary>
/// Informacó·´·n del sistema y dispositivo.
/// Ú—til para debugging y optimizacó·´·n.
/// </summary>
public static class SystemInfoHelper
{
    /// <summary>
    /// Obtiene informacó·´·n completa del sistema.
    /// </summary>
    public static string GetSystemInfo()
    {
        var sb = new System.Text.StringBuilder();
        
        sb.AppendLine("=== SYSTEM INFO ===");
        sb.AppendLine($"Device: {SystemInfo.deviceName}");
        sb.AppendLine($"Model: {SystemInfo.deviceModel}");
        sb.AppendLine($"OS: {SystemInfo.operatingSystem}");
        sb.AppendLine($"Memory: {SystemInfo.systemMemorySize} MB");
        sb.AppendLine($"GPU: {SystemInfo.graphicsDeviceName}");
        sb.AppendLine($"GPU Memory: {SystemInfo.graphicsMemorySize} MB");
        sb.AppendLine($"Resolution: {Screen.width}x{Screen.height}");
        sb.AppendLine($"Refresh Rate: {Screen.currentResolution.refreshRate} Hz");
        sb.AppendLine($"Unity Version: {Application.unityVersion}");
        sb.AppendLine($"Platform: {Application.platform}");
        sb.AppendLine($"Processor Count: {SystemInfo.processorCount}");
        sb.AppendLine($"Supports Gyro: {SystemInfo.supportsGyroscope}");
        sb.AppendLine($"Supports Location: {SystemInfo.supportsLocationService}");
        sb.AppendLine("====================");
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Verifica si el dispositivo cumple los requisitos mí—nimos.
    /// </summary>
    public static bool MeetsMinimumRequirements()
    {
        // Requisitos mí—nimos
        const int MIN_MEMORY_MB = 1024;
        const int MIN_GPU_MEMORY_MB = 256;
        
        if (SystemInfo.systemMemorySize < MIN_MEMORY_MB)
        {
            Console.Warning($"[SystemInfo] Memoria insuficiente: {SystemInfo.systemMemorySize} MB < {MIN_MEMORY_MB} MB");
            return false;
        }
        
        if (SystemInfo.graphicsMemorySize < MIN_GPU_MEMORY_MB)
        {
            Console.Warning($"[SystemInfo] Memoria GPU insuficiente: {SystemInfo.graphicsMemorySize} MB < {MIN_GPU_MEMORY_MB} MB");
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Obtiene el nivel de calidad recomendado.
    /// </summary>
    public static int GetRecommendedQualityLevel()
    {
        // Basado en memoria y GPU
        if (SystemInfo.systemMemorySize >= 4096 && SystemInfo.graphicsMemorySize >= 1024)
        {
            return 2; // High
        }
        else if (SystemInfo.systemMemorySize >= 2048 && SystemInfo.graphicsMemorySize >= 512)
        {
            return 1; // Medium
        }
        else
        {
            return 0; // Low
        }
    }
    
    /// <summary>
    /// Imprime informacó·´·n del sistema en consola.
    /// </summary>
    public static void PrintSystemInfo()
    {
        Console.WriteLine(GetSystemInfo());
    }
}
