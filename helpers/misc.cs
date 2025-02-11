#nullable enable
using Godot;

namespace TESTCS.helpers;

public static class MiscHelper
{
    public static Vector2? GetActiveMainSceneMousePosition()
    {
        var level = GV.Instance._activeMainSceneContainer;
        var mousePos = level.GetLocalMousePosition();
        return mousePos;
    }

    // public static Node2D? GetClosestEnemy()
    // {
    //     return GlobalVariables.Instance._character.closestEnemyGetter.GetClosestEnemy();
    // }
}