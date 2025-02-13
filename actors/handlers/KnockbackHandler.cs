using Godot;

namespace TESTCS.actors.handlers;


/**
 * Each actor has a KnockbackHandler
 * - All i want is a class i can subclass, so actors can have their own knockback handler if they want.
 */
public class KnockbackHandler
{
    public void ApplyKnockbackFriction(double delta, Actor actor)
    {
        if (!actor.IsKnockedBack) return;
        
        var updatedVelocity = Vector2.Zero;
        var knockbackFriction = 200f; // Friction to apply to knockback velocity
        var knockbackEndThreshold = 50f; // Threshold for ending knockback
            
        updatedVelocity = actor.Velocity.MoveToward(Vector2.Zero, knockbackFriction * (float)delta);

        if (updatedVelocity.Length() <= knockbackEndThreshold)
        {
            actor.IsKnockedBack = false;
        }

        actor.Velocity = updatedVelocity;
    }
    
    public void ApplyKnockbackForce(HitInformation hitInformation, Actor actor)
    {
        // Divide enemy weight by hit weight to get force of knockback 
        // 10/5 == 2
        // 50/5 == 10
        // 2/1 == 2
        float knockbackForceMultiplier = (hitInformation.Weight / actor.Weight * 25);
        
        // If hit is blocked
        if (actor.IsHitBlocked(hitInformation))
        {
            var tempBlockCushionEffect = 0.1f;
            knockbackForceMultiplier *= tempBlockCushionEffect;
        }
        
        // Calculate knockback direction
        Vector2 knockbackDirection = (actor.GlobalPosition - hitInformation.PositionOfHit).Normalized();
        
        // Apply knockback force multiplier to Vector
        Vector2 knockbackVelocity = knockbackDirection * knockbackForceMultiplier;
        
        // Apply knockback
        actor.Velocity = knockbackVelocity;
        
        // TODO: Determine if knockback is applied
        actor.IsKnockedBack = true;
        
        // Emit signal to say actor was interrupted
        actor.EmitSignal(nameof(actor.ActorInterrupted));
    }
}