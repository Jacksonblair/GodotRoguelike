using System.Collections.Generic;
using Godot;
using TESTCS.enums;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

public partial class SkillSlotManager : Node
{
    // Skilldata for spells
    [Export] public skills.Fireball.FireballSkillData FireballSkillData { get; set; }
    [Export] public IceballSkillData IceballSkillData { get; set; }

    [Export] public int MaxSlots = 4;

    // Array of skills
    public List<SkillHandler> SkillSlots = new();

    public override void _Ready()
    {
        // Initialize slots with null (empty)
        for (var i = 0; i < MaxSlots; i++)
        {
            SkillSlots.Add(null);
        }
        
        CallDeferred(nameof(TempCreateDefaultSkills));
    }

    public override void _Process(double delta)
    {
        for (var i = 0; i < MaxSlots; i++)
        {
            SkillSlots[i]?.Update(delta);
        }
    }

    public void TempCreateDefaultSkills()
    {
        AssignSkill(PlayerInputs.Skill1, 0, FireballSkillData);
        AssignSkill(PlayerInputs.Skill2, 1, IceballSkillData);
    }

    public void AssignSkill(PlayerInputs input, int skillIndex, SkillData skillData)
    {
        if (skillIndex < 0 || skillIndex >= MaxSlots) return;

        // Unload existing skill, if there is one
        UnassignSkill(skillIndex);

        // SET UP NEW SKILL 
        var skillNode = skillData.InstantiateSkillScene();
        var skillSlotData = new SkillHandler(input, skillData, skillNode, new List<SkillModifier>());
        skillNode.SkillHandler = skillSlotData;
        
        // Add instances to scene
        AddChild(skillNode);

        // Store ref in skillSlots
        SkillSlots[skillIndex] = skillSlotData;
        GD.Print($"Assigned {skillData.SkillName} to slot {skillIndex}");
        GD.Print("EMITTING SIGNAL");
    }

    public void UnassignSkill(int slotIndex)
    {
        // Unload existing skill
        if (SkillSlots[slotIndex] != null)
        {
            SkillSlots[slotIndex].SkillNode.QueueFree();
            SkillSlots[slotIndex] = null;
        }
    }
}