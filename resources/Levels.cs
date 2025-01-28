using Godot;
using Godot.Collections;

public enum LevelsEnum
{
    StoneLevel
}

[GlobalClass]
public partial class Levels : Resource
{
    public static Dictionary<LevelsEnum, PackedScene> LevelsDict = new Dictionary<LevelsEnum, PackedScene>
    {

    };
    [Export]
    public PackedScene GameLevel { get; set; }
}