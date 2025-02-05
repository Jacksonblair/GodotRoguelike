using Godot;
using TESTCS.actors.controllers;

namespace TESTCS.actors;

public partial class PlayerCharacter : Actor, IHittable
{
    private AnimatedSprite2D _sprite;
    public SkillChargingRing SkillChargingRing;
    private Camera2D _camera;
    private HealthBar _healthBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play();
        _camera = GetNode<Camera2D>("%Camera2D");
        SkillChargingRing = GetNode<SkillChargingRing>("SkillChargingRing");
        _healthBar = GetNode<HealthBar>("HealthBar");
        _healthBar.MaxValue = MaxHealth;
        _healthBar.SetValue(Health);
           
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
        if (IsAirborne)
        {
            Height += VerticalVelocity * (float)delta;
            VerticalVelocity -= GlobalVariables.Gravity * (float)delta;
            
            // Update sprites
            // TODO: HANDLE HEIGHT
            // Sprite.Position = new Vector2(Sprite.Position.X, -Height);
            
            // Check if the enemy "lands"
            if (Height <= 0)
            {
                Height = 0;
                IsAirborne = false;
                VerticalVelocity = 0;
                // OnLand();
            }
                        
            // Decay knockback horizontal velocity
            Velocity = Velocity.MoveToward(Vector2.Zero, 50f * (float)delta);
            
            // Move the enemy
            MoveAndSlide();
        }
        else
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
            
            Velocity = velocity;
            MoveAndSlide();
        }
        
        // var velocity = Controller.GetMovementInput(this.Position);
        // // var label = GetNode<Label>("Label");
        // // label.Text = Name;
        // Velocity = velocity;
        // MoveAndSlide();
    }
    
    
    public void ReceiveHit(HitInformation hitInformation)
    {
        Health -= hitInformation.Damage;
        _healthBar.MaxValue = MaxHealth;
        _healthBar.SetValue(Health);
        
        // Divide enemy weight by hit weight to get force of knockback 
        // 10/5 == 2
        // 50/5 == 10
        // 2/1 == 2
        float knockbackForce = hitInformation.Weight / this.Weight * 25;
        
        // Calculate knockback direction
        Vector2 knockbackDirection = (GlobalPosition - hitInformation.Position).Normalized();
        
        // Apply knockback force to Vector
        Vector2 knockbackVector = knockbackDirection * knockbackForce;
        
        // Calculate the vertical lift (based on force magnitude and a multiplier)
        float verticalLift = knockbackForce * 0.5f; // Lift multiplier

        // Apply knockback
        Velocity += knockbackVector;
        VerticalVelocity = verticalLift;
        IsAirborne = true;
        
        // TODO: MOVE SOMEWHERE ELSE
        
        if (Health <= 0)
        {
            GD.Print("YOU DIED");
            Position = GlobalVariables.ActiveMainSceneContainer.GlobalPosition;
            Health = MaxHealth;
        }
        
    }
    
}