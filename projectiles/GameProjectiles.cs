using Godot;

namespace TESTCS.projectiles;

[GlobalClass]
public partial class GameProjectiles : Resource
{
    [Export] public PackedScene BaseProjectileScene { get; set; }
    [Export] public ProjectileData FireballProjectileData { get; set; }
    [Export] public SpriteFrames FireballFrames { get; set; }
    [Export] public SpriteFrames Explosion1 { get; set; }

    public Projectile GetProjectile()
    {
        return BaseProjectileScene.Instantiate<Projectile>();
    }
}