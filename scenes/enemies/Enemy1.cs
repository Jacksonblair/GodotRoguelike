using System;
using Godot;
using TESTCS.enums;

public partial class Enemy1 : BaseEnemy, IDamageable
{
    float max_health = 100;
    float health = 100;
    IEnemyMover mover = new EnemyMoverFollow();
    private CharacterBody2D _player;
    float speed = 50;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Init(EnemyType.Ghost1);
        _player = GetNode<CharacterBody2D>("/root/Main/StoneLevel/PlayerCharacter");
    }

    public override void _Process(double delta)
    {
        mover?.Move(this, _player, speed);
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
