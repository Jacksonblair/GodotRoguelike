using Godot;
using System;
using TESTCS.actors;
using TESTCS.levels;
using TESTCS.managers.LevelManagers;
using TESTCS.projectiles;

public partial class GameManager : Node
{
	[Export] public GameProjectiles GameProjectiles { get; set; }
	
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
	
	// Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {}

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {}

	public void OnStartGame()
	{
		/**
		 * - Load player details (name, items?, stats?, configuration)
		 * - Load location
		 * - Spawn player actor in, get details from global store
		 * - Chuck actor into location
		 */

		LoadLastSave();
		var player = PlayerScene.Instantiate<PlayerCharacter>();
		GlobalVariables.Instance._character = player;
		GlobalVariables.ActiveMainSceneContainer.AddChild(player);
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
			UnloadLevelSpecificStuff();
			GlobalVariables.GameSceneManager.LoadGameScene(scene);
			LoadLevelSpecificStuff();
		}
		else
		{
			GD.PrintErr("The PackedScene does not inherit from BaseLevel. Aborting level load.");
			levelInstance.QueueFree(); // Cleanup the instanced node
		}
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
}
