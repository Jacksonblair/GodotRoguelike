using System;
using Godot;
using TESTCS.enums;
using TESTCS.helpers;
using TESTCS.managers;

namespace TESTCS.skills;

public class InputState
{
    public bool Pressed;
    public bool JustPressed;
    public bool JustReleased;

    public InputState(bool Pressed, bool JustPressed, bool JustReleased)
    {
        this.Pressed = Pressed;
        this.JustPressed = JustPressed;
        this.JustReleased = JustReleased;
    }
}

// TODO: Make one of these for enemies, has to be generic
public partial class SkillInputHandler : GodotObject
{
    private PlayerSkill _playerSkill;
    
    [Signal]
    public delegate void StartedChargingSkillEventHandler();
    [Signal]
    public delegate void TryExecuteSkillEventHandler();

    public SkillInputHandler(PlayerSkill playerSkill)
    {
        this._playerSkill = playerSkill;
    }
    
    [Export] public float ChargeThreshold = 0.0f; // Time required to consider a press as charging
    private float _chargeTimer = 0;
    
    // 'Charging' related properties
    public bool IsCharging { get; set; } = false;
    
    public void Update(double delta, PlayerInputs playerInput)
    {
        var inputName = EnumHelper.GetEnumName(playerInput);
        
        if (Input.IsActionPressed(inputName))
        {
            
            _chargeTimer += (float)delta;

            if (!IsCharging && _chargeTimer >= ChargeThreshold)
            {
                IsCharging = true;
                EmitSignal(nameof(StartedChargingSkill));
            }
        }
        else
        {
            IsCharging = false;
            _chargeTimer = 0;
        }

        if (Input.IsActionJustReleased(inputName))
        {
            IsCharging = false;
            EmitSignal(nameof(TryExecuteSkill));
        }
    }
}