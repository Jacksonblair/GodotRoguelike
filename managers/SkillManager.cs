using System.Collections.Generic;
using Godot;
using Godot.Collections;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

public enum SkillsEnum
{
    BasicProjectileSkillyPoos,
}

public enum SkillSlotsEnum
{
    Skill1,
    Skill2,
    Skill3
}

/**
 * Each skill has a SkilLData resource, which references a PackedScene
 * The scenes implement the actual behaviour of the skill.
 */

public partial class SkillManager : Node
{
    [Export] public skills.Fireball.FireballSkillData FireballSkillData { get; set; }
    [Export] public IceballSkillData IceballSkillData { get; set; }
    [Export] public int MaxSlots = 4;
    private List<Node> skillSlots = new();
    
    // Modifiers that apply to all skills
    public List<SkillModifier> GlobalModifiers { get; set; } = new();
    
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
            skillSlots[slotIndex].QueueFree();
            skillSlots[slotIndex] = null;
        }
        
        // Load and assign new skill
        Node skillInstance = skillData.InstantiateSkillScene();
        
        // Update scene variables based on skill data resource
        AddChild(skillInstance);
        skillSlots[slotIndex] = skillInstance;
        GD.Print($"Assigned {skillData.SkillName} to slot {slotIndex}");
        GD.Print(skillInstance.GetType());
    }

    public void ActivateSkill(int slotIndex)
    {
        if (skillSlots[slotIndex] is Skill skill)
        {
            skill.Execute();  // Calls the Execute method from the ISkill interface
        }
        else
        {
            GD.Print("Skill does not implement: ", typeof(ISkill));
        }
    }
}