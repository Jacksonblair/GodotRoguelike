using Godot;
using TESTCS.scripts;

[GlobalClass]
public partial class GlobalVariables : Node
{
    public static GlobalVariables Instance { get; private set; }
    
    /** Global char reference */
    public CharacterBody2D Character { get; set; }
    
    /** Manages tracking kills */
    public KillTrackingManager KillTrackingManager { get; set; }
    
    /** Manages levels */
    public TESTCS.scripts.managers.GameSceneManager GameSceneManager { get; set; }
    
    /** Manages tracking quests */
    public QuestManager QuestManager { get; set; }

    /** Parent node of active MAIN scene (level, menu, etc) */
    public Node2D ActiveMainScene { get; set; }
    
    public override void _Ready()
    {
        Instance = this;
        KillTrackingManager = GetNode<KillTrackingManager>("/root/KillTrackingManager");
        GameSceneManager = GetNode<TESTCS.scripts.managers.GameSceneManager>("/root/LevelManager");
        QuestManager = GetNode<QuestManager>("/root/QuestManager");
        ActiveMainScene = GetTree().Root.GetNode("Main").GetNode<Node2D>("ActiveMainScene");
        GD.Print("Bootstrapped globals");
        GD.Print("KIll mgr", KillTrackingManager);
        GD.Print("GAME SCENE mgr", GameSceneManager);
        GD.Print("QUEST mgr", QuestManager);
        GD.Print("ACTIVE MAIN SCENE", ActiveMainScene);
    }
}
