/// <summary>
/// Constantes relacionadas con evolución y formas.
/// </summary>
public static class EvolutionConstants
{
    #region Stages
    public const int STAGE_FRESH = 0;
    public const int STAGE_IN_TRAINING = 1;
    public const int STAGE_ROOKIE = 2;
    public const int STAGE_CHAMPION = 3;
    public const int STAGE_ULTIMATE = 4;
    public const int STAGE_MEGA = 5;
    public const int STAGE_SPIRIT = 6;
    #endregion
    
    #region Evolution Types
    public const int EVOLVE_LEVEL = 1;
    public const int EVOLVE_ITEM = 2;
    public const int EVOLVE_FRIENDSHIP = 3;
    public const int EVOLVE_SPECIAL = 4;
    public const int DIGIVOLVE_SPIRIT = 5;
    public const int BEAST_SPIRIT = 6;
    public const int FUSION = 7;
    #endregion
    
    #region Requirements
    /// <summary>
    /// Probabilidad máxima de evolución (0-1).
    /// </summary>
    public const float MAX_EVOLUTION_CHANCE = 1.0f;
    
    /// <summary>
    /// Probabilidad mínima de evolución (0-1).
    /// </summary>
    public const float MIN_EVOLUTION_CHANCE = 0.1f;
    
    /// <summary>
    /// Bonus por amistad máxima a probabilidad de evolución.
    /// </summary>
    public const float MAX_FRIENDSHIP_BONUS = 0.3f;
    
    /// <summary>
    /// Tiempo mínimo entre evoluciones en segundos.
    /// </summary>
    public const float MIN_TIME_BETWEEN_EVOLUTIONS = 30f;
    #endregion
}
