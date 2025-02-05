using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	private Label HealthLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HealthLabel = GetNode<Label>("Label");
		ValueChanged += OnValueChanged;
	}

	private void OnValueChanged(double value)
	{
		HealthLabel.Text = value.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta) {}
}
