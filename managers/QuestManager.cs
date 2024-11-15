using System;
using Godot;

public partial class QuestManager : Node
{
    public override void _Ready()
    {
        // // Create a new instance of Quest1
        Quest1 quest1 = new Quest1();

        // // Add it to the scene tree as a child of QuestManager
        AddChild(quest1);
    }
}
