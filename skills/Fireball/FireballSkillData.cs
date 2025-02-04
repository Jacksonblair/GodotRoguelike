using Godot;

namespace TESTCS.skills.Fireball;

[GlobalClass]
public partial class FireballSkillData : SkillData
{
    public int BaseDamage { get; set; } = 20;
    public int BaseProjectiles { get; set; } = 1;
    public int BaseWeight { get; set; } = 30;
    public int ProjectileSpeed { get; set; } = 300;
    
    public override Skill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<FireballSkill>();
        scene.SkillData = this;
        return scene;
    }
}