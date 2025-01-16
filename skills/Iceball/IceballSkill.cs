using Godot;
using System;
using TESTCS.skills.Interfaces;

public partial class IceballSkill : Skill, IProjectileSkill
{
    public int Damage { get; set; }

    public override void ApplyModifiers()
    {
    }

    public override void Execute()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // public override void Execute()
    // {
    // }
}