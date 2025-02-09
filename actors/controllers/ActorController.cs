using System;
using Godot;

namespace TESTCS.actors;

public abstract partial class ActorController : Node
{
    [Signal] public delegate void AbilityPressedEventHandler(int abilityIndex);
    [Signal] public delegate void StartedBlockingEventHandler();
    [Signal] public delegate void StoppedBlockingEventHandler();
    [Signal] public delegate void InteractedEventHandler();
    [Signal] public delegate void ReachedTargetEventHandler();

    protected Vector2? TargetToMoveTo;
    protected Path2D PathToFollow;
    protected float PathProgress = 0f;
    protected int PathPointIndex = 0;
    
    protected Vector2 _previousAimDirection;
    
    public abstract Vector2 GetMovementInput(Vector2 actorPosition);
    public abstract Vector2 GetAimDirection(Vector2 actorPosition);

    public virtual void Update(double delta) {}
    
    public void MoveToTarget(Vector2 target)
    {
        TargetToMoveTo = target;
    }

    public void CancelToMoveTarget()
    {
        TargetToMoveTo = null;
    }

    public void FollowPath(Path2D path)
    {
        PathToFollow = path;
        PathProgress = 0f;
        PathPointIndex = 0;
    }

    public void CancelFollowPath()
    {
        PathToFollow = null;
        PathProgress = 0f;
        PathPointIndex = 0;
    }

    protected bool MovementInputDisabled;

    /**
     * If in range of player:
     * - Get direction of player from me
     * - Attack in that direction.
     * - Pray it hits something.
     */

    public void DisableMovementInput()
    {
        MovementInputDisabled = true;
    }
    
    public void EnableMovementInput()
    {
        MovementInputDisabled = false;
    }
    
    public bool CanHitPosition(Vector2 actorPosition, Vector2 targetPosition, float range)
    {
        var distance = actorPosition.DistanceTo(targetPosition);
        return distance <= range;
    }
}