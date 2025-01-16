using Godot;

[GlobalClass]
public partial class IceballSkillData : SkillData
{
    public override Node InstantiateSkillScene()
    {
        var iceball = SkillScene.Instantiate<IceballSkill>();
        iceball.Damage = Damage;
        return iceball;
    }
}