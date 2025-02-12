using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.helpers;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class IceballSkill : PlayerSkill, IProjectileSkill
{
    public TESTCS.skills.Iceball.IceballSkillData SkillData;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {}

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override void Interrupt()
    {
        throw new System.NotImplementedException();
    }
}