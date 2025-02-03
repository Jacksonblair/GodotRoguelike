using Godot;

namespace TESTCS.projectiles;

[GlobalClass]
public partial class GameProjectiles : Resource
{
    [Export] public PackedScene BaseProjectileScene { get; set; }
    [Export] public ProjectileData FireballProjectileData { get; set; }
}