using Godot;

namespace TESTCS.skills.Iceball;

[GlobalClass]
public partial class IceballSkillData : SkillData
{
    public override Skill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<IceballSkill>();
        scene.SkillData = this;
        return scene;
    }
}