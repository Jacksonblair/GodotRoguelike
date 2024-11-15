using System;
using Godot;

public partial class CharacterStats : Node
{
    [Signal]
    public delegate void StatsChangedEventHandler();

    [Export]
    uint strength;

    [Export]
    uint wisdom;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Check if the "A" key is pressed
        // if (Input.IsActionJustPressed("ui_left")) // "ui_left" is mapped to the arrow keys and "A" key by default
        // {
        //     GD.Print("UI LEFT DOWN");
        //     EmitSignal(SignalName.StatsChanged);
        // }
    }
}
