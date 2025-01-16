using Godot;
using System;
using System.Collections.Generic;
using TESTCS.enums;
using TESTCS.skills.Modifiers;

public partial class Skill : Node
{
	public string SkillName;
	public int Cooldown;
	public List<SkillModifier> LocalModifiers = new();

	public virtual void ApplyModifiers()
	{
		GD.PushWarning("Modifiers for skill '", SkillName, "' are not being applied manually");
	}
	
	public virtual void Execute()
	{
		GD.PushWarning("Skill '", SkillName, "' is not being executed manually");
		// GD.Print("Executed skill: ", SkillName);
		//
		// // Apply global modifiers
		// foreach (var modifier in GlobalVariables.Instance.SkillManager.GlobalModifiers)
		// {
		// 	if (modifier.AppliesTo(this))
		// 	{
		// 		modifier.ApplyModifier(this);	
		// 	}
		// }
		//
		// // Apply local modifiers
		// foreach (var modifier in LocalModifiers)
		// {
		// 	if (modifier.AppliesTo(this))
		// 	{
		// 		modifier.ApplyModifier(this);	
		// 	}
		// }
	}
	
	// TODO: Add add/remove methods for localModifiers
}
