using Godot;
using System;
using TESTCS.actors.controllers;
using TESTCS.actors.enemies;

public partial class Level1Enemy : EnemyActor
{
    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }

    private Area2D _meleeHitbox;
    private Area2D _meleeAttackIndicator;
    
    public override void _Ready()
    {
        MovementSpeed = 20;
        Controller = new BasicEnemyController();
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _meleeHitbox = GetNode<Area2D>("MeleeHitbox");
        _meleeHitbox.BodyEntered += OnBodyEnteredMeleeHitbox;
        _meleeAttackIndicator = GetNode<Area2D>("AttackIndicator");
    }

    private void OnBodyEnteredMeleeHitbox(Node2D body)
    {
        // Rotate towards player
        Vector2 direction = (body.GlobalPosition - GlobalPosition).Normalized();
        _meleeAttackIndicator.Rotation = direction.Angle();
        _meleeAttackIndicator.SetVisible(true);
        
        var indicatorTimer = new Timer();
        indicatorTimer.WaitTime = 0.5f;
        indicatorTimer.OneShot = true;
        indicatorTimer.Timeout += () =>
        {
            var bodies = _meleeAttackIndicator.GetOverlappingBodies();
            foreach (var node2D in bodies)
            {
                if (node2D is IHittable hittable)
                {
                    hittable.ReceiveHit(new HitInformation(25, 50, Position));
                }
            }
            Controller.EnableMovementInput();
            _meleeAttackIndicator.SetVisible(false);
        };
        
        AddChild(indicatorTimer);
        indicatorTimer.Start();
        
        /**
         * TO START WITH:
         * - Show hurtbox
         * - Apply hit
         * - Make blocking work
         */
        
    }
}