using System.Collections.Generic;
using Godot;
using TESTCS.enums;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

public interface ISkillOwner
{
    public Vector2 GetPosition();
    public Vector2 GetAimDirection();
}

/**
 * Can an enemy use this too? 
 */
public abstract partial class PlayerSkill : Node
{
    public PlayerInputs MappedInput;
    public SkillData SkillData;
    public SkillCooldownManager SkillCooldownManager;
    public SkillChargingManager SkillChargingManager;
    public SkillInputHandler SkillInputHandler;
    public ModifierHandler SkillModifierHandler = new();
    public List<SkillModifier> SkillModifiers = new();

    public override void _Ready()
    {
        SkillCooldownManager = new SkillCooldownManager(this);
        SkillChargingManager = new SkillChargingManager(this);
        SkillInputHandler = new SkillInputHandler(this);
        SkillInputHandler.TryExecuteSkill += OnTryExecuteSkill;
        // TODO: Calculate modifiers 
    }

    private void OnTryExecuteSkill()
    {
        if (SkillCooldownManager.TryUseCharge())
        {
            Execute();            
        }
    }

    public override void _Process(double delta)
    {
        SkillCooldownManager.Update(delta);
        SkillInputHandler.Update(delta, MappedInput);
        SkillChargingManager.Update(delta);
    }

    public abstract void Execute();

    public int GetMaxCharges()
    {
        return SkillData.Charges + SkillModifierHandler.SkillModifierResults.AdditionalCharges;
    }

    public int GetCooldownTime()
    {
        // TODO: Calculate properly
        return SkillData.CooldownTime;
    }

    public bool CanUseSkill()
    {
        return SkillCooldownManager.HasCharges();
    }
}