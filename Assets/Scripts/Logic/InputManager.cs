using System;
using UnityEngine;

/// <summary>
/// Gestor de input del jugador.
/// Soporta teclado, tá—ctil y gamepad.
/// </summary>
public class InputManager : MonoBehaviour, IInputManager
{
    #region Singleton
    private static InputManager _instance;
    
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                
                if (_instance == null)
                {
                    var go = new GameObject("InputManager");
                    _instance = go.AddComponent<InputManager>();
                    DontDestroyOnLoad(go);
                }
            }
            
            return _instance;
        }
    }
    #endregion
    
    #region IInputManager Implementation
    public bool InputEnabled { get; private set; }
    
    public event Action<PlayerInput> OnInputDetected;
    #endregion
    
    #region State
    private bool movementEnabled;
    private bool menuInputEnabled;
    private Vector2Int lastDirection;
    private float inputCooldown;
    #endregion
    
    #region Constants
    private const float INPUT_COOLDOWN = 0.15f;
    private const float MENU_NAVIGATION_COOLDOWN = 0.2f;
    #endregion
    
    #region Unity Lifecycle
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        Initialize();
    }
    
    void Update()
    {
        if (!InputEnabled) return;
        
        inputCooldown -= Time.deltaTime;
        
        // Procesar input
        ProcessKeyboardInput();
        ProcessTouchInput();
        ProcessGamepadInput();
    }
    #endregion
    
    #region IInputManager Methods
    public void Initialize()
    {
        Console.WriteLine("[InputManager] Inicializando...");
        
        InputEnabled = false;
        movementEnabled = false;
        menuInputEnabled = false;
        inputCooldown = 0f;
        
        Console.WriteLine("[InputManager] Inicializado");
    }
    
    public PlayerInput ReadInput()
    {
        if (!InputEnabled || inputCooldown > 0)
        {
            return new PlayerInput(InputType.NONE);
        }
        
        PlayerInput input = new PlayerInput(InputType.NONE);
        
        // Teclado
        if (movementEnabled)
        {
            input = ReadKeyboardMovement();
        }
        else if (menuInputEnabled)
        {
            input = ReadMenuInput();
        }
        
        // Tá—ctil
        if (input.Type == InputType.NONE)
        {
            input = ReadTouchInput();
        }
        
        // Gamepad
        if (input.Type == InputType.NONE)
        {
            input = ReadGamepadInput();
        }
        
        // Disparar evento
        if (input.Type != InputType.NONE)
        {
            OnInputDetected?.Invoke(input);
        }
        
        return input;
    }
    
    public void EnableMovement()
    {
        movementEnabled = true;
        menuInputEnabled = false;
        InputEnabled = true;
        Console.WriteLine("[InputManager] Movimiento habilitado");
    }
    
    public void DisableMovement()
    {
        movementEnabled = false;
        Console.WriteLine("[InputManager] Movimiento deshabilitado");
    }
    
    public void EnableMenuInput()
    {
        menuInputEnabled = true;
        movementEnabled = false;
        InputEnabled = true;
        Console.WriteLine("[InputManager] Input de men ú— habilitado");
    }
    
    public void DisableMenuInput()
    {
        menuInputEnabled = false;
        Console.WriteLine("[InputManager] Input de men ú— deshabilitado");
    }
    
    public void SetInputEnabled(bool enabled)
    {
        InputEnabled = enabled;
        Console.WriteLine($"[InputManager] Input {(enabled ? "habilitado" : "deshabilitado")}");
    }
    #endregion
    
    #region Keyboard Input
    private PlayerInput ReadKeyboardMovement()
    {
        Vector2Int direction = Vector2Int.zero;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2Int.right;
        }
        
        if (direction != Vector2Int.zero)
        {
            inputCooldown = INPUT_COOLDOWN;
            return new PlayerInput(InputType.MOVE, direction);
        }
        
        // Accó·´·n
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            inputCooldown = INPUT_COOLDOWN;
            return new PlayerInput(InputType.ACTION);
        }
        
        // Men ú—
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            inputCooldown = INPUT_COOLDOWN;
            return new PlayerInput(InputType.MENU);
        }
        
        return new PlayerInput(InputType.NONE);
    }
    
    private PlayerInput ReadMenuInput()
    {
        // Navegacó·´·n
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputCooldown = MENU_NAVIGATION_COOLDOWN;
            return new PlayerInput(InputType.MOVE, Vector2Int.up);
        }
        
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputCooldown = MENU_NAVIGATION_COOLDOWN;
            return new PlayerInput(InputType.MOVE, Vector2Int.down);
        }
        
        // Confirmar
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            inputCooldown = INPUT_COOLDOWN;
            var input = new PlayerInput(InputType.CONFIRM);
            input.IsConfirm = true;
            return input;
        }
        
        // Cancelar
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            inputCooldown = INPUT_COOLDOWN;
            var input = new PlayerInput(InputType.CANCEL);
            input.IsCancel = true;
            return input;
        }
        
        return new PlayerInput(InputType.NONE);
    }
    #endregion
    
    #region Touch Input
    private PlayerInput ReadTouchInput()
    {
        if (Input.touchCount == 0) return new PlayerInput(InputType.NONE);
        
        Touch touch = Input.GetTouch(0);
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
                // Tap simple - accó·´·n
                return new PlayerInput(InputType.ACTION);
                
            case TouchPhase.Moved:
                // Swipe - direccó·´·n
                Vector2 swipeDelta = touch.deltaPosition;
                
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // Horizontal
                    return new PlayerInput(
                        InputType.MOVE,
                        new Vector2Int(Mathf.Sign(swipeDelta.x), 0)
                    );
                }
                else
                {
                    // Vertical
                    return new PlayerInput(
                        InputType.MOVE,
                        new Vector2Int(0, Mathf.Sign(swipeDelta.y))
                    );
                }
                
            case TouchPhase.Ended:
                // Tap largo - men ú—
                if (touch.duration > 0.5f)
                {
                    return new PlayerInput(InputType.MENU);
                }
                break;
        }
        
        return new PlayerInput(InputType.NONE);
    }
    #endregion
    
    #region Gamepad Input
    private PlayerInput ReadGamepadInput()
    {
        // Implementar soporte para gamepad
        // Placeholder - usar Unity Input System o legacy
        
        return new PlayerInput(InputType.NONE);
    }
    #endregion
    
    #region Utility
    private void ProcessKeyboardInput()
    {
        // Ya implementado en ReadInput
    }
    
    private void ProcessTouchInput()
    {
        // Ya implementado en ReadInput
    }
    
    private void ProcessGamepadInput()
    {
        // Ya implementado en ReadInput
    }
    #endregion
}
