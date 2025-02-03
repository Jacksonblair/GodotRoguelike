using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.helpers;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class IceballSkill : Skill, IProjectileSkill
{
    public IceballSkillData SkillData;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {}

    public override void Execute(Actor executedBy, ModifierResults modifiers) {}
}