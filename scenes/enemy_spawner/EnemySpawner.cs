using System;
using Godot;

public partial class EnemySpawner : Node2D
{
    [Export]
    public bool enabled = false;
    Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += SpawnEnemy;
    }

    private void SpawnEnemy()
    {
        if (enabled)
        {
            // var level = GlobalVariables.Instance.LevelManager.CurrentLevel;
            // var scene = GD.Load<PackedScene>("res://scenes/enemies/Enemy1.tscn");
            // var instance = scene.Instantiate<Enemy1>();
            // instance.Position = Position;
            // level.AddChild(instance);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
