using Godot;

namespace TESTCS.actors;

public abstract partial class Actor : CharacterBody2D
{
    public int MaxHealth = 100;
    public int Health = 100;
    public float Weight = 10;
    public float Height = 0;
    public float MovementSpeed = 200;
    public bool IsAirborne = false;
    public ActorController Controller;
    public float VerticalVelocity = 0;

    public float RemainingHealthPercent()
    {
        return (float)Health / MaxHealth;
    }
    
    // Debug
    [Export]
    public bool DebugGlobalPosition { get; set; }
}