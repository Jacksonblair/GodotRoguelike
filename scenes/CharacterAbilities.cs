using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

public enum Abilities
{
    Fireball
}

public partial class CharacterAbilities : Node
{
    [Export]
    Godot.Collections.Array<Abilities> abilities = new Godot.Collections.Array<Abilities> { };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var stats = GetParent().GetNode<CharacterStats>("CharacterStats");
        GD.Print(stats);
        stats.StatsChanged += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        GD.Print("STATS CHANGED");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
