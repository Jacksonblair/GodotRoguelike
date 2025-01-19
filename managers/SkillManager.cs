using System.Collections.Generic;
using Godot;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

class SkillSlotData
{
    public Node skillNode { get; set; }
    public SkillData skillData { get; set; }
}

/**
 * When skillmanager equips a skill, it needs to add listeners to it. 
 * 
 */

public partial class SkillManager : Node
{
    [Export] public int MaxSlots = 4;
    public SkillCooldownManager skillCooldownManager;
    
    // Skilldata for spells
    [Export] public skills.Fireball.FireballSkillData FireballSkillData { get; set; }
    [Export] public IceballSkillData IceballSkillData { get; set; }
    
    // Array of skills
    private List<SkillSlotData> skillSlots = new();
    
    // Modifiers that apply to all skills
    public List<SkillModifier> GlobalModifiers { get; set; } = new();
    
    
    // -- Events --
    [Signal] public delegate void SkillEquippedEventHandler(SkillData skillData, Skill skillNode, int skillIndex);
    [Signal] public delegate void SkillUnequippedEventHandler(SkillData skillData, int skillIndex);

    
    public void AddGlobalModifier(SkillModifier modifier)
    {
        GlobalModifiers.Add(modifier);
        GD.Print("Applied global modifier.");
    }

    public void RemoveGlobalModifier(SkillModifier modifier)
    {
        // TODO: Remove by ID instead? Dont think this one will work properly
        GlobalModifiers.Remove(modifier);
        GD.Print("Removed global modifier.");
    }
    
    public override void _Ready()
    {
        // Initialize slots with null (empty)
        for (int i = 0; i < MaxSlots; i++)
        {
            skillSlots.Add(null);
        }
        
        // DEFAULT: ADD SOME SKILLS TO SLOTS
        AssignSkill(0, FireballSkillData);
        AssignSkill(1, IceballSkillData);
    }

    public void AssignSkill(int slotIndex, SkillData skillData)
    {
        if (slotIndex < 0 || slotIndex >= MaxSlots) return;

        // Unload existing skill
        if (skillSlots[slotIndex] != null)
        {
            EmitSignal(nameof(SkillUnequipped), skillSlots[slotIndex].skillData, slotIndex);
            skillSlots[slotIndex].skillNode.QueueFree();
            skillSlots[slotIndex] = null;
        }
        
        // Load, Init and assign new skill
        var skillSlotData = new SkillSlotData();
        var skillInstance = skillData.InstantiateSkillScene();
        skillSlotData.skillNode = skillInstance;
        skillSlotData.skillData = skillData;
        
        EmitSignal(nameof(SkillEquipped), skillData, skillInstance, slotIndex);
        
        // Add instance to scene, and add ref to skillSlots
        AddChild(skillInstance);
        skillSlots[slotIndex] = skillSlotData;
        GD.Print($"Assigned {skillData.SkillName} to slot {slotIndex}");
        GD.Print(skillInstance.GetType());
    }

    public void ActivateSkill(int slotIndex)
    {
        if (skillSlots[slotIndex].skillNode is Skill skill)
        {
            skill.ExecuteSkill();  // Calls the Execute method from the ISkill interface
        }
        else
        {
            GD.Print("Skill does not implement: ", typeof(ISkill));
        }
    }
}