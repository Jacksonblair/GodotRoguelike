using Godot;
using TESTCS.helpers;

namespace TESTCS.actors.controllers;

public partial class PlayerController : ActorController
{
    public override void Update(double delta)
    {
        if (Input.IsActionJustPressed("block"))
        {
            GD.Print("BLOCKING");
            EmitSignal(nameof(StartedBlocking));
        }

        if (Input.IsActionJustReleased("block"))
        {
            GD.Print("STOP BLOCKING");
            EmitSignal(nameof(StoppedBlocking));
        }
        
        // if (Input.IsActionJustPressed("Skill1"))
        // {
        //     EmitSignal(nameof(AbilityPressed), 1);
        // }
        // if (Input.IsActionJustPressed("Skill2"))
        // {
        //     EmitSignal(nameof(AbilityPressed), 2);
        // }
        // if (Input.IsActionJustPressed("Skill3"))
        // {
        //     EmitSignal(nameof(AbilityPressed), 3);
        // }
        // if (Input.IsActionJustPressed("interact"))
        // {
        //     EmitSignal(nameof(Interacted));
        // }
    }
    
    public override Vector2 GetMovementInput(Vector2 actorPosition)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }

        return velocity;
    }

    public override Vector2 GetAimDirection(Vector2 actorPosition)
    {
        Vector2 joystickDirection = new Vector2(
            Input.GetActionStrength("aim_right") - Input.GetActionStrength("aim_left"),
            Input.GetActionStrength("aim_down") - Input.GetActionStrength("aim_up")
        ).Normalized();

        if (joystickDirection.Length() > 0)
        {
            _previousAimDirection = joystickDirection;
            return joystickDirection;
        } 
        else
        {
            return _previousAimDirection;
        }
        
        // TODO: Add controller based aiming
        // Mouse based aiming
        var mousePos = MiscHelper.GetActiveMainSceneMousePosition();
        if (mousePos.HasValue)
        {
            Vector2 direction = (mousePos.Value - actorPosition).Normalized();
            return direction;
        }
        else
        {
            return Vector2.Zero;
        }
    }
}