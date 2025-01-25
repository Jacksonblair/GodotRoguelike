using Godot;
using TESTCS.skills.Modifiers;

[GlobalClass]
public partial class SkillData : Resource
{
    [Export] public PackedScene SkillScene { get; set; }
    
    // Common base skill properties
    [Export] public string SkillName { get; set; }
    [Export] public int CooldownTime { get; set; } = 1;
    [Export] public int Charges { get; set; } = 1;
    
    public virtual Skill InstantiateSkillScene()
    {
        // Default implementation, can be empty or do nothing
        return null;
    }
}