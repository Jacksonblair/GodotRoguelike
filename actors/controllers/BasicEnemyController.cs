using Godot;

namespace TESTCS.actors.controllers;

public partial class BasicEnemyController : ActorController
{
    public override Vector2 GetMovementInput(Vector2 actorPosition)
    {
        if (MovementInputDisabled) return Vector2.Zero;
        
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