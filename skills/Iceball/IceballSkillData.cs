using Godot;

[GlobalClass]
public partial class IceballSkillData : SkillData
{
    public override Skill InstantiateSkillScene()
    {
        var iceball = SkillScene.Instantiate<IceballSkill>();
        iceball.BaseDamage = Damage;
        return iceball;
    }
}