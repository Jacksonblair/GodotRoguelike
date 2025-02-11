using Godot;
using System;

public partial class LevelDoor : Area2D
{
	[Export] private LevelsEnum goesToScene { get; set; }
	[Export] public int DoorId { get; set; }
	[Export] public int TargetDoorId { get; set; }
	
	private bool _ignoreBodyEntered = false;
	private Label _debugLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEnteredDoor;
		BodyExited += OnBodyExitedDoor;
		_debugLabel = GetNode<Label>("Label");
		UpdateDebugLabel();
	}

	private void OnBodyEnteredDoor(Node2D body)
	{
		if (goesToScene == LevelsEnum.NONE)
		{
			GD.PrintErr("No target scene assigned for door. Entering it wont do anything");
			return;
		}
		
		if (_ignoreBodyEntered) return;
		GV.GameManager.LoadLevel(goesToScene, TargetDoorId);
    }
	
	private void OnBodyExitedDoor(Node2D body)
	{
		_ignoreBodyEntered = false;
		UpdateDebugLabel();
	}

	public void DeactivateDoorUntilReentry()
	{
		_ignoreBodyEntered = true;
		UpdateDebugLabel();
	}

	private void UpdateDebugLabel()
	{
		_debugLabel.Text = $"Door: {DoorId} /n Target: {TargetDoorId} /n Active?: {!_ignoreBodyEntered}";
	}
}
