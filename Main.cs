using System;
using Godot;

public partial class Main : Node
{
    StoneLevel level;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StoneLevel childNode = GetNodeOrNull<StoneLevel>("StoneLevel");
        if (childNode != null)
        {
            this.level = childNode;
        }
        else
        {
            GD.Print("Child node not found.");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Check if the "A" key is pressed
        // if (Input.IsActionJustPressed("ui_right")) // "ui_left" is mapped to the arrow keys and "A" key by default
        // {
        //     GD.Print("SPAWN PROJ");
        //     var projectile = (PackedScene)GD.Load("res://scenes/projectiles/Projectile.tscn");
        //     GD.Print(projectile);
        //     var inst = projectile.Instantiate();
        //     GD.Print(inst.Name);
        //     level?.CallDeferred("add_child", inst);

        //     var pos = level.GetLocalMousePosition();
        //     var posx = level.MapToLocal(new Vector2I(10, 10));
        //     GD.Print(posx.ToString());
        //     GD.Print(level.LocalToMap(posx));

        //     ((Node2D)inst).Position += posx;
        // }
    }
}
