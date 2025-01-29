using Godot;
using TESTCS.menus;

namespace TESTCS.managers;

/** For handling game setup when it first runs */
[GlobalClass]
public partial class GameSetupyPoos : Node
{
    public override void _Ready()
    {
        GlobalVariables.GameManager.LoadMainMenu();
    }

    // public override void _Process(double delta) {}
}