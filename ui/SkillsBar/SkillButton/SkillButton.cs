using Godot;
using System;
using TESTCS.managers;

public partial class SkillButton : TextureButton
{
    [Export] 
    public int ButtonIndex { get; set; }
    // private SkillData _skillData;
    // private Skill _skillNode;
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void SetButtonContent(string title)
    {
        _label.Text = title;
    }
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {}
}