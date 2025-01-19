using Godot;

namespace TESTCS.skills.Fireball;

[GlobalClass]
public partial class FireballSkillData : SkillData
{
    public override Skill InstantiateSkillScene()
    {
        var fireball = SkillScene.Instantiate<FireballSkill>();
        fireball.BaseDamage = Damage;
        return fireball;
    }
}