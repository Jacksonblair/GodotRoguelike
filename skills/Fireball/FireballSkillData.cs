using Godot;

namespace TESTCS.skills.Fireball;

[GlobalClass]
public partial class FireballSkillData : SkillData
{
    public override Node InstantiateSkillScene()
    {
        var fireball = SkillScene.Instantiate<FireballSkill>();
        fireball.Damage = Damage;
        return fireball;
    }
}