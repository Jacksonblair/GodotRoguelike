using System;
using DialogueManagerRuntime;
using Godot;

public partial class Npc1 : Area2D, IInteractable
{
    private Area2D area2d;
    private Label label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set up hide/show label when we are close
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        label = GetNode<Label>("Label");
    }

    private void OnBodyExited(Node2D body)
    {
        label.Hide();
    }

    private void OnBodyEntered(Node2D body)
    {
        label.Show();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta) { }

    public void Interact()
    {
        var dialogue = GD.Load<Resource>("res://dialogue/example.dialogue");
        DialogueManager.ShowExampleDialogueBalloon(dialogue, "start");
    }
}
