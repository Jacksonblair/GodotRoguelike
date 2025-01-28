using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class RetaliateSkill : Skill, IProjectileSkill
{
    public RetaliateSkillData SkillData;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {}

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}