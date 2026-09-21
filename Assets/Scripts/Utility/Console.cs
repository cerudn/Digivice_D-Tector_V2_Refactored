using System;

/// <summary>
/// Wrapper para logging consistente.
/// </summary>
public static class Console
{
    public static void WriteLine(string message)
    {
        System.Console.WriteLine($"[LOG] {message}");
    }
    
    public static void Log(string message)
    {
        System.Console.WriteLine($"[LOG] {message}");
    }
    
    public static void Warning(string message)
    {
        System.Console.WriteLine($"[WARNING] {message}");
    }
    
    public static void Error(string message)
    {
        System.Console.WriteLine($"[ERROR] {message}");
    }
    
    public static void Debug(string message)
    {
        #if DEBUG
        System.Console.WriteLine($"[DEBUG] {message}");
        #endif
    }
}
