using Godot;
using TESTCS.skills.Modifiers;

public abstract partial class Skill : Node
{
    public override void _Process(double delta) {}

    // Force inheriting Skills to implement Execute
    public abstract void Execute(ModifierResults modifiers);
}