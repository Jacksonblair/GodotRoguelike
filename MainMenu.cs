using Godot;
using System;
using TESTCS.scripts.managers;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var button = GetNode<Button>("StartButton");
		button.Pressed += OnStartButtonPressed;
	}

	private void OnStartButtonPressed()
	{
		GlobalVariables.Instance.GameSceneManager.LoadGameScene(GameScenesEnum.StoneLevel);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
