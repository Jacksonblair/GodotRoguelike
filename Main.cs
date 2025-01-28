using System;
using Godot;
using TESTCS.scripts.managers;

public partial class Main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (
        GlobalVariables.Instance.GameStateManager.LoadMostRecentGameState(1)
        )
        {
            GD.Print("CURRENT SCENE ID: ", GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID);
            GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID += 5;
            GD.Print("UPDATED SCENE ID: ", GlobalVariables.Instance.GameStateManager.GameState.CurrentSceneID);
            GlobalVariables.Instance.GameStateManager.SaveCurrentGameState(1);
        }
        else
        {
            GlobalVariables.Instance.GameStateManager.CreateNewGameSave(1);
        }

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}