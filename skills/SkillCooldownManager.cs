using System;
using System.Collections.Generic;
using Godot;
using TESTCS.managers;

// TODO: Derive SkillCooldownManager from a base CooldownManager class
namespace TESTCS.skills;

/** Handles cooldowns for a single skill */
// TODO: Handle an object that implements an IHasCooldowns interface (or something), instead of a skill
public partial class SkillCooldownManager : GodotObject
{
    private PlayerSkill _playerSkill;
    public float CooldownTimeRemaining;
    public int CurrentCharges;

    [Signal]
    public delegate void GainedChargeEventHandler(int numCharges);
    [Signal]
    public delegate void StartedCooldownEventHandler(int numCharges);

    public SkillCooldownManager(PlayerSkill playerSkill)
    {
        this._playerSkill = playerSkill;
    }

    public void Update(double delta)
    {
        // If we still have less charges than skill max charges
        if (CurrentCharges < _playerSkill.GetMaxCharges())
        {
            // Reduce the cooldown
            CooldownTimeRemaining -= (float)delta;
            
            // If the cooldown is finished, restore a charge.
            // ...And then restart the cooldown for the next charge. 
            if (CooldownTimeRemaining <= 0f)
            {
                CurrentCharges++;

                EmitSignal(nameof(GainedCharge), CurrentCharges);
                GD.Print("Restoring charges: ", CurrentCharges, "/", _playerSkill.GetMaxCharges());

                if (CurrentCharges < _playerSkill.GetMaxCharges())
                {
                    CooldownTimeRemaining = _playerSkill.GetCooldownTime();
                    EmitSignal(nameof(StartedCooldown), CooldownTimeRemaining);
                }
            }
        }
    }

    public bool TryUseCharge()
    {
        if (CurrentCharges > 0)
        {
            CurrentCharges--;
            CooldownTimeRemaining = _playerSkill.GetCooldownTime();
            return true;
        }
        else
        {
            GD.Print("Could not use charge. Charge available in: ", CooldownTimeRemaining, "s");
            return false;
        }
    }

    public bool HasCharges()
    {
        // TODO: Handle skills with no charges
        return CurrentCharges > 0;
    }

    public void ResetCharges()
    {
        CurrentCharges = _playerSkill.GetMaxCharges();
        CooldownTimeRemaining = 0f;
    }
}