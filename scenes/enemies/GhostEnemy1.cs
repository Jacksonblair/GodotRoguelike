using System;
using Godot;
using TESTCS.enums;

public partial class GhostEnemy1 : BaseEnemy, IHittable
{
    float max_health = 100;
    float health = 100;
    private readonly TowardsTargetMover _mover;
    float speed = 50;
    private Vector2 intialShadowScale;
    
    private float verticalVelocity = 0f; // Simulated vertical velocity
    private float gravity = 200f; // Simulated gravity
    

    private AnimatedSprite2D Sprite;
    private Sprite2D ShadowSprite;
    
    GhostEnemy1() : base(EnemyType.Ghost1)
    {
        _mover = new TowardsTargetMover(GlobalVariables.Instance.Character);
    }

    public override void _Ready()
    {
        _mover.target = GlobalVariables.Instance.Character;
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ShadowSprite = GetNode<Sprite2D>("ShadowSprite");
        intialShadowScale = ShadowSprite.Scale;
    }

    public override void _Process(double delta)
    {

        // TODO: SCALE THE SHADOW UP AND DOWN BASED ON EHGITH OVER GROUND
        
        if (IsAirborne)
        {
            // Vertical velocity starts negative, and it gets scaled higher.        
            Height += verticalVelocity * (float)delta;

            GD.Print("HEIGHT", Height);
            GD.Print("VERTICAL VELOCITY", verticalVelocity);
            
            verticalVelocity -= gravity * (float)delta;
            
            // Update sprites
            Sprite.Position = new Vector2(Sprite.Position.X, -Height);
            
            // Check if the enemy "lands"
            if (Height <= 0)
            {
                GD.Print("LANDED");
                Height = 0;
                IsAirborne = false;
                verticalVelocity = 0;
                OnLand();
            }
                        
            // Decay knockback horizontal velocity
            Velocity = Velocity.MoveToward(Vector2.Zero, 50f * (float)delta);
            
            // Move the enemy
            MoveAndSlide();
        }
        else
        {
            _mover?.Move(this, speed);
        }

    }

    public void OnLand()
    {
        // Do something when we land.
    }

    // TODO: Move to base enemy class, every enemy will do this. 
    public void ReceiveHit(HitInformation hitInformation)
    {   
        health -= hitInformation.Damage;
        var healthNormalized = health / max_health;
        
        // TODO: MOVE INTO A HANDLER

        var hitForce = hitInformation.Weight * 100;
        float knockbackForce = hitForce / this.Weight;
        
        // Calculate knockback direction
        Vector2 knockbackDirection = (this.GlobalPosition - hitInformation.Position).Normalized();
        
        // Apply knockback force to Vector
        Vector2 knockbackVector = knockbackDirection * knockbackForce;
        
        // Calculate the vertical lift (based on force magnitude and a multiplier)
        float verticalLift = knockbackForce * 0.5f; // Lift multiplier

        // Apply knockback
        this.Velocity += knockbackVector;
        verticalVelocity = verticalLift;
        IsAirborne = true;
        
        // GD.Print(healthNormalized);
        var progressBar = GetNode<ProgressBar>("ProgressBar");
        // GD.Print(progressBar.Value);
        progressBar.Value = healthNormalized;
        // GD.Print(progressBar.Value);
        
        if (health <= 0)
        {
            GD.Print("DYING");
            Die();
            QueueFree();
        }
    }
}