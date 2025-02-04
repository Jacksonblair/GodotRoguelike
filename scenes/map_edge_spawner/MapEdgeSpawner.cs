using System;
using Godot;

public partial class MapEdgeSpawner : Node
{
    private Viewport _viewport;
    private Random _random = new Random();
    private Timer _timer;

    public override void _Ready()
    {
        // Assumes the spawner is in the same scene as the camera
        _viewport = GetViewport();
        _timer = GetNode<Timer>("Timer");
        _timer.WaitTime = 1.0f;
        _timer.Timeout += SpawnEnemy;
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 cameraPosition = GetViewport().GetCamera2D().GlobalPosition;
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

        float spawnDistance = 50f; // Distance outside the viewport bounds
        float x, y;

        // Randomly pick a side of the screen to spawn the enemy
        switch (_random.Next(4))
        {
            case 0: // Top
                x =
                    cameraPosition.X
                    + (float)_random.NextDouble() * viewportSize.X
                    - viewportSize.X / 2;
                y = cameraPosition.Y - viewportSize.Y / 2 - spawnDistance;
                break;
            case 1: // Bottom
                x =
                    cameraPosition.X
                    + (float)_random.NextDouble() * viewportSize.X
                    - viewportSize.X / 2;
                y = cameraPosition.Y + viewportSize.Y / 2 + spawnDistance;
                break;
            case 2: // Left
                x = cameraPosition.X - viewportSize.X / 2 - spawnDistance;
                y =
                    cameraPosition.Y
                    + (float)_random.NextDouble() * viewportSize.Y
                    - viewportSize.Y / 2;
                break;
            case 3: // Right
                x = cameraPosition.X + viewportSize.X / 2 + spawnDistance;
                y =
                    cameraPosition.Y
                    + (float)_random.NextDouble() * viewportSize.Y
                    - viewportSize.Y / 2;
                break;
            default:
                x = cameraPosition.X;
                y = cameraPosition.Y;
                break;
        }

        return new Vector2(x, y);
    }

    public void SpawnEnemy()
    {
        var position = GetSpawnPosition();
        var enemy = GlobalVariables.GameManager.Enemies.GhostEnemy.Instantiate<Node2D>();
        enemy.Position = position;
        GlobalVariables.ActiveMainSceneContainer.AddChild(enemy);
        // var level = GlobalVariables.Instance._activeMainSceneContainer;
        // var scene = GD.Load<PackedScene>("res://actors/enemies/GhostEnemy1.tscn");
        // var instance = scene.Instantiate<GhostEnemy1>();
        // instance.Position = position;
        // level.AddChild(instance);
    }
}
