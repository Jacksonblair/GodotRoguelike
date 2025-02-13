using System;
using Godot;

namespace TESTCS.actors.enemies;

/**
 * I need to make the enemy ability interruptable.
 *
 *
 * 
 */

public partial class EnemyActor : Actor, IHittable
{
    public override void _Process(double delta)
    {
        if (IsKnockedBack)
        {
            KnockbackHandler.ApplyKnockbackFriction(delta, this);
        }
        else
        {
            var velocity = Controller.GetMovementInput(this.Position);
            velocity = velocity.Normalized() * MovementSpeed;
            Velocity = velocity;
        }
        
        MoveAndSlide();
    }
    
    public void ReceiveHit(HitInformation hitInformation)
    {
        KnockbackHandler.ApplyKnockbackForce(hitInformation, this);
        ProcessHitDamage(hitInformation);
        
        if (Health <= 0)
        {
            GD.Print("DYING");
            QueueFree();
        }
    }
}