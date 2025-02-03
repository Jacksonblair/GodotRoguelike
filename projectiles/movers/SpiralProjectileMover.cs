using Godot;
using TESTCS.projectiles;

namespace TESTCS.scenes.projectiles;

public partial class SpiralProjectileMover : Node2D, IProjectileMover
{
    private bool _hasSetCenterPosition;
    private Vector2 _centerPosition; // The node to move around

    [Export]
    public float CircleSpeed = 20.0f; // Speed of circular movement (in radians per second)

    [Export]
    public float Radius = 30.0f; // Radius of the circular path in pixels

    private float _angle = 0.0f; // Current angle for the circular movement

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void MoveInSpiral(Projectile proj, float delta)
    {
        // Update the angle for circular movement
        _angle += CircleSpeed * delta;

        // // Calculate the circular offset using polar coordinates
        var xOffset = Mathf.Cos(_angle) * Radius;
        var yOffset = Mathf.Sin(_angle) * Radius;

        proj.Position = _centerPosition + new Vector2(xOffset, yOffset);
        _centerPosition += proj.InitialDirection * proj.Speed * (float)delta;
    }

    public void Move(Projectile projectile, double delta)
    {
        if (!_hasSetCenterPosition)
        {
            _centerPosition = projectile.Position;
            _hasSetCenterPosition = true;
        }
        MoveInSpiral(projectile, (float)delta);
        GD.Print(_centerPosition);
    }
}