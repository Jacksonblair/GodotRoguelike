using Godot;
using Godot.Collections;

namespace TESTCS.scripts.managers;

/**
 * Requirements:
 * - Able to persistently configure a scene that i want to be the first scene that loads when i press Play
 * - All the game scene related shit shouldn't load until i have a game scene loaded.
 *
 * --- I need to load this gamestate when the game starts, and update it as I go.
 *
 * Problems:
 * - Identify a packed scene using a string
 * - Track when scene we are in using string
 *      - Tell GameState what scene we are in when changing
 * - Load that scene again when we open game
 *      - Game should grab scene based on GameState, and put into the game
 */


[GlobalClass]
public partial class GameStateManager : Node
{
    private static string GameStateFilePath = "user://gamestate.tres";
    
    private PackedScene StoneLevel;
    private PackedScene OtherLevel;
    
    [Export]
    public GameState GameState { get; set; }
    
    public void SaveGameState()
    {
        var error =ResourceSaver.Save(GameState, GameStateFilePath);
        GD.Print(error);
    }
    
    public void LoadGameState()
    {
        // Load the saved game state or create a new one if none exists
        if (FileAccess.FileExists(GameStateFilePath))
        {
            GameState = ResourceLoader.Load<GameState>(GameStateFilePath);
            GD.Print(GameState);
        }
        else
        {
            GameState = new GameState();
            SaveGameState();
        }
    }

}