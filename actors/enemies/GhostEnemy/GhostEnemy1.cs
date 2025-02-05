    using Godot;
using TESTCS.actors.controllers;
using TESTCS.actors.enemies;

public partial class GhostEnemy1 : EnemyActor
{
    private Vector2 intialShadowScale;
    private float verticalVelocity = 0f; // Simulated vertical velocity
    
    private AnimatedSprite2D Sprite;
    private Sprite2D ShadowSprite;
    private HealthBar HealthBar;
    
    private float MAX_HIT_FORCE = 10;
    private float MAX_SHADOW_HEIGHT = 50;

    private Area2D _swipeHitbox;
    private bool _isSwiping = false;

    public override void _Ready()
    {
        this.MovementSpeed = 50;
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ShadowSprite = GetNode<Sprite2D>("ShadowSprite");
        HealthBar = GetNode<HealthBar>("%HealthBar");
        _swipeHitbox = GetNode<Area2D>("SwipeHitbox");
        _swipeHitbox.BodyEntered += OnBodyEnteredSwipeHitbox;
        _swipeHitbox.BodyExited += OnBodyExitedSwipeHitbox;
        intialShadowScale = ShadowSprite.Scale;
        Controller = new BasicEnemyController();
        
        HealthBar.MaxValue = MaxHealth;
        HealthBar.SetValue(Health);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        HealthBar.Value = Health;
        
        if (_isSwiping)
        {
            
        }
    }

    private void OnBodyExitedSwipeHitbox(Node2D body)
    {
        if (!_isSwiping)
        {
            Controller.EnableMovementInput();
        }
    }

    private void OnBodyEnteredSwipeHitbox(Node2D body)
    {
        _isSwiping = true;
        Controller.DisableMovementInput();
        GD.Print("ENEMY NOW IN RANGE");

        var attackIndicator = GetNode<Area2D>("AttackIndicator");   
        
        // Rotate towards player
        Vector2 direction = (body.GlobalPosition - GlobalPosition).Normalized();
        attackIndicator.Rotation = direction.Angle();
        attackIndicator.SetVisible(true);

        var indicatorTimer = new Timer();
        indicatorTimer.WaitTime = 1f;
        indicatorTimer.OneShot = true;
        indicatorTimer.Timeout += () =>
        {
            var bodies = attackIndicator.GetOverlappingBodies();
            foreach (var node2D in bodies)
            {
                if (node2D is IHittable hittable)
                {
                    GD.Print("ATTACK!!!");
                    hittable.ReceiveHit(new HitInformation(25, 100, Position));
                }
            }
            Controller.EnableMovementInput();
            attackIndicator.SetVisible(false);
        };
        AddChild(indicatorTimer);
        indicatorTimer.Start();

        // Start timer to remove indicator before attack
        // _indicatorTimer = new Timer();
        // _indicatorTimer.WaitTime = IndicatorDuration;
        // _indicatorTimer.OneShot = true;
        // _indicatorTimer.Timeout += () => attackIndicator.QueueFree();
        // AddChild(_indicatorTimer);
        // _indicatorTimer.Start();
    }

    public void OnLand()
    {
        // Do something when we land.
    }
}