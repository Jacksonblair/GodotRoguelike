using TESTCS.actors;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

namespace TESTCS.skills.Retaliate;

public partial class RetaliateSkill : Skill, IProjectileSkill
{
    public RetaliateSkillData SkillData;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {}

    public override void Execute(Actor executedBy, ModifierResults modifiers)
    {
        throw new System.NotImplementedException();
    }
}