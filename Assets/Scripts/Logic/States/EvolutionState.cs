using System;
using System.Collections.Generic;

/// <summary>
/// Estado de evolució·´·n de Digimon.
/// Maneja el proceso completo de digievolució·´·n.
/// </summary>
public class EvolutionState : IGameState
{
    private readonly ILogicManager logicManager;
    private readonly IScreenManager screenManager;
    private readonly IAudioManager audioManager;
    private readonly ISpriteDatabase spriteDatabase;
    
    public GameState StateType => GameState.EVOLUTION;
    
    // Estado de la evolució·´·n
    private EvolutionPhase currentPhase;
    private Digimon evolvingDigimon;
    private DigimonData targetForm;
    private float phaseTimer;
    private bool evolutionSuccess;
    
    public EvolutionState(ILogicManager lm, IScreenManager sm, IAudioManager am, ISpriteDatabase sd)
    {
        logicManager = lm ?? throw new ArgumentNullException(nameof(lm));
        screenManager = sm ?? throw new ArgumentNullException(nameof(sm));
        audioManager = am ?? throw new ArgumentNullException(nameof(am));
        spriteDatabase = sd ?? throw new ArgumentNullException(nameof(sd));
    }
    
    public void Enter()
    {
        Console.WriteLine("[EvolutionState] Enter - Iniciando evolució·´·n");
        
        currentPhase = EvolutionPhase.INIT;
        phaseTimer = 0f;
        evolutionSuccess = false;
        
        // Reproducir m ú—sica de evolució·´·n
        audioManager.PlayMusic(SoundIDs.MUSIC_EVOLUTION);
        
        // Mostrar UI de evolució·´·n
        screenManager.Transition(ScreenType.EVOLUTION);
        
        // Suscribirse a eventos
        GameEvents.OnEvolutionCompleted += OnEvolutionCompleted;
    }
    
    public void Update(float deltaTime)
    {
        phaseTimer += deltaTime;
        
        // Má—quina de estados de fases de evolució·´·n
        switch (currentPhase)
        {
            case EvolutionPhase.INIT:
                UpdateInitPhase(deltaTime);
                break;
                
            case EvolutionPhase.ANIMATION:
                UpdateAnimationPhase(deltaTime);
                break;
                
            case EvolutionPhase.TRANSFORMATION:
                UpdateTransformationPhase(deltaTime);
                break;
                
            case EvolutionPhase.REVEAL:
                UpdateRevealPhase(deltaTime);
                break;
                
            case EvolutionPhase.COMPLETE:
                UpdateCompletePhase(deltaTime);
                break;
        }
    }
    
    public void Exit()
    {
        Console.WriteLine("[EvolutionState] Exit - Finalizando evolució·´·n");
        
        // Limpiar estado
        evolvingDigimon = null;
        targetForm = null;
        
        // Desuscribirse de eventos
        GameEvents.OnEvolutionCompleted -= OnEvolutionCompleted;
    }
    
    public void HandleInput(PlayerInput input)
    {
        // Durante la evolució·´·n, input limitado
        if (input.IsCancel && currentPhase == EvolutionPhase.INIT)
        {
            // Cancelar evolució·´·n
            CancelEvolution();
        }
    }
    
    #region Phase Updates
    private void UpdateInitPhase(float deltaTime)
    {
        // Fase inicial - mostrar Digimon actual
        if (phaseTimer >= 1f)
        {
            currentPhase = EvolutionPhase.ANIMATION;
            phaseTimer = 0f;
            
            // Iniciar animacó·´·n de evolució·´·n
            StartEvolutionAnimation();
        }
    }
    
    private void UpdateAnimationPhase(float deltaTime)
    {
        // Fase de animacó·´·n - efectos de luz, part í—culas
        if (phaseTimer >= 2f)
        {
            currentPhase = EvolutionPhase.TRANSFORMATION;
            phaseTimer = 0f;
            
            // Aplicar transformacó·´·n
            ApplyTransformation();
        }
    }
    
    private void UpdateTransformationPhase(float deltaTime)
    {
        // Fase de transformacó·´·n - cambiar sprite, stats
        if (phaseTimer >= 1.5f)
        {
            currentPhase = EvolutionPhase.REVEAL;
            phaseTimer = 0f;
            
            // Mostrar nueva forma
            ShowNewForm();
        }
    }
    
    private void UpdateRevealPhase(float deltaTime)
    {
        // Fase de revelacó·´·n - mostrar nuevo Digimon
        if (phaseTimer >= 2f)
        {
            currentPhase = EvolutionPhase.COMPLETE;
            phaseTimer = 0f;
            
            // Reproducir sonido de completado
            audioManager.PlaySFX(SoundIDs.SFX_EVOLUTION);
            
            // Disparar evento
            GameEvents.TriggerEvolutionCompleted(evolvingDigimon);
            evolutionSuccess = true;
        }
    }
    
    private void UpdateCompletePhase(float deltaTime)
    {
        // Fase final - esperar input para continuar
        // Auto-salir después de 3 segundos
        if (phaseTimer >= 3f)
        {
            // Volver al estado anterior
            GameEvents.TriggerStateChanged(GameState.EXPLORATION);
        }
    }
    #endregion
    
    #region Evolution Logic
    /// <summary>
    /// Inicia el proceso de evolució·´·n.
    /// </summary>
    public void StartEvolution(Digimon digimon, DigimonData newForm)
    {
        evolvingDigimon = digimon;
        targetForm = newForm;
        
        Console.WriteLine($"[EvolutionState] {digimon.BaseName} → {newForm.digimonName}");
        
        // Disparar evento de inicio
        GameEvents.TriggerEvolutionStarted(digimon, new Digimon(newForm.digimonID));
    }
    
    private void StartEvolutionAnimation()
    {
        // Iniciar animacó·´·n de evolució·´·n
        // Part í—culas, efectos de luz, etc.
        Console.WriteLine("[EvolutionState] Iniciando animacó·´·n de evolucó·´·n");
    }
    
    private void ApplyTransformation()
    {
        // Aplicar cambios de stats
        evolvingDigimon.Stage = targetForm.stage;
        evolvingDigimon.BaseName = targetForm.digimonName;
        
        // Recalcular stats
        var newStats = targetForm.GetStatsAtLevel(evolvingDigimon.Level);
        evolvingDigimon.MaxHP = newStats.HP;
        evolvingDigimon.MaxMP = newStats.MP;
        evolvingDigimon.Attack = newStats.Attack;
        evolvingDigimon.Defense = newStats.Defense;
        evolvingDigimon.Speed = newStats.Speed;
        
        // Curar completamente
        evolvingDigimon.FullyHeal();
        
        Console.WriteLine($"[EvolutionState] Stats actualizados: HP={newStats.HP}, ATK={newStats.Attack}");
    }
    
    private void ShowNewForm()
    {
        // Mostrar sprite del nuevo Digimon
        var newSprite = spriteDatabase.GetDigimonSprite(targetForm.digimonID);
        
        // Actualizar UI
        screenManager.ShowToast($" ¡{evolvingDigimon.BaseName} ha evolucionado!", 3f);
    }
    
    private void CancelEvolution()
    {
        Console.WriteLine("[EvolutionState] Evolucó·´·n cancelada");
        
        // Reproducir sonido de cancelacó·´·n
        audioManager.PlaySFX(SoundIDs.SFX_CANCEL);
        
        // Volver al estado anterior
        GameEvents.TriggerStateChanged(GameState.EXPLORATION);
    }
    
    private void OnEvolutionCompleted(Digimon digimon)
    {
        Console.WriteLine($"[EvolutionState] Evolucó·´·n de {digimon.BaseName} completada");
        evolutionSuccess = true;
    }
    #endregion
    
    #region Utility
    /// <summary>
    /// Verifica si la evolucó·´·n fue exitosa.
    /// </summary>
    public bool WasSuccessful() => evolutionSuccess;
    
    /// <summary>
    /// Obtiene el Digimon evolucionado.
    /// </summary>
    public Digimon GetEvolvedDigimon() => evolvingDigimon;
    #endregion
}

/// <summary>
/// Fases del proceso de evolucó·´·n.
/// </summary>
public enum EvolutionPhase
{
    INIT = 0,
    ANIMATION = 1,
    TRANSFORMATION = 2,
    REVEAL = 3,
    COMPLETE = 4
}
