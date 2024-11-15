using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Godot;

public partial class Projectile : Area2D, IDamager
{
    public float speed = 250;
    public Vector2 initDirection;
    private IProjectileMover mover;
    private Godot.Timer timer;
    private bool hasHit = false;
    private bool stopMoving = false;

    [Export]
    public int Damage = 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // GD.Print("Projectile ready");
        timer = GetNode<Godot.Timer>("Lifetime");
        timer.WaitTime = 3f;
        timer.Timeout += OnLifetimeEnd;
        timer.Start();

        // Add event listener to projectile
        BodyEntered += OnBodyEntered;

        var explosion = GetNode<Explosion1>("Explosion1");
        var animation = explosion.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animation.AnimationFinished += new Action(() =>
        {
            DespawnProjectile();
        });
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("SETTING HASHIT TO TRUE");
        stopMoving = true;
        hasHit = true;
        Explode();
    }

    public override void _Process(double delta)
    {
        if (!stopMoving)
        {
            mover?.Move(this, delta);
        }
    }

    public void Init(IProjectileMover mover, Vector2 initDirection)
    {
        this.mover = mover;
        this.initDirection = initDirection;
    }

    private void OnLifetimeEnd()
    {
        if (!hasHit)
        {
            Explode();
        }
    }

    private void Explode()
    {
        var explosion = GetNode<Explosion1>("Explosion1");
        var animation = explosion.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        explosion.Show();
        animation.Play();

        ApplyExplosionDamage();
    }

    private void DespawnProjectile()
    {
        QueueFree();
    }

    private void ApplyExplosionDamage()
    {
        var explosion = GetNode<Explosion1>("Explosion1");
        var bodies = explosion.GetOverlappingBodies();
        foreach (Node2D body in bodies)
        {
            if (body is IDamageable damageable)
            {
                damageable.ReceiveDamage(Damage);
            }
        }
    }

    public int GetDamage()
    {
        return Damage;
    }
}
