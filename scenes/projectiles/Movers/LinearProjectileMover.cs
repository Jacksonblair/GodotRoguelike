using Godot;

namespace TESTCS.scenes.projectiles;

public partial class LinearProjectileMover : Node2D, IProjectileMover
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void Move(TESTCS.scenes.projectiles.BaseProjectile proj, double delta)
    {
        proj.Position += proj.InitDirection * proj.Speed * (float)delta;
    }
}