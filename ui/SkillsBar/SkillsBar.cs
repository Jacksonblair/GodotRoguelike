using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TESTCS.managers;

public partial class SkillsBar : Control
{
	[Export] public PackedScene ButtonScene; // The button scene to instantiate

	private SkillSlotManager _skillSlotManager;
	private readonly List<SkillButton> _skillButtons = new();

	// Hover elements
	private Panel _skillHoverPanel;
	private Label _skillHoverLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_skillSlotManager = GlobalVariables.Instance.SkillSlotManager;
		_skillHoverPanel = GetNode<Panel>("SkillHoverPanel");
		_skillHoverLabel = _skillHoverPanel. GetNode<Label>("Label");
			
		var ctr = GetNode<HBoxContainer>("HBoxContainer");
		
		GD.Print(_skillSlotManager.SkillSlots.Count);
		
		// Create buttons as children of hbox container
		for (int i = 0; i < _skillSlotManager.SkillSlots.Count; i++)
		{
			var button = ButtonScene.Instantiate<SkillButton>();
			button.SkillIndex = i;
			ctr.AddChild(button);
			var i1 = i;
			button.MouseEntered += () => ShowButtonTooltip(i1);
			button.MouseExited += HideButtonTooltip;
			_skillButtons.Add(button);
		}
	}

	// public override void _Process(double delta) {}

	private void HideButtonTooltip()
	{
		// GD.Print("HIDE TOOLTIP");
	}

	private void ShowButtonTooltip(int index)
	{
		// GD.Print("SHOW TOOLTIP FOR BUTTON AT INDEX: ", index);
        
		ShowTooltip(index);
	}

	private void ShowTooltip(int index)
	{
		var skill = _skillButtons[index];
		if (skill == null) return;
		
		_skillHoverLabel.Text = "HELLO";
		_skillHoverPanel.Visible = true;
		_skillHoverPanel.GlobalPosition = GetViewport().GetMousePosition() + new Vector2(10, 10);;
	}
}
