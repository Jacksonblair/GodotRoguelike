using Godot;
using TESTCS.actors.controllers;
using TESTCS.actors.enemies;

public partial class GhostEnemy1 : EnemyActor
{
    private Vector2 intialShadowScale;
    private float verticalVelocity = 0f; // Simulated vertical velocity
    
    private AnimatedSprite2D Sprite;
    private Sprite2D ShadowSprite;
    
    private float MAX_HIT_FORCE = 10;
    private float MAX_SHADOW_HEIGHT = 50;

    public override void _Ready()
    {
        this.MovementSpeed = 50;
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ShadowSprite = GetNode<Sprite2D>("ShadowSprite");
        HealthBar = GetNode<HealthBar>("%HealthBar");
        intialShadowScale = ShadowSprite.Scale;
        Controller = new BasicEnemyController();
    }

    public void OnLand()
    {
        // Do something when we land.
    }
}