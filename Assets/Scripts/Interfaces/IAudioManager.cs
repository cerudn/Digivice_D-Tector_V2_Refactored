using System;

/// <summary>
/// Interfaz para el gestor de audio.
/// </summary>
public interface IAudioManager
{
    /// <summary>
    /// Volumen actual de la música (0-1).
    /// </summary>
    float MusicVolume { get; set; }
    
    /// <summary>
    /// Volumen actual de los efectos de sonido (0-1).
    /// </summary>
    float SFXVolume { get; set; }
    
    /// <summary>
    /// Inicializa el sistema de audio.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Reproduce una pista de música.
    /// </summary>
    /// <param name="musicID">ID de la música</param>
    /// <param name="fade">Transición gradual</param>
    void PlayMusic(int musicID, bool fade = true);
    
    /// <summary>
    /// Detiene la música actual.
    /// </summary>
    /// <param name="fade">Transición gradual</param>
    void StopMusic(bool fade = true);
    
    /// <summary>
    /// Reproduce un efecto de sonido.
    /// </summary>
    /// <param name="sfxID">ID del efecto</param>
    /// <param name="volume">Volumen (0-1)</param>
    void PlaySFX(int sfxID, float volume = 1f);
    
    /// <summary>
    /// Reproduce un efecto de sonido en posición 3D.
    /// </summary>
    /// <param name="sfxID">ID del efecto</param>
    /// <param name="position">Posición en el mundo</param>
    void PlaySFXAt(int sfxID, UnityEngine.Vector3 position);
    
    /// <summary>
    /// Pausa o reanuda toda la reproducción de audio.
    /// </summary>
    /// <param name="paused">Estado de pausa</param>
    void SetPaused(bool paused);
    
    /// <summary>
    /// Evento disparado cuando la música cambia.
    /// </summary>
    event Action<int> OnMusicChanged;
}
