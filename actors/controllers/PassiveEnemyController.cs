using Godot;

namespace TESTCS.actors.controllers;

public partial class PassiveEnemyController : ActorController
{
    public override Vector2 GetMovementInput(Vector2 actorPosition)
    {
        // MOVE TO TARGET
        if (TargetToMoveTo.HasValue) {
            if (actorPosition.DistanceTo(TargetToMoveTo.Value) < 5f)
            {
                GD.Print("REACHED TARGET");
                TargetToMoveTo = null;
                EmitSignal(nameof(ReachedTarget));
            }
            else
            {
                var velocity = (TargetToMoveTo.Value - actorPosition).Normalized();
                return velocity;
            }
        }
        
        // FOLLOW PATH
        // TODO: PUT THIS FUNCTIONALITY SOMEWHERE GENERIC
        if (PathToFollow != null)
        {   
            // Get next position along the curve
            var points = PathToFollow.Curve.GetBakedPoints();

            var target = points[PathPointIndex];
            if (actorPosition.DistanceTo(target) < 5)
            {
                PathPointIndex += 1;
                if (PathPointIndex > points.Length - 1)
                {
                    CancelFollowPath();
                    return Vector2.Zero;
                }
                target = points[PathPointIndex];
            }
            
            var velocity = (target - actorPosition).Normalized();
            return velocity;
            
            // if position.distance_to(target) < 1:
            // patrol_index = wrapi(patrol_index + 1, 0, patrol_points.size())
            // target = patrol_points[patrol_index]
            // velocity = (target - position).normalized() * move_speed
            // velocity = move_and_slide(velocity)
            // Check if we reached the end
            // if (actorPosition.DistanceTo(splineTarget) < 5f)
            // {
            //     PathProgress += 0.1f; // Small step forward
            //     if (PathProgress >= PathToFollow.Curve.GetBakedLength())
            //     {
            //         PathToFollow = null;
            //         EmitSignal(nameof(ReachedTargetEventHandler)); // Notify completion
            //     }
            // }

            // return (splineTarget - actorPosition).Normalized();
        }
        
        return Vector2.Zero;
    }

    public override Vector2 GetAimDirection(Vector2 actorPosition)
    {
        return Vector2.Zero;
    }
}