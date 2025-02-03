using System.Collections.Generic;
using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

/**
 * SkillSlotManager right now only handles a single player.
 * - I dont think it should be the child of the actor.
 * - What if i have an ally, who also uses skills (with cooldowns, etc)
 * 
 * What about enemies?
 * - My ghost enemy should use a Swipe/Fireball skill from time to time.
 *
 * Can i just associate a skillslotmanager with a particular actor?
 */

/** Manages which skills are equipped, and calls .Update on them */
public partial class PlayerSkillSlotManager : Node
{
    // Skilldata for spells
    [Export] public skills.Fireball.FireballSkillData FireballSkillData { get; set; }
    [Export] public IceballSkillData IceballSkillData { get; set; }

    [Export] public int MaxSlots = 4;
    
    [Signal] public delegate void EquippedSkillEventHandler(int abilityIndex);
    [Signal] public delegate void UnequippedSkillEventHandler(int abilityIndex);

    // Array of skills
    public List<SkillHandler> SkillSlots = new();

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
        SkillSlots[0].SkillModifiers.Add(new ExtraProjectileModifier(5));
    }

    public void AssignSkill(PlayerInputs input, int slotIndex, SkillData skillData)
    {
        if (slotIndex < 0 || slotIndex >= MaxSlots) return;

        // Unload existing skill, if there is one
        UnassignSkill(slotIndex);  

        // SET UP NEW SKILL 
        var skillNode = skillData.InstantiateSkillScene();
        var skillSlotData = new SkillHandler(GlobalVariables.PlayerCharacter, input, skillData, skillNode, new List<SkillModifier>());
        skillNode.SkillHandler = skillSlotData;
        
        // Add instances to scene
        AddChild(skillNode);

        // Store ref in skillSlots
        SkillSlots[slotIndex] = skillSlotData;
        GD.Print($"Assigned {skillData.SkillName} to slot {slotIndex}");
        GD.Print("EMITTING SIGNAL");
        EmitSignal(nameof(EquippedSkill), slotIndex);
    }

    public void UnassignSkill(int slotIndex)
    {
        // Unload existing skill
        if (SkillSlots[slotIndex] != null)
        {
            SkillSlots[slotIndex].SkillNode.QueueFree();
            SkillSlots[slotIndex] = null;
            EmitSignal(nameof(UnequippedSkill), slotIndex);
        }
    }
}