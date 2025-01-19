using Godot;
using System;
using System.Collections.Generic;
using TESTCS.enums;
using TESTCS.skills.Modifiers;

public abstract partial class Skill : Node
{
    // Properties
    public string SkillName;
    public int BaseNumCharges = 1; // Number of times a skill can be used per cooldown
    public int BaseCooldown = 0;
    public int BaseDamage = 0;

    // TODO: Replace with something better
    public int NumCharges => BaseNumCharges + finalModifiers.AdditionalCharges;
    public int CooldownTime => BaseCooldown + finalModifiers.CooldownReduction;

    // Modifiers
    public List<SkillModifier> LocalModifiers = new();
    private ModifierHandler _handler = new();
    public ModifierResults finalModifiers = new();

    public override void _Process(double delta) {}

    // Calculate modifiers results
    public void ApplyModifiers()
    {
        var results = new ModifierResults();
        _handler.CalculateModifierResults(results, this.LocalModifiers);
        _handler.CalculateModifierResults(results, GlobalVariables.Instance.SkillManager.GlobalModifiers);
        this.finalModifiers = results;
    }

    public void ExecuteSkill()
    {
        // Before executing, apply modifiers
        ApplyModifiers();

        // Then call execution method on children
        this.Execute();
    }

    public abstract void Execute();
}