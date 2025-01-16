using Godot;
using Godot.Collections;

namespace TESTCS.scripts.managers;

public enum GameScenesEnum
{
    MainMenu,
    StoneLevel
}

/** Manages the current MAIN scene */
public partial class GameSceneManager : Node
{
    private static readonly Dictionary<GameScenesEnum, NodePath> GameScenesPathDictionary = new Dictionary<GameScenesEnum, NodePath>
    {
        {
            GameScenesEnum.StoneLevel, "res://levels/stone_level.tscn"
        },
        {
            GameScenesEnum.MainMenu, "res://main_menu.tscn"
        }
    };

    private Node _currentActiveScene; 
    
    public override void _Ready()
    {
        GD.Print("GameSceneManager initialized");
    }

    public void LoadGameScene(GameScenesEnum gameScene)
    {
        // Remove previous scene
        if (_currentActiveScene != null)
        {
            GlobalVariables.Instance.ActiveMainSceneContainer.RemoveChild(_currentActiveScene);
        }
        
        // Instantiate specified scene
        var scene = GetGameScenePacked(gameScene);
        var inst = scene.Instantiate();
        GlobalVariables.Instance.ActiveMainSceneContainer.AddChild(inst);
        
        // Update internal reference
        _currentActiveScene = inst;
        
        GD.Print("Loading: ", gameScene);
        GD.Print(GlobalVariables.Instance.ActiveMainSceneContainer);

        // Add player to level
        // Rather... Despawn old level including player... and then respawn player in new level
        // TODO: Unload old level
        // TODO: Transition?
    }

    private static NodePath GetGameScenePath(GameScenesEnum gameScene)
    {
        return GameScenesPathDictionary[gameScene];
    }

    private static PackedScene GetGameScenePacked(GameScenesEnum gameScene)
    { 
        return GD.Load<PackedScene>(GetGameScenePath(gameScene));
    }

    // Should this go elsewhere?
    // public void AddPlayerToScene()
    // {
    //     var scene = GD.Load<PackedScene>("res://scenes/PlayerCharacter.tscn");
    //     var inst = scene.Instantiate<PlayerCharacter>();
    //     GlobalVariables.Instance.ActiveMainScene.AddChild(inst);
    //     GlobalVariables.Instance.Character = inst;
    // }
}