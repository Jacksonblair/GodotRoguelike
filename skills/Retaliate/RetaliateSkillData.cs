using Godot;

[GlobalClass]
public partial class RetaliateSkillData : SkillData
{
    public override Skill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<RetaliateSkill>();
        scene.SkillData = this;
        return scene;
    }
}