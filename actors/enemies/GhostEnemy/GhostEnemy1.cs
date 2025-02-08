    using System;
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

    private Area2D _fireballHitbox;
    private bool _isInFireballHitbox = false;
    private double _fireballCooldown = 3;
    private double _fireballCooldoownTimeLeft = 0;

    public override void _Ready()
    {
        this.MovementSpeed = 50;
        
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ShadowSprite = GetNode<Sprite2D>("ShadowSprite");
        HealthBar = GetNode<HealthBar>("%HealthBar");
        
        _swipeHitbox = GetNode<Area2D>("SwipeHitbox");
        _swipeHitbox.BodyEntered += OnBodyEnteredSwipeHitbox;
        _swipeHitbox.BodyExited += OnBodyExitedSwipeHitbox;
        
        _fireballHitbox = GetNode<Area2D>("FireballHitbox");
        _fireballHitbox.BodyEntered += OnBodyEnteredFireballHitbox;
        _fireballHitbox.BodyExited += OnBodyExitedFireballHitbox;
        
        intialShadowScale = ShadowSprite.Scale;
        Controller = new BasicEnemyController();

        HealthBar.MaxValue = MaxHealth;
        HealthBar.SetValue(Health);
    }

    private void OnBodyExitedFireballHitbox(Node2D body)
    {
        _isInFireballHitbox = false;
    }

    private void OnBodyEnteredFireballHitbox(Node2D body)
    {
        _isInFireballHitbox = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        _fireballCooldoownTimeLeft = Math.Max(0, _fireballCooldoownTimeLeft - delta);

        // TODO: Replace w signal? 
        HealthBar.Value = Health;
        
        if (_isSwiping)
        {
            // TODO: do something when swiping? 
        }

        if (_isInFireballHitbox && _fireballCooldoownTimeLeft <= 0)
        {
            ShootFireball();
            _fireballCooldoownTimeLeft = _fireballCooldown;
        }
    }

    private void ShootFireball()
    {
        var projectile = GlobalVariables.GameManager.GameProjectiles.GetProjectile();
        var direction = (GlobalVariables.PlayerCharacter.GlobalPosition - this.GlobalPosition).Normalized();
        
        projectile.InitialDirection = direction;
        projectile.Speed = 100;
        projectile.Position = this.GlobalPosition;
        projectile.ProjectileAnimation = GlobalVariables.GameManager.GameProjectiles.FireballFrames;
        projectile.ProjectileCollisionAnimation = GlobalVariables.GameManager.GameProjectiles.Explosion1;
        projectile.Damage = 20;
        projectile.Weight = 20;
        projectile.Lifetime = 3f;
        
        projectile.SetCollisionMask(0);
        projectile.SetCollisionMaskValue(3, true);
        
        GlobalVariables.ActiveMainSceneContainer.AddChild(projectile);
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
        // GD.Print("ENEMY NOW IN RANGE");

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