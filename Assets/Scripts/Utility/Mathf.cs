using System;

/// <summary>
/// Utilidades matemá·´­ticas b á—sicas (compatibilidad sin UnityEngine).
/// </summary>
public static class Mathf
{
    public static int FloorToInt(float f) => (int)Math.Floor(f);
    
    public static int CeilToInt(float f) => (int)Math.Ceiling(f);
    
    public static int RoundToInt(float f) => (int)Math.Round(f);
    
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    
    public static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * Clamp01(t);
    }
    
    public static float Clamp01(float value)
    {
        return Clamp(value, 0f, 1f);
    }
    
    public static float InverseLerp(float a, float b, float value)
    {
        if (a == b) return 0f;
        return Clamp01((value - a) / (b - a));
    }
}
