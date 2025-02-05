using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Levels : Resource
{
    [Export] public PackedScene StoneLevel { get; set; }
    [Export] public PackedScene SecondFloorLevel { get; set; }
    [Export] public PackedScene OtherLevel { get; set; }
    [Export] public PackedScene Transition1 { get; set; }
}