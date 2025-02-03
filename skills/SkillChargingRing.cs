using Godot;
using System;
using TESTCS.skills;

public partial class SkillChargingRing : Node2D
{
	public SkillChargingManager SkillChargingManager;
	private TextureProgressBar _progressBar;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		_progressBar = GetNode<TextureProgressBar>("TextureProgressBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (SkillChargingManager == null)
		{
			_progressBar.Visible = false;
			return;
		};
		
		// TODO: Make this function less crappy you legend
		// GD.Print(SkillChargingManager.ChargedFor);
		// GD.Print(SkillChargingManager.NextStageAt);

		_progressBar.Value = SkillChargingManager.PercentageStateCharged;
		_progressBar.MaxValue = 1;
		_progressBar.Visible = true;
	}
}
