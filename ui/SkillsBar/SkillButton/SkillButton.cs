using Godot;
using TESTCS.skills;

namespace TESTCS.ui.SkillsBar.SkillButton;

public partial class SkillButton : TextureButton
{
    [Export] 
    private Label _label;
    private Label _chargeLabel;
    private TextureProgressBar _progressBar;
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
    }
    
    public override void _Process(double delta)
    {
        // if (SkillHandler == null)
        // {
        //     _chargeLabel.Text = "";
        //     _progressBar.Visible = false;
        // }
        // else
        // {
        //     // TODO: One day, do this more efficiently. Or dont, whatever.
        //
        //     _chargeLabel.Text = SkillHandler.SkillCooldownManager.CurrentCharges.ToString();
        //     _progressBar.MaxValue = SkillHandler.SkillData.CooldownTime;
        //     _progressBar.Value = SkillHandler.SkillCooldownManager.CooldownTimeRemaining;
        //     _progressBar.Visible = SkillHandler.SkillCooldownManager.CooldownTimeRemaining > 0;
        // }
    }
}