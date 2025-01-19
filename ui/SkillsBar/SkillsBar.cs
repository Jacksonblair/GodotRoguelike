using Godot;
using System;
using System.Collections.Generic;
using TESTCS.managers;

public partial class SkillsBar : CanvasLayer
{
	[Export] public PackedScene ButtonScene; // The button scene to instantiate

	private SkillManager _skillManager;
	private List<SkillButton> _buttons = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_skillManager = GlobalVariables.Instance.SkillManager;

		// Create buttons as children
		for (int i = 0; i < _skillManager.MaxSlots; i++)
		{
			var button = ButtonScene.Instantiate<SkillButton>();
			AddChild(button);
			var i1 = i;
			button.MouseEntered += () => ShowButtonTooltip(i1);
			button.MouseExited += HideButtonTooltip;
			
			_buttons.Add(button);
		}
		
		// Add signal listeners to skill mgr
		_skillManager.SkillEquipped += OnSkillEquipped;
		_skillManager.SkillUnequipped += OnSkillUnequipped;
	}

	private void HideButtonTooltip()
	{
		GD.Print("HIDE TOOLTIP");
	}

	private void ShowButtonTooltip(int index)
	{
		GD.Print("SHOW TOOLTIP FOR BUTTON AT INDEX: ", index);
		ShowTooltip(index);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{}
	
	/**
	 *
	 * We need to know:
	 * - How many charges a skill has (so we need to be told how many charges)
	 * - When the skill is on cooldown, and how long the cooldown will take
	 */
	
	private void OnSkillUnequipped(SkillData skilldata, int skillindex)
	{
		var button = _buttons[skillindex];
		button?.SetButtonContent("");

		GD.Print("Skilled unequipped");
	}

	private void OnSkillEquipped(SkillData skilldata, Skill skillnode, int skillindex)
	{
		GD.Print("Trying to equip skill: ", skilldata.SkillName, " at index: ", skillindex);
        
		var button = _buttons[skillindex];
		button?.SetButtonContent(skilldata.SkillName);
	}
	
	private void ShowTooltip(int index) {}
}
