using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SkillData : Resource
{
    [Export] public PackedScene SkillScene { get; set; }
    
    // Common base skill properties
    [Export] public string SkillName { get; set; }
    [Export] public int CooldownTime { get; set; } = 1;
    [Export] public int Charges { get; set; } = 1;
    [Export] public Array<float> ChargingStages { get; set; } = new Array<float>();
    
    public virtual Skill InstantiateSkillScene()
    {
        // Default implementation, can be empty or do nothing
        return null;
    }
}