using Godot;
using System;
using Godot.NativeInterop;
using TESTCS.managers;
using TESTCS.skills;

public partial class SkillButton : TextureButton
{
    [Export] 
    private Label _label;
    private Label _chargeLabel;
    private TextureProgressBar _progressBar;
    public SkillHandler SkillState;
    public int SkillIndex;
    
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _chargeLabel = GetNode<Label>("ChargeLabel");
        _progressBar = GetNode<TextureProgressBar>("TextureProgressBar");
    }

    public void OnUnequipSkill()
    {
        _label.Text = "";
        _chargeLabel.Text = "";
        _progressBar.Value = 0;
        _progressBar.MaxValue = 0;
        _progressBar.Visible = false;
        SkillState = null;
    }
    
    public override void _Process(double delta)
    {
        var skill = GlobalVariables.Instance.SkillSlotManager.SkillSlots[SkillIndex];
        if (skill == null)
        {
            _chargeLabel.Text = "";
            _progressBar.Visible = false;
        }
        else
        {
            // TODO: One day, do this more efficiently. Or dont, whatever.
            _chargeLabel.Text = skill.SkillCooldownManager.CurrentCharges.ToString();
            _progressBar.MaxValue = skill.SkillData.CooldownTime;
            _progressBar.Value = skill.SkillCooldownManager.CooldownTimeRemaining;
            _progressBar.Visible = skill.SkillCooldownManager.CooldownTimeRemaining > 0;
        }
    }
}