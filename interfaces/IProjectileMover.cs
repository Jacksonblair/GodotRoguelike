using Godot;
using TESTCS.projectiles;

public interface IProjectileMover
{
    void Move(Projectile projectile, double delta);
}
