using Godot;
using System;
using Godot.NativeInterop;
using TESTCS.managers;

public partial class SkillButton : TextureButton
{
    [Export] 
    private Label _label;
    private Label _chargeLabel;
    private TextureProgressBar _progressBar;
    private float _cooldownTimeLeft;
    
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _chargeLabel = GetNode<Label>("ChargeLabel");
        _progressBar = GetNode<TextureProgressBar>("TextureProgressBar");
    }
    
    public void OnEquipSkill(string title, int defaultCharges)
    {
        GD.Print("Setting button label text to: ", title);
        _label.Text = title;
        _chargeLabel.Text = defaultCharges.ToString();
    }

    public void OnUnequipSkill()
    {
        _label.Text = "";
        _chargeLabel.Text = "";
    }

    public void SetCooldown(float timeleft)
    {
        GD.Print("SETTING BUTTON COOLDOWN TO: ", timeleft);
        
        _cooldownTimeLeft = timeleft;
        _progressBar.MaxValue = timeleft;
        _progressBar.Value = 0;
    }

    public void SetCharges(int charges)
    {
        _chargeLabel.Text = charges.ToString();   
    }
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_cooldownTimeLeft > 0)
        {
            if (!_progressBar.Visible) _progressBar.Visible = true;
            
            _cooldownTimeLeft -= (float)delta;
            _progressBar.Value = _cooldownTimeLeft;
        }
        else if (_progressBar.Visible)
        {
            // Hide overlay when cooldown is complete
            _progressBar.Visible = false;
        }
    }
}