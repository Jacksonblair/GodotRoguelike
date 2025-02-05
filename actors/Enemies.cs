using Godot;

namespace TESTCS.enemies;

[GlobalClass]
public partial class Enemies : Resource
{
    [Export] public PackedScene GhostEnemy { get; set; }
    [Export] public PackedScene BallEnemy { get; set; }
}