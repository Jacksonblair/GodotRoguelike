using Godot;

[GlobalClass]
public partial class SlashSkillData : SkillData
{
    public override PlayerSkill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<SlashSkill>();
        scene.SlashSkillData = this;
        scene.SkillData = this;
        return scene;
    }
}