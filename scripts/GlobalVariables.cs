using Godot;
using Godot.Collections;
using TESTCS.managers;
using TESTCS.scripts;
using TESTCS.scripts.managers;

/** Purely for being a singleton of references to game objects */
[GlobalClass]
public partial class GlobalVariables : Node
{
    
    public static GlobalVariables Instance { get; private set; }
    
    public GameManager _gameManager { get; set; }
    
    /** Manages Skills */
    public SkillSlotManager SkillSlotManager { get; set; }
    
    /** Global char reference */
    public PlayerCharacter Character { get; set; }
    
    /** Manages tracking kills */
    // public KillTrackingManager KillTrackingManager { get; set; }
    
    /** Manages levels */
    private GameSceneManager _gameSceneManager { get; set; }
    
    /** Manages game setup */
    public GameSetupyPoos GameSetup { get; set; }
    
    /** Manages tracking quests */
    // public QuestManager QuestManager { get; set; }

    /** Parent node of active MAIN scene (level, menu, etc) */
    public Node2D ActiveMainSceneContainer { get; set; }
    
    public GameStateManager _gamePersistenceManager { get; set; }
    
    
    // NICER ACCESSORS
    // TODO: The rest
    public static GameSceneManager GameSceneManager => Instance._gameSceneManager; // Correct alias
    public static GameManager GameManager => Instance._gameManager;
    public static GameStateManager GameStateManager => Instance._gamePersistenceManager;
    
    // When global variables script is ready, update all references in game. 
    public override void _Ready()
    {
        Instance = this;
        
        // Load first
        _gameSceneManager = GetNode<GameSceneManager>("/root/GameSceneManager");
        
        // Manages skill slots for the player
        // SkillSlotManager = GetTree().Root.GetNode("Main").GetNode<SkillSlotManager>("SkillSlotManager");
        var MainScene = GetTree().Root.GetNode("Main");
        
        // Manages saving/loading the game state
        _gameManager = MainScene.GetNode<GameManager>("GameManager");
        _gamePersistenceManager = MainScene.GetNode<GameStateManager>("GameStateManager");
        ActiveMainSceneContainer = MainScene.GetNode<Node2D>("ActiveMainScene");
        GameSetup = MainScene.GetNode<GameSetupyPoos>("GameSetupyPoos");
        
        // KillTrackingManager = GetNode<KillTrackingManager>("/root/KillTrackingManager");
        // QuestManager =  GetNode<QuestManager>("/root/QuestManager");
        // SkillManager = GetTree().Root.GetNode("Main").GetNode<Node>("SkillManager");

        // var nd = GetTree().Root.GetNode("Main").GetNode("SkillManager");

        // GD.Print(nd.GetType());
        // GD.Print(QuestManager.GetType());
        
        GD.Print("Bootstrapped globals");
        // GD.Print("KIll mgr", KillTrackingManager);
        // GD.Print("GAME SCENE mgr", GameSceneManager);
        // GD.Print("QUEST mgr", QuestManager);
        // GD.Print("ACTIVE MAIN SCENE", ActiveMainSceneContainer);
    }
}
