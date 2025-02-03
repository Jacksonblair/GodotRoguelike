// using System;
// using System.Linq;
// using Godot;
//
// public partial class ClosestEnemyGetter : Node2D
// {
//     private Node2D closestEnemy;
//     private Line2D _line;
//
//     [Export]
//     private float Range = 200;
//
//     public Node2D GetClosestEnemy()
//     {
//         UpdateClosestEnemy();
//         if (IsInstanceValid(closestEnemy))
//         {
//             return closestEnemy;
//         }
//         else
//         {
//             closestEnemy = null;
//             return null;
//         }
//     }
//
//     public override void _Process(double delta)
//     {
//         DebugDrawLineToClosestEnemy();
//     }
//
//     public override void _Ready()
//     {
//         // Initialize the Line2D node for drawing the line
//         _line = new Line2D();
//         AddChild(_line);
//         _line.DefaultColor = Colors.Red; // Set the color of the line
//         _line.Width = 2; // Set the width of the line
//     }
//
//     public void UpdateClosestEnemy()
//     {
//         // Find all nodes in the "Enemy" group
//         var enemies = GetTree().GetNodesInGroup("Enemy").Cast<GhostEnemy1>().ToList();
//
//         if (enemies.Count == 0)
//         {
//             // GD.Print("No enemies found.");
//             _line.ClearPoints(); // Clear the line if no enemies
//             return;
//         }
//
//         // GD.Print("Enemies in group: ", enemies.Count);
//         // GD.Print(enemies);
//
//         // Find the closest enemy by calculating distance
//         var _closestEnemy = enemies
//             .OrderBy(enemy => enemy.Position.DistanceTo(GlobalVariables.Instance._character.Position))
//             .FirstOrDefault();
//
//         if (_closestEnemy != null)
//         {
//             // Calculate the distance between the character and the closest enemy
//             float distanceToEnemy = GlobalPosition.DistanceTo(_closestEnemy.GlobalPosition);
//
//             // If the enemy is outside the specified range, don't draw the line
//             if (distanceToEnemy < Range)
//             {
//                 closestEnemy = _closestEnemy;
//             }
//             else
//             {
//                 closestEnemy = null;
//             }
//         }
//         else
//         {
//             closestEnemy = null;
//         }
//
//         // if (_closestEnemy != null)
//         // {
//         //     // GD.Print("Closest enemy: " + _closestEnemy.Name);
//         //     // Trigger your event here (e.g., attack, alert, etc.)
//         //     TriggerEvent();
//         // }
//     }
//
//     private void DebugDrawLineToClosestEnemy()
//     {
//         if (GetClosestEnemy() == null)
//         {
//             _line.ClearPoints();
//             return;
//         }
//
//         var camera = GetViewport().GetCamera2D();
//
//         // GD.Print(GlobalVariables.GetCharacter().GlobalPosition);
//         // GD.Print(_closestEnemy.GlobalPosition);
//
//         // Clear previous points from the Line2D node
//         _line.ClearPoints();
//
//         // Calculate points relative to the camera's position
//         // THE CAMERA EXPECTS POSITIONS IN VIEWPORT COORDINATES, NOT GLOBAL COORDINATES
//         Vector2 characterViewportPos = GlobalPosition - camera.GlobalPosition;
//         Vector2 enemyViewportPos = closestEnemy.GlobalPosition - camera.GlobalPosition;
//
//         // Add the transformed points to Line2D
//         _line.AddPoint(characterViewportPos);
//         _line.AddPoint(enemyViewportPos);
//     }
// }
