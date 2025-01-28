using Godot;
using Godot.Collections;
using TESTCS.managers;
using TESTCS.scripts;
using TESTCS.scripts.managers;

[GlobalClass]
public partial class GlobalVariables : Node
{
    public static GlobalVariables Instance { get; private set; }
    
    /** Manages Skills */
    public SkillSlotManager SkillSlotManager { get; set; }
    
    /** Global char reference */
    public PlayerCharacter Character { get; set; }
    
    /** Manages tracking kills */
    public KillTrackingManager KillTrackingManager { get; set; }
    
    /** Manages levels */
    public GameSceneManager GameSceneManager { get; set; }
    
    /** Manages tracking quests */
    public QuestManager QuestManager { get; set; }

    /** Parent node of active MAIN scene (level, menu, etc) */
    public Node2D ActiveMainSceneContainer { get; set; }
    
    public GameStateManager GameStateManager { get; set; }
    
    // When global variables script is ready, update all references in game. 
    public override void _Ready()
    {
        Instance = this;
        
        KillTrackingManager = GetNode<KillTrackingManager>("/root/KillTrackingManager");
        GameSceneManager = GetNode<TESTCS.scripts.managers.GameSceneManager>("/root/GameSceneManager");
        QuestManager =  GetNode<QuestManager>("/root/QuestManager");
        
        ActiveMainSceneContainer = GetTree().Root.GetNode("Main").GetNode<Node2D>("ActiveMainScene");
        // SkillManager = GetTree().Root.GetNode("Main").GetNode<Node>("SkillManager");

        // var nd = GetTree().Root.GetNode("Main").GetNode("SkillManager");
        SkillSlotManager = GetTree().Root.GetNode("Main").GetNode<TESTCS.managers.SkillSlotManager>("SkillSlotManager");
        GameStateManager = GetTree().Root.GetNode("Main").GetNode<GameStateManager>("GameStateManager");

        // GD.Print(nd.GetType());
        // GD.Print(QuestManager.GetType());
        
        GD.Print("Bootstrapped globals");
        // GD.Print("KIll mgr", KillTrackingManager);
        // GD.Print("GAME SCENE mgr", GameSceneManager);
        // GD.Print("QUEST mgr", QuestManager);
        // GD.Print("ACTIVE MAIN SCENE", ActiveMainSceneContainer);
    }
}
