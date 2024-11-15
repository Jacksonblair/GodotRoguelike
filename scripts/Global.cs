using Godot;

[GlobalClass]
public partial class Global : Node
{
    // Store a reference to the character
    public static CharacterBody2D Character { get; set; }
    public static StoneLevel Level { get; set; }
    public static KillTrackingManager KillTrackingManagerer { get; set; }
}
