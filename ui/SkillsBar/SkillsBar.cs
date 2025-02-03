using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SkillsBar : Control
{
	[Export] public PackedScene ButtonScene; // The button scene to instantiate

	private readonly List<TESTCS.ui.SkillsBar.SkillButton.SkillButton> _skillButtonRefs = new();

	// Hover elements
	private Panel _skillHoverPanel;
	private Label _skillHoverLabel;
	private HBoxContainer _hBoxContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("THIS SHOULD RUN THIRD");
		
		GlobalVariables.LevelManagers.PlayerSkillSlotManager.EquippedSkill += OnEquippedSkill;
		GlobalVariables.LevelManagers.PlayerSkillSlotManager.UnequippedSkill += OnUnequippedSkill;
		
		_skillHoverPanel = GetNode<Panel>("%SkillHoverPanel");
		_skillHoverLabel = GetNode<Label>("%SkillHoverLabel");
		_hBoxContainer = GetNode<HBoxContainer>("%HBoxContainer");
		
		for (int i = 0; i < GlobalVariables.LevelManagers.PlayerSkillSlotManager.SkillSlots.Count; i++)
		{
			var button = ButtonScene.Instantiate<TESTCS.ui.SkillsBar.SkillButton.SkillButton>();
			button.SkillIndex = i;
			_hBoxContainer.AddChild(button);
			var i1 = i;
			button.MouseEntered += () => ShowButtonTooltip(i1);
			button.MouseExited += HideButtonTooltip;
			_skillButtonRefs.Add(button);
		}
	}

	private void OnUnequippedSkill(int abilityindex)
	{
		if (abilityindex + 1 > _skillButtonRefs.Count) return;
		var button = _skillButtonRefs[abilityindex];
		button.SkillHandler = null;
		// throw new NotImplementedException();
	}

	private void OnEquippedSkill(int abilityindex)
	{
		if (abilityindex + 1 > _skillButtonRefs.Count) return;
		var button = _skillButtonRefs[abilityindex];
		button.SkillHandler = GlobalVariables.LevelManagers.PlayerSkillSlotManager.SkillSlots[abilityindex];
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
		var skill = _skillButtonRefs[index];
		if (skill == null) return;
		
		_skillHoverLabel.Text = "HELLO";
		_skillHoverPanel.Visible = true;
		_skillHoverPanel.GlobalPosition = GetViewport().GetMousePosition() + new Vector2(10, 10);;
	}
}
