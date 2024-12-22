using System;
using Godot;
using TESTCS.enums;

public partial class GhostEnemy1 : BaseEnemy, IDamageable
{
    float max_health = 100;
    float health = 100;
    private readonly TowardsTargetMover _mover;
    float speed = 50;

    GhostEnemy1() : base(EnemyType.Ghost1)
    {
        _mover = new TowardsTargetMover(GlobalVariables.Instance.Character);
    }

    public override void _Ready()
    {
        _mover.target = GlobalVariables.Instance.Character;
    }

    public override void _Process(double delta)
    {
        _mover?.Move(this, speed);
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        var healthNormalized = health / max_health;

        // GD.Print(healthNormalized);
        var progressBar = GetNode<ProgressBar>("ProgressBar");
        // GD.Print(progressBar.Value);
        progressBar.Value = healthNormalized;
        // GD.Print(progressBar.Value);
        
        if (health < 1)
        {
            GD.Print("DYING");
            Die();
            QueueFree();
        }
    }
}