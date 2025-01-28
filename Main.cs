using System;
using Godot;
using TESTCS.scripts.managers;

public partial class Main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalVariables.Instance.GameStateManager.LoadGameState();
        GD.Print("CURRENT SCENE ID: ", GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID);
        GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID += 5;
        GD.Print("UPDATED SCENE ID: ", GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID);
        GlobalVariables.Instance.GameStateManager.SaveGameState();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}