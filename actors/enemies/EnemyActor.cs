using System;
using Godot;

namespace TESTCS.actors.enemies;

public partial class EnemyActor : Actor, IHittable
{
    [Export]
    public HealthBar HealthBar { get; set; }
    [Export]
    public AnimatedSprite2D Sprite { get; set; }
    
    public override void _Process(double delta)
    {
        if (Controller.CanHitPosition(this.Position, GlobalVariables.PlayerCharacter.Position, 50f))
        {
            GD.Print("CAN HIT PLAYER");
            GlobalVariables.PlayerCharacter.ReceiveHit(new HitInformation(10, 100, this.Position));
        }
        
        if (IsAirborne)
        {
            Height += VerticalVelocity * (float)delta;
            VerticalVelocity -= GlobalVariables.Gravity * (float)delta;
            
            // Update sprites
            // TODO: HANDLE HEIGHT
            // Sprite.Position = new Vector2(Sprite.Position.X, -Height);
            
            // Check if the enemy "lands"
            if (Height <= 0)
            {
                Height = 0;
                IsAirborne = false;
                VerticalVelocity = 0;
                // OnLand();
            }
                        
            // Decay knockback horizontal velocity
            Velocity = Velocity.MoveToward(Vector2.Zero, 50f * (float)delta);
            
            // Move the enemy
            MoveAndSlide();
        }
        else
        {
            var velocity = Controller.GetMovementInput(this.Position);
            velocity = velocity.Normalized() * MovementSpeed;
            Velocity = velocity;
            MoveAndSlide();
        }
    }
    
    public void ReceiveHit(HitInformation hitInformation)
    {
        Health -= hitInformation.Damage;
        HealthBar.MaxValue = 1;
        HealthBar.Value = RemainingHealthPercent();
        
        // Divide enemy weight by hit weight to get force of knockback 
        // 10/5 == 2
        // 50/5 == 10
        // 2/1 == 2
        float knockbackForce = hitInformation.Weight / this.Weight * 25;
        
        // Calculate knockback direction
        Vector2 knockbackDirection = (GlobalPosition - hitInformation.Position).Normalized();
        
        // Apply knockback force to Vector
        Vector2 knockbackVector = knockbackDirection * knockbackForce;
        
        // Calculate the vertical lift (based on force magnitude and a multiplier)
        float verticalLift = knockbackForce * 0.5f; // Lift multiplier

        // Apply knockback
        Velocity += knockbackVector;
        VerticalVelocity = verticalLift;
        IsAirborne = true;
        
        // TODO: MOVE SOMEWHERE ELSE
        
        if (Health <= 0)
        {
            GD.Print("DYING");
            QueueFree();
        }
        
    }
}