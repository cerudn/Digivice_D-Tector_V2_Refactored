using System;

/// <summary>
/// Interfaz para el gestor de entrada del jugador.
/// </summary>
public interface IInputManager
{
    /// <summary>
    /// Indica si la entrada está habilitada.
    /// </summary>
    bool InputEnabled { get; }
    
    /// <summary>
    /// Inicializa el sistema de entrada.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Lee la entrada actual del jugador.
    /// </summary>
    /// <returns>Entrada procesada</returns>
    PlayerInput ReadInput();
    
    /// <summary>
    /// Habilita la entrada de movimiento.
    /// </summary>
    void EnableMovement();
    
    /// <summary>
    /// Deshabilita la entrada de movimiento.
    /// </summary>
    void DisableMovement();
    
    /// <summary>
    /// Habilita la entrada de menú.
    /// </summary>
    void EnableMenuInput();
    
    /// <summary>
    /// Deshabilita la entrada de menú.
    /// </summary>
    void DisableMenuInput();
    
    /// <summary>
    /// Establece si el input está bloqueado.
    /// </summary>
    /// <param name="enabled">Estado de habilitación</param>
    void SetInputEnabled(bool enabled);
    
    /// <summary>
    /// Evento disparado cuando se detecta entrada.
    /// </summary>
    event Action<PlayerInput> OnInputDetected;
}
