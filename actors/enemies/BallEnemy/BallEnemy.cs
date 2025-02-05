using Godot;
using System;
using TESTCS.actors.controllers;
using TESTCS.actors.enemies;

public partial class BallEnemy : EnemyActor
{
    public override void _Ready()
    {
        Controller = new BasicEnemyController();
    }
}
