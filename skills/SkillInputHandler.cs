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

public partial class SkillInputHandler : Godot.GodotObject
{
    [Signal]
    public delegate void StartedChargingSkillEventHandler();
    [Signal]
    public delegate void ExecutedSkillEventHandler();
    private bool _pressed;
    
    public void Update(PlayerInputs playerInput)
    {
        var inputName = EnumHelper.GetEnumName(playerInput);
        
        if (Input.IsActionPressed(inputName) && !_pressed)
        {
            EmitSignal(nameof(StartedChargingSkill));
        }

        if (Input.IsActionJustReleased(inputName))
        {
            EmitSignal(nameof(ExecutedSkill));
        }
    }
}