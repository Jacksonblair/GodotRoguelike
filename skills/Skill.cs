using Godot;
using TESTCS.actors;
using TESTCS.enums;
using TESTCS.managers;
using TESTCS.skills;
using TESTCS.skills.Modifiers;

public abstract partial class Skill : Node
{
    public PlayerInputs MappedInput;
    public SkillHandler SkillHandler;

    // Force inheriting Skills to implement Execute
    public abstract void Execute(Actor executedBy, ModifierResults modifiers);
}