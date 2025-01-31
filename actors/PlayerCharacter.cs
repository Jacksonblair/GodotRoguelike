using Godot;
using TESTCS.actors;
using TESTCS.actors.controllers;

public partial class PlayerCharacter : Actor, IHittable
{
    string current_terrain;
    CollisionShape2D collision;
    AnimatedSprite2D sprite;
    public ClosestEnemyGetter closestEnemyGetter;
    Timer getEnemyTimer;
    Area2D NPCArea2D;
    public SkillChargingRing SkillChargingRing;

    // Nearby NPC
    IInteractable nearbyNPC;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalVariables.Instance._character = this;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play();
        closestEnemyGetter = GetNode<ClosestEnemyGetter>("ClosestEnemyGetter");
        
        NPCArea2D = GetNode<Area2D>("NPCArea2D");
        NPCArea2D.AreaEntered += onNPCAreaEntered;
        NPCArea2D.AreaExited += onNPCAreaExited;
        SkillChargingRing = GetNode<SkillChargingRing>("SkillChargingRing");

        Controller = new PlayerController();
        
        Controller.AbilityPressed += OnAbilityPressed;
        Controller.Interacted += OnInteracted;
    }

    private void OnInteracted()
    {
        nearbyNPC?.Interact();
    }

    private void OnAbilityPressed(int abilityindex)
    {
        // TODO: HANDLE ABILITY PRESSY POOS
    }

    private void onNPCAreaExited(Area2D area)
    {
        nearbyNPC = null;
    }

    private void onNPCAreaEntered(Node2D body)
    {
        nearbyNPC = (IInteractable)body;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var velocity = Controller.GetMovementInput(this.Position);
        
        /**
            If a character's input or other factors affect velocity: 
            normalizing it and applying speed means the character’s movement remains consistent regardless of input strength.
            So, even if velocity varies, the actual speed won’t change—only the direction will.
        */
        if (velocity.Length() > 0)
        {
            sprite.Animation = "run";

            // Update sprite based on velocity
            if (velocity.X != 0)
            {
                sprite.FlipH = velocity.X < 0;
            }

            velocity = velocity.Normalized() * MovementSpeed;
        }
        else
        {
            sprite.Animation = "idle";
        }

        var label = GetNode<Label>("Label");
        label.Text = Position.ToString();

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