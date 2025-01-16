using Godot;
using TESTCS.skills.Modifiers;

[GlobalClass]
public partial class SkillData : Resource
{
    [Export] public string SkillName { get; set; }
    [Export] public PackedScene SkillScene { get; set; }
    [Export] public int Damage { get; set; } = 10;
    [Export] public int Cooldown { get; set; } = 10;
    
    public virtual Node InstantiateSkillScene()
    {
        // Default implementation, can be empty or do nothing
        return null;
    }
}