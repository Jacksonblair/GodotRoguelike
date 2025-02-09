using Godot;
using TESTCS.skills.Fireball;

namespace TESTCS.skills;

[GlobalClass]
public partial class Skills : Resource
{
    [Export] public FireballSkillData FireballSkillData { get; set; }
    [Export] public Iceball.IceballSkillData IceballSkillData { get; set; }
    [Export] public SlashSkillData SlashSkillData { get; set; }
}