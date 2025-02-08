using System;
using DialogueManagerRuntime;
using Godot;
using TESTCS.actors.controllers;
using TESTCS.levels;

public partial class StoneLevel : Level
{
    private Area2D _preDoorCollider;
    private Camera2D _cutsceneCamera;
    private AnimationPlayer _animationPlayer;
    private Level1Enemy _enemy1;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _preDoorCollider = GetNode<Area2D>("PreDoorCollider");
        _preDoorCollider.BodyEntered += OnBodyEnteredPreDoorCollider;
        _cutsceneCamera = GetNode<Camera2D>("CutsceneCamera2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _enemy1 = GetNode<Level1Enemy>("Level1Enemy");
    }

    // When player enters collision area just prior to exit door
    private async void OnBodyEnteredPreDoorCollider(Node2D body)
    {
        _cutsceneCamera.MakeCurrent();
        
        // Disable player input
        GlobalVariables.PlayerCharacter.Controller = new PassiveEnemyController();
        
        // Move camera
        _animationPlayer.Play("TryExitEarly");
        await ToSignal(_animationPlayer, "animation_finished");

        var path = GetNode<Path2D>("Path2D");
        _enemy1.Controller.FollowPath(path);
        // var movePoint1 = GetNode<Node2D>("MovePoint1");
        // _enemy1.Controller.MoveToTarget(movePoint1.Position);
        // await ToSignal(_enemy1.Controller, "ReachedTarget");
        // var movePoint2 = GetNode<Node2D>("MovePoint2");
        // _enemy1.Controller.MoveToTarget(movePoint2.Position);
        // await ToSignal(_enemy1.Controller, "ReachedTarget");
        
        // Then start triggering dialogue
        DialogueManager.ShowDialogueBalloon(GD.Load<Resource>("res://dialogue/dialogue1.dialogue"));
        DialogueManager.DialogueEnded += (res) => {
            GlobalVariables.PlayerCharacter.Controller = new PlayerController();
            GlobalVariables.PlayerCharacter.Camera.MakeCurrent();
        };
        
        /**
         * Could i...
         * - Use a particular controller for the enemy, that moves it ceaselessly towards a direction.
         *
         * Downsides of above?
         * - Might have to rely on 
         *
         */
        
        // TODO:
        // - Somewhere in here, skeletons need to walk up to the player. 
        // - Then, make one of them hostile. 
        // - Play some dialogue
        // - Make the next one hostile
        // - Play some dialogue
        // - Make the next one hostile
    }

    private void OnBodyEnteredExit(Node2D body)
    {}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
