using Godot;
using System;
using TESTCS.levels;

public partial class SecondFloorLevel : BaseLevel
{
	private Area2D _exit;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_exit = GetNode<Area2D>("%Exit");
		_exit.BodyEntered += OnBodyEnteredExit;
	}

	private void OnBodyEnteredExit(Node2D body)
	{
		GlobalVariables.GameManager.LoadLevel(GlobalVariables.GameManager.Levels.StoneLevel);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
