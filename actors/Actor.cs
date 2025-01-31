using Godot;

namespace TESTCS.actors;

public partial class Actor : CharacterBody2D
{
    public float MovementSpeed { get; set; }
    public ActorController Controller;
    
    // Debug
    [Export]
    public bool DebugVelocity { get; set; }

    // public override void _PhysicsProcess(double delta)
    // {
    //     if (Controller == null) return;
    //
    //     Vector2 movement = Controller.GetMovementInput(this.Position);
    //     Velocity = movement * Speed;
    //
    //     MoveAndSlide();
    // }
    //
    // private void Interact()
    // {
    //     GD.Print($"{Name} interacted with something!");
    // }
}