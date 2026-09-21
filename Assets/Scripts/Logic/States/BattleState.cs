using System;
using System.Collections.Generic;

/// <summary>
/// Estado de combate.
/// Maneja turnos, ataques, cálculos de daño y resolució·´·n de batalla.
/// </summary>
public class BattleState : IGameState
{
    private readonly ILogicManager logicManager;
    private readonly IInputManager inputManager;
    private readonly IAudioManager audioManager;
    private readonly ISpriteDatabase spriteDatabase;
    
    public GameState StateType => GameState.BATTLE;
    
    // Estado de batalla
    private BattleData battleData;
    private int currentTurnIndex;
    private bool isPlayerTurn;
    private float turnTimer;
    
    public BattleState(ILogicManager lm, IInputManager im, IAudioManager am, ISpriteDatabase sd)
    {
        logicManager = lm ?? throw new ArgumentNullException(nameof(lm));
        inputManager = im ?? throw new ArgumentNullException(nameof(im));
        audioManager = am ?? throw new ArgumentNullException(nameof(am));
        spriteDatabase = sd ?? throw new ArgumentNullException(nameof(sd));
    }
    
    public void Enter()
    {
        Console.WriteLine("[BattleState] Enter - Iniciando combate");
        
        // Inicializar datos de batalla
        battleData = new BattleData();
        currentTurnIndex = 0;
        turnTimer = GameConstants.TURN_TIME_LIMIT;
        
        // Configurar UI de batalla
        inputManager.EnableMenuInput();
        audioManager.PlayMusic(SoundIDs.MUSIC_BATTLE);
        
        // Determinar quién empieza (basado en velocidad)
        DetermineTurnOrder();
    }
    
    public void Update(float deltaTime)
    {
        // Actualizar timer de turno
        if (isPlayerTurn)
        {
            turnTimer -= deltaTime;
            
            if (turnTimer <= 0)
            {
                HandleTurnTimeout();
            }
        }
        
        // Actualizar animaciones de combate
        UpdateBattleAnimations(deltaTime);
    }
    
    public void Exit()
    {
        Console.WriteLine("[BattleState] Exit - Finalizando combate");
        
        // Limpiar estado de batalla
        battleData = null;
        inputManager.DisableMenuInput();
    }
    
    public void HandleInput(PlayerInput input)
    {
        if (!isPlayerTurn) return;
        
        switch (input.Type)
        {
            case InputType.MENU:
                HandleBattleMenuInput();
                break;
                
            case InputType.CONFIRM:
                HandleConfirmAttack();
                break;
                
            case InputType.CANCEL:
                HandleCancelAttack();
                break;
        }
    }
    
    private void DetermineTurnOrder()
    {
        // Ordenar participantes por velocidad
        var allParticipants = new List<Combatant>();
        allParticipants.AddRange(battleData.playerTeam);
        allParticipants.AddRange(battleData.enemyTeam);
        
        allParticipants.Sort((a, b) => b.Speed.CompareTo(a.Speed));
        
        battleData.turnOrder = allParticipants;
        isPlayerTurn = IsPlayerParticipant(battleData.turnOrder[0]);
    }
    
    private void HandleBattleMenuInput()
    {
        // Mostrar menú de combate (Atacar, Item, Huir)
        GameEvents.TriggerMenuOpened(MenuType.BATTLE);
    }
    
    private void HandleConfirmAttack()
    {
        // Ejecutar ataque seleccionado
        if (battleData.selectedAttack != null)
        {
            ExecuteAttack(battleData.selectedAttack);
        }
    }
    
    private void HandleCancelAttack()
    {
        // Volver al menú principal
        GameEvents.TriggerMenuClosed();
    }
    
    private void ExecuteAttack(Attack attack)
    {
        Combatant attacker = battleData.turnOrder[currentTurnIndex];
        Combatant defender = GetTargetForAttack(attack);
        
        // Disparar evento de ataque iniciado
        GameEvents.TriggerAttackStarted(attacker.Digimon, defender.Digimon, attack);
        
        // Calcular daño
        int damage = CalculateDamage(attacker, defender, attack);
        
        // Aplicar daño
        defender.TakeDamage(damage);
        
        // Disparar evento de daño
        GameEvents.TriggerDamageDealt(defender.Digimon, damage);
        
        // Verificar si el defensor se debilitó·´·
        if (defender.HP <= 0)
        {
            GameEvents.TriggerDigimonFainted(defender.Digimon);
            CheckBattleEnd();
        }
        
        // Pasar al siguiente turno
        NextTurn();
    }
    
    private int CalculateDamage(Combatant attacker, Combatant defender, Attack attack)
    {
        // Fó—rmula de daño simplificada
        float baseDamage = attack.Power * ((float)attacker.Attack / defender.Defense);
        
        // Bonus de tipo
        float typeMultiplier = GetTypeMultiplier(attack.Element, defender.Element);
        
        // Bonus STAB (Same Type Attack Bonus)
        float stabBonus = IsSameType(attack.Element, attacker.Element) ? 
            GameConstants.STAB_BONUS : 0f;
        
        // Crí·´­tico
        bool isCritical = UnityEngine.Random.value < GameConstants.CRITICAL_CHANCE;
        float criticalMultiplier = isCritical ? GameConstants.CRITICAL_MULTIPLIER : 1f;
        
        // Dañ·´·o final
        float finalDamage = baseDamage * typeMultiplier * (1f + stabBonus) * criticalMultiplier;
        
        return Mathf.RoundToInt(finalDamage);
    }
    
    private float GetTypeMultiplier(ElementType attackElement, ElementType defenderElement)
    {
        // Tabla de ventajas de tipo
        // Implementar segó·´·n sistema de tipos del juego
        return 1f; // Placeholder
    }
    
    private bool IsSameType(ElementType type1, ElementType type2)
    {
        return type1 == type2;
    }
    
    private void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % battleData.turnOrder.Count;
        isPlayerTurn = IsPlayerParticipant(battleData.turnOrder[currentTurnIndex]);
        turnTimer = GameConstants.TURN_TIME_LIMIT;
    }
    
    private bool IsPlayerParticipant(Combatant combatant)
    {
        return battleData.playerTeam.Contains(combatant);
    }
    
    private Combatant GetTargetForAttack(Attack attack)
    {
        // Determinar objetivo segó·´·n tipo de ataque
        return battleData.enemyTeam[0]; // Placeholder - primer enemigo
    }
    
    private void HandleTurnTimeout()
    {
        // Tiempo agotado - selecció·´·n automá·´­tica o huida
        Console.WriteLine("[BattleState] Tiempo de turno agotado");
        NextTurn();
    }
    
    private void CheckBattleEnd()
    {
        // Verificar si todos los Digimon de un lado están debilitados
        bool playerTeamAlive = battleData.playerTeam.Exists(c => c.HP > 0);
        bool enemyTeamAlive = battleData.enemyTeam.Exists(c => c.HP > 0);
        
        if (!playerTeamAlive)
        {
            GameEvents.TriggerBattleLost();
            Exit();
        }
        else if (!enemyTeamAlive)
        {
            GameEvents.TriggerBattleWon();
            Exit();
        }
    }
    
    private void UpdateBattleAnimations(float deltaTime)
    {
        // Actualizar animaciones de sprites
        // Partí·´­culas y efectos
    }
}

/// <summary>
/// Datos de una batalla en curso.
/// </summary>
[System.Serializable]
public class BattleData
{
    public List<Combatant> playerTeam;
    public List<Combatant> enemyTeam;
    public List<Combatant> turnOrder;
    public Attack selectedAttack;
    
    public BattleData()
    {
        playerTeam = new List<Combatant>();
        enemyTeam = new List<Combatant>();
        turnOrder = new List<Combatant>();
    }
}

/// <summary>
/// Participante en combate (Digimon o enemigo).
/// </summary>
[System.Serializable]
public class Combatant
{
    public Digimon Digimon;
    public int HP;
    public int MP;
    public int Attack;
    public int Defense;
    public int Speed;
    public ElementType Element;
    
    public void TakeDamage(int damage)
    {
        HP = Math.Max(0, HP - damage);
    }
}
