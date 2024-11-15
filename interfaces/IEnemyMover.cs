using Godot;

public interface IEnemyMover
{
    void Move(CharacterBody2D origin, Node2D target, float speed);
}
