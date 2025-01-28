using System;
using System.Security.Cryptography.X509Certificates;
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
    
    private float MAX_HIT_FORCE = 10;
    private float MAX_SHADOW_HEIGHT = 50;
    
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
        var maxShadowScale = intialShadowScale * 0.5f;
        var heightEffect = Mathf.Clamp(Height, 0, MAX_SHADOW_HEIGHT);
        var normalisedHeightEffect = heightEffect / MAX_SHADOW_HEIGHT;
        
        // Lerp between initial shadow scale and max shadow scale   
        ShadowSprite.Scale = intialShadowScale.Lerp(intialShadowScale - maxShadowScale, normalisedHeightEffect);

        if (IsAirborne)
        {
            // GD.Print(maxShadowScale);
            // GD.Print(normalisedHeightEffect);
            
            Height += verticalVelocity * (float)delta;

            // GD.Print("HEIGHT", Height);
            // GD.Print("VERTICAL VELOCITY", verticalVelocity);
            
            verticalVelocity -= gravity * (float)delta;
            
            // Update sprites
            Sprite.Position = new Vector2(Sprite.Position.X, -Height);
            
            // Check if the enemy "lands"
            if (Height <= 0)
            {
                // GD.Print("LANDED");
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
        
        // Divide enemy weight by hit weight to get force of knockback 
        // 10/5 == 2
        // 50/5 == 10
        // 2/1 == 2
        float knockbackForce = (Math.Max(hitInformation.Weight / this.Weight, MAX_HIT_FORCE)) * 25;
        
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
        var progressBar = Sprite.GetNode<ProgressBar>("ProgressBar");
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