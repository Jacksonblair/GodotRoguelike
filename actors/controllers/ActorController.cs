using Godot;

namespace TESTCS.actors;

/**
 * Moving
 * - Player moves depending on WASD.
 * - Enemy moves depending on some movement generation (follow the player)
 *
 * So...
 * If i were to swap out the controllers.
 *
 * EnemyActor needs to be moved towards the player
 * Player needs to be moved towards wherever the vector is
 * - The common thing is that each Actor gets given a vector which they should update their movement to? 
 *
 * Aiming. Same thing. Generate a vector. 
 */

public abstract partial class ActorController : Node
{
    [Signal] public delegate void AbilityPressedEventHandler(int abilityIndex);
    [Signal] public delegate void InteractedEventHandler();
    
    public abstract Vector2 GetMovementInput(Vector2 actorPosition);
    public abstract Vector2 GetAimDirection(Vector2 actorPosition);
    
    
    /**
     * If in range of player:
     * - Get direction of player from me
     * - Attack in that direction.
     * - Pray it hits something.
     */
    
    public bool CanHitPosition(Vector2 actorPosition, Vector2 targetPosition, float range)
    {
        var distance = actorPosition.DistanceTo(targetPosition);
        return distance <= range;
    }
}