using System;
using Godot;

public partial class StoneLevel : TileMapLayer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("STONE LEVEL LOADED");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
