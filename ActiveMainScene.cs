using Godot;
using TESTCS.scripts.managers;

namespace TESTCS;

public partial class ActiveMainScene : Node2D
{
	[Export] public GameScenesEnum DefaultActiveScene = GameScenesEnum.MainMenu;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalVariables.Instance.GameSceneManager.LoadGameScene(DefaultActiveScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}