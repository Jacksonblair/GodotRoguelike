using System;
using System.Collections.Generic;
using Godot;
using TESTCS.managers;

// TODO: Derive SkillCooldownManager from a base CooldownManager class
namespace TESTCS.skills;

public class SkillCooldownManager
{
    private List<SkillSlotState> skillData;
    private Action<float, int> _startedCooldownHandler;
    private Action<int, int> _chargesChangedHandler;

    public SkillCooldownManager(List<SkillSlotState> SkillData, Action<float, int> startedCooldownHandler,
        Action<int, int> chargesChangedHandler)
    {
        // _skillData = skillData;
        // _cooldownTimer = 0f;
        // _currentCharges = _skillData.Charges;
        _startedCooldownHandler = startedCooldownHandler;
        _chargesChangedHandler = chargesChangedHandler;
        skillData = SkillData;
    }

    public void Update(double delta)
    {
        for (int i = 0; i < skillData.Count; i++)
        {
            var skill = skillData[i];
            if (skill == null) return; // Not guaranteed to have an object in each index

            // If we still have less charges than skill max charges
            if (skill.CurrentCharges < skill.GetMaxCharges())
            {
                // Reduce the cooldown
                skill.CurrentCooldown -= (float)delta;
                // If the cooldown is finished, restore a charge.
                // ...And then restart the cooldown for the next charge. 
                if (skill.CurrentCooldown <= 0f)
                {
                    skill.CurrentCharges++;
                    _chargesChangedHandler?.Invoke(skill.CurrentCharges, i);
                    GD.Print("Restoring charges: ", skill.CurrentCharges, "/", skill.GetMaxCharges());

                    if (skill.CurrentCharges < skill.GetMaxCharges())
                    {
                        skill.CurrentCooldown = skill.GetSkillCooldownTime();
                        _startedCooldownHandler?.Invoke(skill.CurrentCooldown, i);
                    }
                }
            }
        }
    }

    public bool TryUseCharge(int skillIndex)
    {
        var skill = skillData[skillIndex];
        if (skill == null) return false;

        // GD.Print("Skill: ", skill.SkillData.SkillName, " has a base cooldown of: ", skill.GetSkillCooldownTime());
        if (skill.CurrentCharges > 0)
        {
            // GD.Print("Used charge");
            skill.CurrentCharges--;
            _chargesChangedHandler?.Invoke(skill.CurrentCharges, skillIndex);
            skill.CurrentCooldown = skill.GetSkillCooldownTime();
            _startedCooldownHandler?.Invoke(skill.CurrentCooldown, skillIndex);

            // if (skill.CurrentCharges < skill.GetMaxCharges() && skill.CurrentCooldown <= 0f) {
            //     GD.Print("Charge restoring in: ", skill.CurrentCooldown);
            // }
            return true;
        }
        else
        {
            GD.Print("Could not use charge. Charge available in: ", skill.CurrentCooldown, "s");
        }

        return false;
    }

    public void ResetCharges(int skillIndex)
    {
        var data = skillData[skillIndex];
        if (data == null) return;
        data.CurrentCharges = data.GetMaxCharges();
        data.CurrentCooldown = 0f;
    }
}