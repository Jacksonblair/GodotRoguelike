using Godot;

namespace TESTCS.scenes.projectiles;

public partial class BaseProjectile : Area2D
{
    /// <summary>
    /// Speed of projectile
    /// </summary>
    [Export]
    public float Speed { get; private set; } = 250;

    /// <summary>
    /// Initial Direction of projectile
    /// </summary>
    public Vector2 InitDirection { get; set; }

    /// <summary>
    /// Class for moving the projectile
    /// </summary>
    protected IProjectileMover Mover = new LinearProjectileMover();

    public override void _Process(double delta)
    {
        {
            Mover?.Move(this, delta);
        }
    }

    public void Init(IProjectileMover mover, Vector2 initDirection)
    {
        this.Mover = mover;
        this.InitDirection = initDirection;
    }
}