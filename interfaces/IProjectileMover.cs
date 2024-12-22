using Godot;

public interface IProjectileMover
{
    void Move(TESTCS.scenes.projectiles.BaseProjectile projectile, double delta);
}
