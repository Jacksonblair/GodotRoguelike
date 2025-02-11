using System.Collections.Generic;
using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

/** Manages which skills are equipped */
public partial class PlayerSkillSlotManager : Node
{
    // Skilldata for spells
    [Export] public Skills SkillsData { get; set; }
    [Export] public int MaxSlots = 4;
    
    [Signal] public delegate void EquippedSkillEventHandler(int abilityIndex);
    [Signal] public delegate void UnequippedSkillEventHandler(int abilityIndex);

    // Array of skills
    public List<PlayerSkill> SkillSlots = new();

    public override void _Ready()
    {
        // Initialize slots with null (empty)
        for (var i = 0; i < MaxSlots; i++)
        {
            SkillSlots.Add(null);
        }
        
        GD.Print("PlayerSkillSlotManager ready");
        
        CallDeferred(nameof(TempCreateDefaultSkills));
    }
    
    public void TempCreateDefaultSkills()
    {
        AssignSkill(PlayerInputs.Skill1, 0, SkillsData.SlashSkillData);
        AssignSkill(PlayerInputs.Skill2, 1, SkillsData.FireballSkillData);
        // AssignSkill(PlayerInputs.Skill3, 2, SkillsData.IceballSkillData);
        // SkillSlots[0].SkillModifiers.Add(new ExtraProjectileModifier(5));
    }

    public void AssignSkill(PlayerInputs input, int slotIndex, SkillData skillData)
    {
        if (slotIndex < 0 || slotIndex >= MaxSlots) return;

        // Unload existing skill, if there is one
        UnassignSkill(slotIndex);  

        // SET UP NEW SKILL 
        var skillNode = skillData.InstantiateSkillScene();
        skillNode.MappedInput = input;
        skillNode.Owner = GV.PlayerCharacter;

        // Add instances to scene
        GV.PlayerCharacter.AddChild(skillNode);
        // AddChild(skillNode);

        // Store ref in skillSlots
        SkillSlots[slotIndex] = skillNode;
        GD.Print($"Assigned {skillData.SkillName} to slot {slotIndex}");
        GD.Print("EMITTING SIGNAL");
        EmitSignal(nameof(EquippedSkill), slotIndex);
    }

    public void UnassignSkill(int slotIndex)
    {
        // Unload existing skill
        if (SkillSlots[slotIndex] != null)
        {
            SkillSlots[slotIndex].QueueFree();
            SkillSlots[slotIndex] = null;
            EmitSignal(nameof(UnequippedSkill), slotIndex);
        }
    }
}