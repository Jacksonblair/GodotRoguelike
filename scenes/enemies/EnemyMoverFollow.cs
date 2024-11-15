using Godot;

class EnemyMoverFollow : IEnemyMover
{
    public void Move(CharacterBody2D origin, Node2D target, float speed)
    {
        // Calculate the direction vector from the enemy to the player
        Vector2 direction = (target.Position - origin.Position).Normalized();

        // Move the enemy towards the player
        origin.Velocity = direction * speed;
        origin.MoveAndSlide();
    }
}
