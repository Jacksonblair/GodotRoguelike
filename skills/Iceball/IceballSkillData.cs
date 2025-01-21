using Godot;

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