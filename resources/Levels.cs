using Godot;
using Godot.Collections;

public enum LevelsEnum
{
    StoneLevel
}

[GlobalClass]
public partial class Levels : Resource
{
    [Export] public PackedScene StoneLevel { get; set; }
    [Export] public PackedScene OtherLevel { get; set; }
}