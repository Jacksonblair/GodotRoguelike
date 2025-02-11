using Godot;
using TESTCS.levels.transition_screens;

namespace TESTCS.scripts.managers;

/** Specifically for loading/unloading scenes and handling transitions */
public partial class GameSceneManager : Node
{
    public Node CurrentActiveScene;
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
                GV.Instance._activeMainSceneContainer.RemoveChild(CurrentTransitionScene);
            }
        
            scene.SetVisible(false);
            
            // Instantiate specified scene
            GV.Instance._activeMainSceneContainer.AddChild(scene);
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
        GV.Instance._activeMainSceneContainer.AddChild(inst);
        
        // Update internal reference
        CurrentActiveScene = inst;
        
        // GD.Print("Loading: ", gameScene);
    }

    public void UnloadCurrentGameScene()
    {
        // Remove previous scene
        if (CurrentActiveScene != null)
        {
            GV.Instance._activeMainSceneContainer.RemoveChild(CurrentActiveScene);
        }
    }
    
    public void UnloadCurrentTransitionScene()
    {
        // Remove previous scene
        if (CurrentTransitionScene != null)
        {
            GV.Instance._activeMainSceneContainer.RemoveChild(CurrentTransitionScene);
        }
    }
}