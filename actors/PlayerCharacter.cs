using Godot;
using TESTCS.actors.controllers;

namespace TESTCS.actors;


/**
 * - 'Equip' means:
 *      - Add skill as child of player
 *      - Skill should include its hurtbox, and so on.
 *      - Skill should include its cooldown management, charging management, and so on?
 *
 * What about animations?
 * - I think player skills should be coupled with the player. So they know exactly what animations i have.
 *
 * So slash:
 *      - When I execute it, I need to tell the player to play the 'slash' animation.
 *      - So that means passing a reference to the player to the Slash skill class
 *          - ANd calling player.plauSlash() or whatever
 *
 * Fireball?    
 *      - When I execute it, spawn a projectile at the players location and send it off.
 *
 *
 * 
 * 
 */

public partial class PlayerCharacter : Actor, IHittable
{
    private AnimatedSprite2D _sprite;
    public SkillChargingRing SkillChargingRing;
    public Camera2D Camera;
    private HealthBar _healthBar;
    private Sprite2D _blockIndicator;
    private bool _isBlocking;
    private CpuParticles2D _dragParticles;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play();
        Camera = GetNode<Camera2D>("%Camera2D");
        SkillChargingRing = GetNode<SkillChargingRing>("SkillChargingRing");
        _healthBar = GetNode<HealthBar>("HealthBar");
        _healthBar.MaxValue = MaxHealth;
        _healthBar.SetValue(Health);
        _blockIndicator = GetNode<Sprite2D>("BlockIndicator");
        _dragParticles = GetNode<CpuParticles2D>("DragParticles");

        Controller = new PlayerController();
        Controller.AbilityPressed += OnAbilityPressed;
        Controller.Interacted += OnInteracted;
        Controller.StartedBlocking += OnStartedBlocking;
        Controller.StoppedBlocking += OnStoppedBlocking;
    }

    private void OnStoppedBlocking()
    {
        GD.Print("STOPPED BLOCKING");
        _blockIndicator.Hide();
        _isBlocking = false;
    }

    private void OnStartedBlocking()
    {
        _isBlocking = true;
        _blockIndicator.Show();
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
        Controller.Update(delta);

        // TODO: Update aim direction indicator
        if (_isBlocking)
        {
            _blockIndicator.Rotation = Controller.GetAimDirection(this.Position).Angle();
        }
        
        if (IsAirborne)
        {
            _dragParticles.SetEmitting(true);
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
                OnLand();
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

    private void OnLand()
    {
        _dragParticles.SetEmitting(false);
    }


    public void ReceiveHit(HitInformation hitInformation)
    {
        var blockCushion = 1f;
        const float blockCushionMultiplier = 0.25f;
        if (_isBlocking)
        {
            blockCushion = blockCushionMultiplier;
        }
        
        Health -= (int)((float)hitInformation.Damage * blockCushion);
        _healthBar.MaxValue = MaxHealth;
        _healthBar.SetValue(Health);
        
        // Divide enemy weight by hit weight to get force of knockback 
        // 10/5 == 2
        // 50/5 == 10
        // 2/1 == 2
        float knockbackForce = (hitInformation.Weight / this.Weight * 25) * blockCushion;
        
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