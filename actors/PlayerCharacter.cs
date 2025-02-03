using Godot;
using TESTCS.actors.controllers;

namespace TESTCS.actors;

public partial class PlayerCharacter : Actor, IHittable
{
    private AnimatedSprite2D _sprite;
    public SkillChargingRing SkillChargingRing;
    private Camera2D _camera;

    // CollisionShape2D collision;
    // public ClosestEnemyGetter closestEnemyGetter;
    // Area2D NPCArea2D;
    // Nearby NPC
    // IInteractable nearbyNPC;

    // public override void _EnterTree()
    // {
    //     SetMultiplayerAuthority(Name.ToString().ToInt());
    // }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play();
        _camera = GetNode<Camera2D>("%Camera2D");
        SkillChargingRing = GetNode<SkillChargingRing>("SkillChargingRing");

        // GlobalVariables.Instance._character = this;
        // closestEnemyGetter = GetNode<ClosestEnemyGetter>("ClosestEnemyGetter");
        // NPCArea2D = GetNode<Area2D>("NPCArea2D");
        // NPCArea2D.AreaEntered += onNPCAreaEntered;
        // NPCArea2D.AreaExited += onNPCAreaExited;
        Controller = new PlayerController();
        Controller.AbilityPressed += OnAbilityPressed;
        Controller.Interacted += OnInteracted;
    }

    private void OnInteracted()
    {
        // nearbyNPC?.Interact();
    }

    private void OnAbilityPressed(int abilityindex)
    {
        // TODO: HANDLE ABILITY PRESSY POOS
    }

    // private void onNPCAreaExited(Area2D area)
    // {
    //     nearbyNPC = null;
    // }
    //
    // private void onNPCAreaEntered(Node2D body)
    // {
    //     nearbyNPC = (IInteractable)body;
    // }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var velocity = Controller.GetMovementInput(this.Position);
        if (velocity.Length() > 0)
        {
            _sprite.Animation = "run";

            // Update sprite based on velocity
            if (velocity.X != 0)
            {
                _sprite.FlipH = velocity.X < 0;
            }

            velocity = velocity.Normalized() * MovementSpeed;
        }
        else
        {
            _sprite.Animation = "idle";
        }

        // var label = GetNode<Label>("Label");
        // label.Text = Name;
        Velocity = velocity;
        MoveAndSlide();
    }

    public void ReceiveHit(HitInformation hitInformation)
    {
        Health -= hitInformation.Damage;
        if (Health > 0) return;
        GD.Print("PLAYED DIEDY POOS");
        QueueFree();
    }
}