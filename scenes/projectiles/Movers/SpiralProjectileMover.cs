using Godot;

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

    public void Move(TESTCS.scenes.projectiles.BaseProjectile proj, double delta)
    {
        if (!_hasSetCenterPosition)
        {
            _centerPosition = proj.Position;
            _hasSetCenterPosition = true;
        }
        MoveInSpiral(proj, (float)delta);
        GD.Print(_centerPosition);
    }

    private void MoveInSpiral(BaseProjectile proj, float delta)
    {
        // Update the angle for circular movement
        _angle += CircleSpeed * delta;

        // // Calculate the circular offset using polar coordinates
        var xOffset = Mathf.Cos(_angle) * Radius;
        var yOffset = Mathf.Sin(_angle) * Radius;

        proj.Position = _centerPosition + new Vector2(xOffset, yOffset);
        _centerPosition += proj.InitDirection * proj.Speed * (float)delta;
    }
}