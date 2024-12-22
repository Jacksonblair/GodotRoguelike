using Godot;

class TowardsTargetMover
{
    public TowardsTargetMover(Node2D target)
    {
        this.target = target;
    }
    
    public Node2D target { get; set; }
    
    public void Move(CharacterBody2D origin, float speed)
    {
        // Calculate the direction vector from the enemy to the target
        Vector2 direction = (target.Position - origin.Position).Normalized();

        // Move the enemy towards the target
        origin.Velocity = direction * speed;
        origin.MoveAndSlide();
    }
}
