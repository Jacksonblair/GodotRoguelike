using System;
using System.Collections.Generic;
using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.skills.Modifiers;

namespace TESTCS.skills;


/** Skill state context object. Pass around between skill related systems */
public partial class SkillHandler : GodotObject
{
    /**
     * I want to know the input used to trigger the skill. Or do i?
     * Well the slot and the input are separate. If the input to trigger the skill is pressed, i shouldnt need to know it specifically.
     */
    
    public PlayerInputs MappedInput { get; set; }
    
    public Actor Actor { get; set; }
    
    // Skill scene
    public Skill SkillNode { get; set; }
    
    // Static skill data
    public SkillData SkillData { get; set; }
    
    // Handler for cooldowns
    public SkillCooldownManager SkillCooldownManager { get; set; }
    
    // Handler for charging
    public SkillChargingManager SkillChargingManager { get; set; } = new();
    
    // Handler for input
    public SkillInputHandler SkillInputHandler { get; set; }
    
    // Modifiers
    public List<SkillModifier> SkillModifiers { get; set; }

    // Modifier handler (calculating final state)
    public ModifierHandler SkillModifierHandler { get; set; } = new();

    public SkillHandler(Actor actor, PlayerInputs mappedInput, SkillData skillData, Skill skillNode, List<SkillModifier> modifiers)
    {
        Actor = actor;
        SkillNode = skillNode;
        SkillData = skillData;
        SkillModifiers = modifiers;
        MappedInput = mappedInput;
        SkillCooldownManager = new SkillCooldownManager(this);
        SkillInputHandler = new SkillInputHandler();

        SkillInputHandler.ExecutedSkill += OnExecutedSkill;
        SkillChargingManager.SkillHandler = this;
    }

    public void Update(double delta)
    {
        // Update cooldown mgr
        
        // TODO: Take modifiers into account
        SkillCooldownManager.Update(delta);
        
        // Update input mgr
        SkillInputHandler.Update(delta, MappedInput);
        SkillChargingManager.Update(delta);
    }

    private void OnExecutedSkill()
    {
        // Recalculate skill modifiers
        SkillModifierHandler.CalculateModifierResults(SkillModifiers);
        
        if (SkillCooldownManager.TryUseCharge())
        {
            SkillNode.Execute(this.Actor, SkillModifierHandler.SkillModifierResults);
        }
    }

    // TODO: Replace with something more general
    public int GetMaxCharges()
    {
        return SkillData.Charges + SkillModifierHandler.SkillModifierResults.AdditionalCharges;
    }

    // TODO: Calculate better. 
    public int GetSkillCooldownTime()
    {
        return SkillData.CooldownTime - SkillModifierHandler.SkillModifierResults.CooldownReduction;
    }

}
