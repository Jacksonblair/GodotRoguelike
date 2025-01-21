using System.Collections.Generic;
using Godot;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

namespace TESTCS.managers;

/**
 * TODOS:
 * - Imagine this is on a server, and handles multiple players
 *
 * 
 * Skill modifiers need to be calculated before any UI/usage occurs.
 *  - Add local modifier? Recalculate skill
 *  - Add global modifier? Recalculate all skills
 *
 * Some skills may have modifiers saved somewhere, and need to be created with modifiers already attached
 *
 * Maybe they should just be calculated each time they're used, to save worrying about sync issues.
 * But they should also be calculated each time a modifier is changed, because UI needs to be up to date.
 *
 * Skill -> Modifiers -> Things accessing skill
 *
 * Modifiers are not tied to a skill.. rather, they are applied to a skill by:
 *  - Talent points
 *  - Items
 *  - Buffs
 *
 * So they're all external more or less, but their result needs to be available in the Skill scene, and to the UI 
 */

// Create one of these class instances for each equipped skill
public class SkillSlotState
{
    public Skill SkillNode { get; set; }
    public SkillData SkillData { get; set; }
    
    public List<SkillModifier> SkillModifiers { get; set; }
    public ModifierResults FinalSkillState { get; set; }

    public float CurrentCooldown { get; set; }
    public int CurrentCharges { get; set; }
    

    public SkillSlotState(SkillData skillData, Skill skillNode, List<SkillModifier> modifiers)
    {
        SkillNode = skillNode;
        SkillData = skillData;
        SkillModifiers = modifiers;
        FinalSkillState = new ModifierResults();
        
        // Make sure charges are set to maximum by default
        CurrentCharges = GetMaxCharges();
    }

    // TODO: Replace with something more general
    public int GetMaxCharges()
    {
        return SkillData.Charges + FinalSkillState.AdditionalCharges;
    }

    // TODO: Calculate better.
    public int GetSkillCooldownTime()
    {
        return SkillData.Cooldown - FinalSkillState.CooldownReduction;
    }

    // public void AddModifiers(string skillName, List<SkillModifier> modifiers)
    // {
    //     SkillModifiers.AddRange(modifiers);
    // }
    
}

/**
 * SkillMgr
 *    PackedScenes for each skill
 *    Slots
 *
 * Equip a skill
 *  - Start managing
 *      - Emitting signals (in mgr)
 *      - Cooldowns/Charges.cs
 *      - Modifiers.cs
 *
 * Only when a skill can be executed, is when the method is called and the modifiers are passed to it.
 *
 * SkillData is a static bit of data representing the base properties of a skill.
 */
public partial class SkillManager : Node
{
    [Export] public int MaxSlots = 4;
    public SkillCooldownManager SkillCooldownManager;

    // Skilldata for spells
    [Export] public skills.Fireball.FireballSkillData FireballSkillData { get; set; }
    [Export] public IceballSkillData IceballSkillData { get; set; }

    // Array of skills
    public List<SkillSlotState> SkillSlots = new();

    // TEMP: Modifiers that apply to all skills (items, passives, etc) 
    public List<SkillModifier> GlobalModifiers { get; set; } = new();
    

    public ModifierHandler ModifierHandler = new();

    // -- Events --
    [Signal]
    public delegate void SkillUnequippedEventHandler(int skillIndex);

    [Signal]
    public delegate void SkillEquippedEventHandler(SkillData skillData, int skillIndex);

    [Signal]
    public delegate void StartedCooldownEventHandler(float timeLeft, int skillIndex);

    [Signal]
    public delegate void NumberChargesChangedEventHandler(int numCharges, int skillIndex);

    // public void CalculateSkillModifiers(SkillSlotState skillSlotState)
    // {
    //     var results = new ModifierResults();
    //     ModifierHandler.CalculateModifierResults(results, skillSlotState.SkillModifiers);
    //     ModifierHandler.CalculateModifierResults(results, GlobalVariables.Instance.SkillManager.GlobalModifiers);
    //     skillSlotState.ModifierResults = results;
    // }

    // public void AddGlobalModifier(SkillModifier modifier)
    // {
    //     GlobalModifiers.Add(modifier);
    //     GD.Print("Applied global modifier.");
    // }
    //
    // public void RemoveGlobalModifier(SkillModifier modifier)
    // {
    //     // TODO: Remove by ID instead? Dont think this one will work properly
    //     GlobalModifiers.Remove(modifier);
    //     GD.Print("Removed global modifier.");
    // }

    public void AddSkillModifier( SkillModifier modifier)
    {
        
    }
    
    public override void _Ready()
    {
        SkillCooldownManager = new SkillCooldownManager(SkillSlots,
            (timeleft, index) => { EmitSignal(nameof(StartedCooldown), timeleft, index); },
            (numcharges, index) => { EmitSignal(nameof(NumberChargesChanged), numcharges, index); });

        GD.Print("SKILL CD MANAGER G2G", SkillCooldownManager);
        
        // Initialize slots with null (empty)
        for (int i = 0; i < MaxSlots; i++)
        {
            SkillSlots.Add(null);
        }
        
        CallDeferred(nameof(TempCreateDefaultSkills));
    }

    public override void _Process(double delta)
    {
        SkillCooldownManager.Update(delta);
    }

    public void TempCreateDefaultSkills()
    {
        // DEFAULT: ADD SOME SKILLS TO SLOTS
        AssignSkill(0, FireballSkillData);
        SkillSlots[0].SkillModifiers.Add(new ExtraProjectileModifier(1));
        SkillSlots[0].SkillModifiers.Add(new ExtraProjectileModifier(2));
        SkillSlots[0].SkillModifiers.Add(new FlatDamageModifier(20));
        AssignSkill(1, IceballSkillData);
    }

    public void AssignSkill(int skillIndex, SkillData skillData)
    {
        if (skillIndex < 0 || skillIndex >= MaxSlots) return;

        // Unload existing skill, if there is one
        UnassignSkill(skillIndex);

        // Load, Init and assign new skill
        var skillNode = skillData.InstantiateSkillScene();
        var skillSlotData = new SkillSlotState(skillData, skillNode, new List<SkillModifier>());
        
        // Add instances to scene
        AddChild(skillNode);

        // Store ref in skillSlots
        SkillSlots[skillIndex] = skillSlotData;
        GD.Print($"Assigned {skillData.SkillName} to slot {skillIndex}");
        GD.Print("EMITTING SIGNAL");
        EmitSignal(nameof(SkillEquipped), skillData, skillIndex);
    }

    public void UnassignSkill(int slotIndex)
    {
        // Unload existing skill
        if (SkillSlots[slotIndex] != null)
        {
            // Make sure to free the related scenes.
            SkillSlots[slotIndex].SkillNode.QueueFree();
            // skillSlots[slotIndex].CooldownManagerRef.QueueFree();
            SkillSlots[slotIndex] = null;
            EmitSignal(nameof(SkillUnequipped), slotIndex);
        }
    }

    private void OnSkillStartedCooldown(float timeleft, int slotIndex)
    {
        EmitSignal(nameof(NumberChargesChanged), timeleft, slotIndex);
    }

    private void OnNumberSkillChargesChanged(int numCharges, int slotIndex)
    {
        EmitSignal(nameof(StartedCooldown), numCharges, slotIndex);
    }

    public void ActivateSkill(int slotIndex)
    {
        // Calculate modifiers.
        // Take abilities into account
        // Take stats into account
        // 
        
        if (SkillSlots[slotIndex].SkillNode is Skill skill)
        {
            if (SkillCooldownManager.TryUseCharge(slotIndex))
            {
                skill.Execute(SkillSlots[slotIndex].FinalSkillState);
            }
        }
        else
        {
            GD.Print("Skill does not implement: ", typeof(ISkill));
        }
    }
}