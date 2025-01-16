using Godot;
using System;

[GlobalClass]
public partial class test_resource : Resource
{
    [Export]
    public PackedScene Scene { get; set; }
}
