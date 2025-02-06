using Godot;
using System;
using TESTCS.actors;
using TESTCS.enemies;
using TESTCS.levels;
using TESTCS.managers.LevelManagers;
using TESTCS.projectiles;

public enum LevelsEnum
{
	NONE,
	StoneLevel,
	SecondFloorLevel
}

public partial class GameManager : Node
{
	[Export] public GameProjectiles GameProjectiles { get; set; }
	[Export] public Enemies Enemies { get; set; }
	
	// public ENetMultiplayerPeer EnetPeer = new ENetMultiplayerPeer();
	[Export] public PackedScene PlayerScene { get; set; }
	
	[Export] public Levels Levels { get; set; }
	[Export] public PackedScene MainMenuScene { get; set; }
	[Export] public PackedScene CreditsScene { get; set; }

	[Export] public PackedScene LevelUIPackedScene { get; set; }
	private Node LevelUISceneRef;

	[Export] public PackedScene LevelManagersPackedScene { get; set; }
	private Node LevelManagersSceneRef;
	
	// Emit signal when level managers are ready. 
	[Signal]
	public delegate void LevelManagersReadyEventHandler();
	
	public void OnStartGame()
	{
		LoadLastSave();
	}

	public void LoadMainMenu()
	{
		UnloadLevelSpecificStuff();
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
				LoadLevel(LevelsEnum.StoneLevel);
				break;
			default:
				return;
		}
	}
	
	public async void LoadLevel(LevelsEnum scene, int targetDoorId = -1)
	{
		if (scene == LevelsEnum.NONE)
		{
			GD.Print("No scene specified. Stopping loading level");
		}
		
		GlobalVariables.GameSceneManager.LoadTransitionScene(Levels.Transition1);
		await GlobalVariables.GameSceneManager.CurrentTransitionScene.TransitionIn();
		
		GlobalVariables.GameSceneManager.UnloadCurrentGameScene();

		// Remove all enemies
		// Remove all projectiles
		RemoveAllEnemies();
		RemoveAllProjectiles();
		
		// Remove character
		if (GlobalVariables.PlayerCharacter != null)
		{
			GlobalVariables.PlayerCharacter.QueueFree();
		}
		
		// Add player before adding scene, otherwise things break atm
		// TODO: Fix this one day ^
		var player = PlayerScene.Instantiate<PlayerCharacter>();	
		GlobalVariables.Instance._character = player;
		GlobalVariables.ActiveMainSceneContainer.AddChild(player);
		
		// Load in the level stuff
		UnloadLevelSpecificStuff();
		
		// TODO: THIS IS FUCKED REDO IT
		if (scene == LevelsEnum.StoneLevel)
		{
			GlobalVariables.GameSceneManager.LoadGameScene(Levels.StoneLevel);
		}

		if (scene == LevelsEnum.SecondFloorLevel)
		{
			GlobalVariables.GameSceneManager.LoadGameScene(Levels.SecondFloorLevel);
		}
		
		LoadLevelSpecificStuff();

		// Based on 
		if (GlobalVariables.GameSceneManager.CurrentActiveScene is Level)
		{
			Vector2 spawnPosition = Vector2.Zero;
			var doors = GetTree().GetNodesInGroup("LevelDoor");
			foreach (var door in doors)
			{
				if (door is LevelDoor levelDoor && levelDoor.DoorId == targetDoorId)
				{
					levelDoor.DeactivateDoorUntilReentry();
					spawnPosition = levelDoor.GlobalPosition;
				}
			}

			player.Position = spawnPosition;
		}
		
		// TODO: NEED TO PUT PLAYER IN THE SPAWN POSITION IN LEVEL
		// CLEAR UP THIS PROCEDURE

		await GlobalVariables.GameSceneManager.CurrentTransitionScene.TransitionOut();
		GlobalVariables.GameSceneManager.UnloadCurrentTransitionScene();
	}

	public void LoadCredits()
	{
		UnloadLevelSpecificStuff();
		GlobalVariables.GameSceneManager.LoadGameScene(CreditsScene);
	}

	private void UnloadLevelSpecificStuff()
	{
		if (LevelManagersSceneRef != null)
		{
			LevelManagersSceneRef.QueueFree();
			LevelManagersSceneRef = null;
			GlobalVariables.Instance._levelManagers = null;
		}

		if (LevelUISceneRef != null)
		{
			LevelUISceneRef.QueueFree();
			LevelUISceneRef = null;
		}
	}

	private void LoadLevelSpecificStuff()
	{
		// Setup level managers
		var levelManagers = LevelManagersPackedScene.Instantiate();
		LevelManagersSceneRef = levelManagers;
		GlobalVariables.Instance._levelManagers = (LevelManagers)levelManagers;
		GlobalVariables.Instance._activeMainSceneContainer.AddChild(levelManagers);

		// Setup level UI	
		var levelUI = LevelUIPackedScene.Instantiate<LevelUI>();
		LevelUISceneRef = levelUI;
		GlobalVariables.Instance._activeMainSceneContainer.AddChild(levelUI);
	}
	
	// public void OnHostButtonPressed()
	// {
	// 	// TODO: Hide main menu
	// 	EnetPeer.CreateServer(GlobalVariables.Port);
	// 	Multiplayer.MultiplayerPeer = EnetPeer;
	// 	Multiplayer.PeerConnected += OnPeerConnected;
	//
	// 	LoadLevel(Levels.StoneLevel);
	// 	AddPlayer(Multiplayer.GetUniqueId());
	// }

	// private void OnPeerConnected(long id)
	// {
	// 	GD.Print("Player joined, peer id: ", id.ToString());
	// 	AddPlayer(id);	
	// }

	// public void OnJoinButtonPressed(string serverAddress)
	// {
	// 	// TODO: Hide main menu
	// 	EnetPeer.CreateClient(serverAddress, GlobalVariables.Port);
	// 	Multiplayer.MultiplayerPeer = EnetPeer;
	// 	LoadLevel(Levels.StoneLevel);	
	// }

	// public void AddPlayer(long peerID)
	// {
	// 	var player = PlayerScene.Instantiate<PlayerCharacter>();
	// 	player.Name = peerID.ToString();
	// 	player.SetMultiplayerAuthority((int)peerID);
	// 	GlobalVariables.ActiveMainSceneContainer.AddChild(player);
	// 	
	// 	GD.Print($"Spawned player node: {player.Name} for peerID: {peerID}");
	//
	// 	// Instantiate player scene
	// 	// Assign PeerID to it as ID
	// 	// Add it to scene
	// }

	private void RemoveAllEnemies()
	{
		var enemies = GetTree().GetNodesInGroup("Enemy");
		foreach (var enemy in enemies)
		{
			enemy.QueueFree();
		}
	}

	private void RemoveAllProjectiles()
	{
		var projectiles = GetTree().GetNodesInGroup("Projectile");
		foreach (var p in projectiles)
		{
			p.QueueFree();
		}
	}
}
