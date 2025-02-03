using Godot;
using Godot.Collections;
using TESTCS.actors;
using TESTCS.managers;
using TESTCS.managers.LevelManagers;
using TESTCS.scripts;
using TESTCS.scripts.managers;

/** Purely for being a singleton of references to game objects */
[GlobalClass]
public partial class GlobalVariables : Node
{
    public const float Gravity = 200f;
    
    public static GlobalVariables Instance { get; private set; }
    
    public GameManager _gameManager { get; set; }
    
    public LevelManagers _levelManagers { get; set; }
    
    /** Global char reference */
    public PlayerCharacter _character { get; set; }
    
    /** Manages tracking kills */
    // public KillTrackingManager KillTrackingManager { get; set; }
    
    /** Manages levels */
    private GameSceneManager _gameSceneManager { get; set; }
    
    /** Manages game setup */
    public GameSetupyPoos GameSetup { get; set; }
    
    /** Parent node of active MAIN scene (level, menu, etc) */
    public Node2D _activeMainSceneContainer { get; set; }
    
    public GameStateManager _gamePersistenceManager { get; set; }
    
    // NICER ACCESSORS
    public static GameSceneManager GameSceneManager => Instance._gameSceneManager; // Correct alias
    public static GameManager GameManager => Instance._gameManager;
    public static GameStateManager GameStateManager => Instance._gamePersistenceManager;
    public static Node2D ActiveMainSceneContainer => Instance._activeMainSceneContainer;
    public static PlayerCharacter PlayerCharacter => Instance._character;
    public static LevelManagers LevelManagers => Instance._levelManagers;
    
    
    // When global variables script is ready, update all references in game. 
    public override void _Ready()
    {
        // TODO: Rejigger
        
        Instance = this;
        
        // Load first
        _gameSceneManager = GetNode<GameSceneManager>("/root/GameSceneManager");
        
        var MainScene = GetTree().Root.GetNode("Main");
        
        // Manages saving/loading the game state
        _gameManager = MainScene.GetNode<GameManager>("GameManager");
        _gamePersistenceManager = MainScene.GetNode<GameStateManager>("GameStateManager");
        _activeMainSceneContainer = MainScene.GetNode<Node2D>("ActiveMainScene");
        GameSetup = MainScene.GetNode<GameSetupyPoos>("GameSetupyPoos");
        
        GD.Print("Bootstrapped globals");
        // GD.Print("KIll mgr", KillTrackingManager);
        // GD.Print("GAME SCENE mgr", GameSceneManager);
        // GD.Print("QUEST mgr", QuestManager);
        // GD.Print("ACTIVE MAIN SCENE", ActiveMainSceneContainer);
    }
}
