using Godot;
using System;
using TESTCS.skills.Interfaces;

public partial class IceballSkill : Skill, IProjectileSkill
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}