using System;
using Godot;

public partial class SpiralProjectileMover : Node2D, IProjectileMover
{
    private bool HasSetCenterPosition = false;
    private Vector2 CenterPosition; // The node to move around

    [Export]
    public float CircleSpeed = 20.0f; // Speed of circular movement (in radians per second)

    [Export]
    public float Radius = 30.0f; // Radius of the circular path in pixels

    private float _angle = 0.0f; // Current angle for the circular movement

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void Move(Projectile proj, double delta)
    {
        if (!HasSetCenterPosition)
        {
            CenterPosition = proj.Position;
            HasSetCenterPosition = true;
        }
        MoveInSpiral(proj, (float)delta);
        GD.Print(CenterPosition);
    }

    private void MoveInSpiral(Projectile proj, float delta)
    {
        // Update the angle for circular movement
        _angle += CircleSpeed * delta;

        // // Calculate the circular offset using polar coordinates
        float xOffset = Mathf.Cos(_angle) * Radius;
        float yOffset = Mathf.Sin(_angle) * Radius;

        proj.Position = CenterPosition + new Vector2(xOffset, yOffset);
        CenterPosition += proj.initDirection * proj.speed * (float)delta;
    }
}
