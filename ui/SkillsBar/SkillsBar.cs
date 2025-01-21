using Godot;
using System;
using System.Collections.Generic;
using TESTCS.managers;

public partial class SkillsBar : Control
{
	[Export] public PackedScene ButtonScene; // The button scene to instantiate

	private SkillManager _skillManager;
	private readonly List<SkillButton> _skillButtons = new();

	// Hover elements
	private Panel _skillHoverPanel;
	private Label _skillHoverLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_skillManager = GlobalVariables.Instance.SkillManager;
		_skillHoverPanel = GetNode<Panel>("SkillHoverPanel");
		_skillHoverLabel = _skillHoverPanel. GetNode<Label>("Label");
			
		var ctr = GetNode<HBoxContainer>("HBoxContainer");
		
		// Create buttons as children of hbox container
		for (int i = 0; i < _skillManager.SkillSlots.Count; i++)
		{
			var button = ButtonScene.Instantiate<SkillButton>();
			ctr.AddChild(button);
			var i1 = i;
			button.MouseEntered += () => ShowButtonTooltip(i1);
			button.MouseExited += HideButtonTooltip;
			_skillButtons.Add(button);
		}
		
		GD.Print(_skillManager);
		
		// Add signal listeners to skill mgr
		_skillManager.SkillEquipped += OnSkillEquipped;
		_skillManager.SkillUnequipped += OnSkillUnequipped;
		_skillManager.NumberChargesChanged += OnSkillNumChargesChanged;
		_skillManager.StartedCooldown += OnSkillStartedCooldown;
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
	
	private void OnSkillUnequipped(int skillindex)
	{
		var button = _skillButtons[skillindex];
		button?.OnUnequipSkill();
	}

	private void OnSkillEquipped(SkillData skilldata, int skillindex)
	{
		// GD.Print("Trying to equip skill: ", skilldata.SkillName, " at index: ", skillindex);
        
		var button = _skillButtons[skillindex];
		GD.Print(button);
		GD.Print(skillindex);
		
		button?.OnEquipSkill(skilldata.SkillName, skilldata.Charges);
	}
	
	private void OnSkillStartedCooldown(float timeleft, int skillindex)
	{	
		GD.Print("Cooldown started");
		var button = _skillButtons[skillindex];
		button?.SetCooldown(timeleft);
	}

	private void OnSkillNumChargesChanged(int numcharges, int skillindex)
	{
		GD.Print("Charges changed");
		var button = _skillButtons[skillindex];
		button?.SetCharges(numcharges);
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
