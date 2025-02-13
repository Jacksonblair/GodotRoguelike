using Godot;
using TESTCS.actors.handlers;
using TESTCS.managers.PlayerManagers;

namespace TESTCS.actors;

public abstract partial class Actor : CharacterBody2D
{
    public ActorController Controller;
    [Export] public int MaxHealth = 100;
    [Export] public int Health = 100;
    [Export] public float Weight = 10;
    [Export] public float Height = 0;
    [Export] public float MovementSpeed = 200;
    public SpriteAnimationManager AnimationManager;

    // Knockback
    public KnockbackHandler KnockbackHandler = new();
    [Export] public bool IsKnockedBack = false;
    
    // Blocking vars
    public Vector2 BlockDirection = Vector2.Zero;
    public float BlockAngle = 0;
    private bool _isBlocking = false;

    public bool IsBlocking
    {
        get => _isBlocking;
        set => _isBlocking = value;
    }

    [Signal] public delegate void ActorStunnedEventHandler();
    [Signal] public delegate void ActorInterruptedEventHandler();

    public float RemainingHealthPercent()
    {
        return (float)Health / MaxHealth;
    }
    
    // Debug
    // [Export]
    // public bool DebugGlobalPosition { get; set; }

    public bool IsHitBlocked(HitInformation hitInformation)
    {
        if (!IsBlocking) return false;
        var hitDir = (GlobalPosition - hitInformation.PositionOfHit).Normalized();
        float angleBetween = Mathf.RadToDeg(BlockDirection.AngleTo(hitDir));
        return Mathf.Abs(angleBetween) <= (BlockAngle / 2f);
    }

    protected void ProcessHitDamage(HitInformation hitInformation)
    {
        // TODO: Put these const somewhere else
        var blockCushion = 1f;
        const float blockCushionMultiplier = 0.25f;
        if (IsBlocking)
        {
            blockCushion = blockCushionMultiplier;
        }
        
        var dmg = (int)((float)hitInformation.Damage * blockCushion);
        Health -= dmg;
        
        GD.Print("HIT WITH: ", hitInformation);
        GD.Print("FINAL DMG: ", dmg);
        
        if (Health <= 0)
        {
            GD.Print("YOU DIED");
            Position = GV.ActiveMainSceneContainer.GlobalPosition;
            Velocity = Vector2.Zero;
            Health = MaxHealth;
        }
    }
}