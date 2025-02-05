using Godot;
using Godot.Collections;
using TESTCS.levels.transition_screens;

namespace TESTCS.scripts.managers;

/** Specifically for loading/unloading scenes and handling transitions */
public partial class GameSceneManager : Node
{
    private Node _currentActiveScene;
    public Transition CurrentTransitionScene;
    
    // public override void _Ready() {}

    public void LoadTransitionScene(PackedScene transitionScene)
    {
        var inst = transitionScene.Instantiate();

        if (inst is Transition scene)
        {
            // Remove previous scene
            if (CurrentTransitionScene != null)
            {
                GlobalVariables.Instance._activeMainSceneContainer.RemoveChild(CurrentTransitionScene);
            }
        
            scene.SetVisible(false);
            
            // Instantiate specified scene
            GlobalVariables.Instance._activeMainSceneContainer.AddChild(scene);
            CurrentTransitionScene = scene;
            GD.Print("ADDED TRANSITION SCENE TO GAME");
        }
        else
        {
            inst.QueueFree();
            GD.PrintErr("Invalid transition scene. Cannot load it");   
        }
    }

    public void LoadGameScene(PackedScene gameScene)
    {
        // TODO: VALIDATE LEVEL
        UnloadCurrentGameScene();
        
        // Instantiate specified scene
        var inst = gameScene.Instantiate();
        GlobalVariables.Instance._activeMainSceneContainer.AddChild(inst);
        
        // Update internal reference
        _currentActiveScene = inst;
        
        // GD.Print("Loading: ", gameScene);
    }

    public void UnloadCurrentGameScene()
    {
        // Remove previous scene
        if (_currentActiveScene != null)
        {
            GlobalVariables.Instance._activeMainSceneContainer.RemoveChild(_currentActiveScene);
        }
    }
    
    public void UnloadCurrentTransitionScene()
    {
        // Remove previous scene
        if (CurrentTransitionScene != null)
        {
            GlobalVariables.Instance._activeMainSceneContainer.RemoveChild(CurrentTransitionScene);
        }
    }
}