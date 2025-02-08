using Godot;
using System;
using TESTCS.actors.controllers;
using TESTCS.actors.enemies;

public partial class Level1Enemy : EnemyActor
{
    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }
    
    public override void _Ready()
    {
        Controller = new PassiveEnemyController();
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}