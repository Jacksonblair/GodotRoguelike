using Godot;
using System;
using System.Threading.Tasks;
using TESTCS.levels.transition_screens;

public partial class Transition1 : Transition
{
	private AnimationPlayer _player;
	
	public override void _Ready()
	{
		_player = GetNode<AnimationPlayer>("%AnimationPlayer");
	}

	public override async Task TransitionIn()
	{
		GD.Print("TRANSITIONING IN");
		_player.Play("FadeIn");
		await ToSignal(_player, "animation_finished");
	}

	public override async Task TransitionOut()
	{
		GD.Print("TRANSITIONING OUT");
		_player.Play("FadeOut");
		await ToSignal(_player, "animation_finished");
	}
}
