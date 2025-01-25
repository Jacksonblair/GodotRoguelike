using System.Collections.Generic;
using Godot;
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
    
    // Skill scene
    public Skill SkillNode { get; set; }
    
    // Static skill data
    public SkillData SkillData { get; set; }
    
    // Handler for cooldowns
    public SkillCooldownManager SkillCooldownManager { get; set; }
    
    // Handler for input
    public SkillInputHandler SkillInputHandler { get; set; }
    
    // Modifiers
    public List<SkillModifier> SkillModifiers { get; set; }
    
    // TODO: FIGURE THIS OUT
    public ModifierResults FinalSkillState { get; set; }
    
    // How 'Charged' the skill is
    public float ChargeAmount { get; set; }

    public SkillHandler(PlayerInputs mappedInput, SkillData skillData, Skill skillNode, List<SkillModifier> modifiers)
    {
        SkillNode = skillNode;
        SkillData = skillData;
        SkillModifiers = modifiers;
        MappedInput = mappedInput;
        FinalSkillState = new ModifierResults();
        SkillCooldownManager = new SkillCooldownManager(this);
        SkillInputHandler = new SkillInputHandler();

        SkillInputHandler.ExecutedSkill += OnExecutedSkill;
        SkillInputHandler.StartedChargingSkill += OnStartedChargingSkill;
    }

    public void Update(double delta)
    {
        // Update cooldown mgr
        // TODO: Take modifiers into account
        SkillCooldownManager.Update(delta);
        // Update input mgr
        SkillInputHandler.Update(MappedInput);
    }
    
    /**
     * Ok now. If i want to execute a skill, how does it go:
     * - InputMgr says 'hey we tried to execute'
     * - CooldownMgr says yes or no.
     * - Then we execute the skill. The node needs to know about this. 
     */
    
    private void OnStartedChargingSkill()
    {
        if (SkillCooldownManager.HasCharges())
        {
            SkillNode.Charge();
        }
    }

    private void OnExecutedSkill()
    {
        if (SkillCooldownManager.TryUseCharge())
        {
            SkillNode.Execute(new ModifierResults());
        }
    }

    // TODO: Replace with something more general
    public int GetMaxCharges()
    {
        return SkillData.Charges + FinalSkillState.AdditionalCharges;
    }

    // TODO: Calculate better. 
    public int GetSkillCooldownTime()
    {
        return SkillData.CooldownTime - FinalSkillState.CooldownReduction;
    }
}
