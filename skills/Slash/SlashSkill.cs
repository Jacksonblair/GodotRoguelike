using TESTCS.actors;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class SlashSkill : Skill, IProjectileSkill
{
    public SlashSkillData SkillData;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }

    public override void Execute(Actor executedBy, ModifierResults modifiers)
    {
        throw new System.NotImplementedException();
    }
}