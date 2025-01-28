using Godot;

[GlobalClass]
public partial class SlashSkillData : SkillData
{
    public override Skill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<SlashSkill>();
        scene.SkillData = this;
        return scene;
    }
}