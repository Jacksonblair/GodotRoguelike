using System;
using Godot;

public partial class LinearProjectileMover : Node2D, IProjectileMover
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void Move(Projectile proj, double delta)
    {
        proj.Position += proj.initDirection * proj.speed * (float)delta;
    }
}
