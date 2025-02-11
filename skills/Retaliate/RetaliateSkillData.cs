using Godot;

[GlobalClass]
public partial class RetaliateSkillData : SkillData
{
    public override PlayerSkill InstantiateSkillScene()
    {
        var scene = SkillScene.Instantiate<TESTCS.skills.Retaliate.RetaliateSkill>();
        scene.SkillData = this;
        return scene;
    }
}