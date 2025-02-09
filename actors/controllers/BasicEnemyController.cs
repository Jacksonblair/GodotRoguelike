using Godot;

namespace TESTCS.actors.controllers;

/**
 * Controller should be handling logic.
 * - If adversary in melee range, controller should try and have a crack at them.
 * - Controller determines if enemy aiming at someone or not.
 *
 * - Player enters hitbox
 * - Controller registers this, starts aiming, notifies that its aiming.
 * - After n seconds, controller checks if target can still be bit, and then executes hit. 
 */

public partial class BasicEnemyController : ActorController
{
    public override Vector2 GetMovementInput(Vector2 actorPosition)
    {
        if (MovementInputDisabled) return Vector2.Zero;
        
        if (TargetToMoveTo.HasValue) {
            if (actorPosition.DistanceTo(TargetToMoveTo.Value) < 5f)
            {
                GD.Print("REACHED TARGET");
                TargetToMoveTo = null;
            }
            else
            {
                return (TargetToMoveTo.Value - actorPosition).Normalized();
            }
        }
        
        // Calculate the direction vector from the actor to the player
        // return actorPosition;
        return (GlobalVariables.PlayerCharacter.Position - actorPosition).Normalized();
    }

    public override Vector2 GetAimDirection(Vector2 actorPosition)
    {
        // return actorPosition;
        return (GlobalVariables.PlayerCharacter.Position - actorPosition).Normalized();
    }
}