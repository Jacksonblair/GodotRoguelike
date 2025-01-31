using Godot;
using System;
using TESTCS.levels;
using TESTCS.managers.LevelManagers;

public partial class GameManager : Node
{
	[Export] public Levels Levels { get; set; }
	[Export] public PackedScene MainMenuScene { get; set; }
	[Export] public PackedScene CreditsScene { get; set; }
	[Export] public PackedScene LevelManagersPackedScene { get; set; }
	private Node LevelManagersScene;
	
	// Emit signal when level managers are ready. 
	[Signal]
	public delegate void LevelManagersReadyEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	public void LoadMainMenu()
	{
		UnloadLevelManagers();
		GlobalVariables.GameSceneManager.LoadGameScene(MainMenuScene);
	}

	public void LoadLastSave()
	{
		GlobalVariables.GameStateManager.CreateNewGameSave(1);
		// GlobalVariables.GameStateManager.LoadMostRecentGameState(1);
		
		GD.Print(GlobalVariables.Instance._gamePersistenceManager.GameState.CurrentSceneID);
		switch (GlobalVariables.GameStateManager.GameState.CurrentSceneID)
		{
			case 1:
				LoadLevel(Levels.StoneLevel);
				// GlobalVariables.GameSceneManager.LoadGameScene(Levels.StoneLevel);
				break;
			default:
				return;
		}
	}
	
	public void LoadLevel(PackedScene scene)
	{
		// Instance the scene to check its type
		Node levelInstance = scene.Instantiate();

		if (levelInstance is BaseLevel)
		{
			// Proceed with the level loading process
			UnloadLevelManagers();
			GlobalVariables.GameSceneManager.LoadGameScene(scene);
			LoadLevelManagers();
		}
		else
		{
			GD.PrintErr("The PackedScene does not inherit from BaseLevel. Aborting level load.");
			levelInstance.QueueFree(); // Cleanup the instanced node
		}
	}

	public void LoadCredits()
	{
		UnloadLevelManagers();
		GlobalVariables.GameSceneManager.LoadGameScene(CreditsScene);
	}

	private void UnloadLevelManagers()
	{
		if (LevelManagersScene == null) return;
		LevelManagersScene.QueueFree();
		LevelManagersScene = null;
		GlobalVariables.Instance._levelManagers = null;
	}

	private void LoadLevelManagers()
	{
		var levelManagers = LevelManagersPackedScene.Instantiate();
		GlobalVariables.Instance.ActiveMainSceneContainer.AddChild(levelManagers);
		LevelManagersScene = levelManagers;
		GlobalVariables.Instance._levelManagers = (LevelManagers)levelManagers;
		CallDeferred("NotifyLevelManagersReady");
	}

	// Emit signal notifying that level managers are ready to go
	private void NotifyLevelManagersReady()
	{
		EmitSignal(nameof(LevelManagersReady));
	}
}
