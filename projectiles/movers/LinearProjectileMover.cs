using Godot;
using TESTCS.projectiles;

namespace TESTCS.scenes.projectiles;

public class LinearProjectileMover : IProjectileMover
{
    public void Move(Projectile projectile, double delta)
    {
        projectile.Position += projectile.InitialDirection * projectile.Speed * (float)delta;
    }
}