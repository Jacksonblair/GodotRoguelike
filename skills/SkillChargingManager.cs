using System;
using System.Linq;
using Godot;

namespace TESTCS.skills;

public partial class SkillChargingManager : GodotObject
{
    public SkillHandler SkillHandler;
    
    public float ChargedFor { get; set; } = 0;
    public int ChargeStage { get; set; } = 0;

    public float NextStageAt
    {
        get
        {
            if (SkillHandler.SkillData.ChargingStages.Count < 1) return 0;
            if (ChargeStage >= SkillHandler.SkillData.ChargingStages.Count) return 0;
            return SkillHandler.SkillData.ChargingStages[ChargeStage];
        }
    }

    /** Returns the current percentage value of chargement to the next stage */
    public float PercentageStateCharged
    {
        get
        {
            var val = 0f;
            if (SkillHandler.SkillData.ChargingStages.Count < 1) return 0;
            if (ChargeStage == 0)
            {
                val = ChargedFor / GetNextStageBreakpoint();
            }
            else {
                // The base value should be the % from the previous breakpoint to the next breakpoint
                // Prev: 2, Next: 4. If value is 3, we are 50% there.
                // 2 - 2 == 0 and 4 - 2 == 2.  1/2 = .5
                
                // GD.Print("PREV BREAKPOINT: ", GetPreviousStageBreakpoint());
                // GD.Print("NEXT BREAKPOINT: ", GetNextStageBreakpoint());
                val = (ChargedFor - GetPreviousStageBreakpoint()) / (GetNextStageBreakpoint() - GetPreviousStageBreakpoint());
            }

            // GD.Print("PERCENTAGE CHARGED: ", val);
            return val;
            // 0 - 0.5 - 1 === 50%
            // 1 - 1.5 - 3 === 25%
            // 3 - 3 - 5 === 0%
        }
    }

    public float GetPreviousStageBreakpoint()
    {
        if (SkillHandler.SkillData.ChargingStages.Count < 1) return 0;
        if (ChargeStage == 0) return -1;
        return SkillHandler.SkillData.ChargingStages[ChargeStage - 1];
    }

    public float GetNextStageBreakpoint()
    {
        if (SkillHandler.SkillData.ChargingStages.Count < 1) return 0;
        return SkillHandler.SkillData.ChargingStages[Math.Min(ChargeStage, SkillHandler.SkillData.ChargingStages.Count - 1)];
    }

    public void Update(double delta)
    {
    // if skill has no charges, dont update
    if (SkillHandler.SkillData.ChargingStages.Count == 0) return;
    
    if (SkillHandler.SkillInputHandler.IsCharging)
    {
        if (GlobalVariables.PlayerCharacter != null)
        {
            GlobalVariables.PlayerCharacter.SkillChargingRing.SkillChargingManager = this;
        }
    
        ChargedFor += (float)delta;
        
        // Keep charging as long as we're not at the last stage
        // If we are at less than OR stage 3, and there are three stages.
        if (ChargeStage < SkillHandler.SkillData.ChargingStages.Count)
        {
            if (ChargedFor >= GetNextStageBreakpoint())
            {
                ChargeStage++;
                // GD.Print($"Stage increased to: {ChargeStage}");
            }
        }
        
        // GD.Print("CHARGING: ", ChargedFor);
        // GD.Print("CHARGE STAGE: ", ChargeStage);
        // GD.Print("NEXT STAGE AT: ", GetNextStageBreakpoint());
    }
    else
    {
        if (GlobalVariables.Instance._character != null)
        {
            GlobalVariables.PlayerCharacter.SkillChargingRing.SkillChargingManager = null;
        }
    
        ChargeStage = 0;
        ChargedFor = 0;
    }
    }
}