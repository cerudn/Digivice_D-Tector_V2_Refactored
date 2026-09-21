using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// Sistema de cifrado para PlayerPrefs.
/// Proporciona seguridad b á—sica para datos sensibles.
/// </summary>
public static class EncryptedPlayerPrefs
{
    #region Constants
    private const string ENCRYPTION_KEY = "DigiviceEncryptionKey2024";
    private const string SALT = "DigiviceSaltV2";
    #endregion
    
    #region Set Methods
    /// <summary>
    /// Guarda un string encriptado.
    /// </summary>
    public static void SetString(string key, string value)
    {
        string encrypted = Encrypt(value);
        PlayerPrefs.SetString(key, encrypted);
    }
    
    /// <summary>
    /// Guarda un int encriptado.
    /// </summary>
    public static void SetInt(string key, int value)
    {
        string encrypted = Encrypt(value.ToString());
        PlayerPrefs.SetString(key, encrypted);
    }
    
    /// <summary>
    /// Guarda un float encriptado.
    /// </summary>
    public static void SetFloat(string key, float value)
    {
        string encrypted = Encrypt(value.ToString());
        PlayerPrefs.SetString(key, encrypted);
    }
    #endregion
    
    #region Get Methods
    /// <summary>
    /// Obtiene un string desencriptado.
    /// </summary>
    public static string GetString(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return string.Empty;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        return Decrypt(encrypted);
    }
    
    /// <summary>
    /// Obtiene un string desencriptado con valor por defecto.
    /// </summary>
    public static string GetString(string key, string defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        return Decrypt(encrypted);
    }
    
    /// <summary>
    /// Obtiene un int desencriptado.
    /// </summary>
    public static int GetInt(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return 0;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        string decrypted = Decrypt(encrypted);
        
        if (int.TryParse(decrypted, out int value))
        {
            return value;
        }
        
        return 0;
    }
    
    /// <summary>
    /// Obtiene un int desencriptado con valor por defecto.
    /// </summary>
    public static int GetInt(string key, int defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        string decrypted = Decrypt(encrypted);
        
        if (int.TryParse(decrypted, out int value))
        {
            return value;
        }
        
        return defaultValue;
    }
    
    /// <summary>
    /// Obtiene un float desencriptado.
    /// </summary>
    public static float GetFloat(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return 0f;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        string decrypted = Decrypt(encrypted);
        
        if (float.TryParse(decrypted, out float value))
        {
            return value;
        }
        
        return 0f;
    }
    
    /// <summary>
    /// Obtiene un float desencriptado con valor por defecto.
    /// </summary>
    public static float GetFloat(string key, float defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        
        string encrypted = PlayerPrefs.GetString(key);
        string decrypted = Decrypt(encrypted);
        
        if (float.TryParse(decrypted, out float value))
        {
            return value;
        }
        
        return defaultValue;
    }
    #endregion
    
    #region Utility
    /// <summary>
    /// Verifica si existe una clave.
    /// </summary>
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    
    /// <summary>
    /// Elimina una clave.
    /// </summary>
    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    
    /// <summary>
    /// Guarda todos los cambios.
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Elimina todas las claves.
    /// </summary>
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
    
    #region Encryption
    private static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return string.Empty;
        }
        
        try
        {
            using (Aes aes = Aes.Create())
            {
                // Derivar key desde password
                var keyDerivation = new Rfc2898DeriveBytes(
                    ENCRYPTION_KEY,
                    Encoding.UTF8.GetBytes(SALT),
                    1000,
                    HashAlgorithmName.SHA256
                );
                
                aes.Key = keyDerivation.GetBytes(32);
                aes.IV = keyDerivation.GetBytes(16);
                
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error($"[EncryptedPlayerPrefs] Error encriptando: {ex.Message}");
            return plainText; // Fallback sin encriptar
        }
    }
    
    private static string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
        {
            return string.Empty;
        }
        
        try
        {
            using (Aes aes = Aes.Create())
            {
                var keyDerivation = new Rfc2898DeriveBytes(
                    ENCRYPTION_KEY,
                    Encoding.UTF8.GetBytes(SALT),
                    1000,
                    HashAlgorithmName.SHA256
                );
                
                aes.Key = keyDerivation.GetBytes(32);
                aes.IV = keyDerivation.GetBytes(16);
                
                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error($"[EncryptedPlayerPrefs] Error desencriptando: {ex.Message}");
            return encryptedText; // Fallback sin desencriptar
        }
    }
    #endregion
}
