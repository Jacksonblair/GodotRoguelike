using Godot;
using Godot.Collections;

namespace TESTCS.scripts.managers;

/** Specifically for loading/unloading scenes and handling transitions */
public partial class GameSceneManager : Node
{
    private Node _currentActiveScene;
    
    [Export]
    public PackedScene MainMenuScene { get; set; }

    public override void _Ready()
    {}

    public void LoadGameScene(PackedScene gameScene)
    {
        // Remove previous scene
        if (_currentActiveScene != null)
        {
            GlobalVariables.Instance._activeMainSceneContainer.RemoveChild(_currentActiveScene);
        }
        
        // Instantiate specified scene
        var inst = gameScene.Instantiate();
        GlobalVariables.Instance._activeMainSceneContainer.AddChild(inst);
        
        // Update internal reference
        _currentActiveScene = inst;
        
        GD.Print("Loading: ", gameScene);
        GD.Print(GlobalVariables.Instance._activeMainSceneContainer);

        // Add player to level
        // Rather... Despawn old level including player... and then respawn player in new level
        // TODO: Unload old level
        // TODO: Transition?
    }

    
    // public void LoadGameLevel(PackedScene scene)
    // {
    //     /**
    //      * When loading a level
    //      * - Make sure level managers are set up
    //      */
    // }
    //
    // public void LoadMenuLevel(PackedScene scene)
    // {
    //     /**
    //      * When loading a menu
    //      * - Make sure level managers are torn down
    //      */
    // }    
    

    // Should this go elsewhere?
    // public void AddPlayerToScene()
    // {
    //     var scene = GD.Load<PackedScene>("res://scenes/PlayerCharacter.tscn");
    //     var inst = scene.Instantiate<PlayerCharacter>();
    //     GlobalVariables.Instance.ActiveMainScene.AddChild(inst);
    //     GlobalVariables.Instance.Character = inst;
    // }
}