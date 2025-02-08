using Godot;

namespace TESTCS.actors;

public abstract partial class Actor : CharacterBody2D
{
    public ActorController Controller;
    [Export] public int MaxHealth = 100;
    [Export] public int Health = 100;
    [Export] public float Weight = 10;
    [Export] public float Height = 0;
    [Export] public float MovementSpeed = 200;
    [Export] public bool IsAirborne = false;
    [Export] public float VerticalVelocity = 0;

    public float RemainingHealthPercent()
    {
        return (float)Health / MaxHealth;
    }
    
    // Debug
    // [Export]
    // public bool DebugGlobalPosition { get; set; }
}