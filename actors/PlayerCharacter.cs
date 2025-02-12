using Godot;
using TESTCS.actors.controllers;
using TESTCS.managers.PlayerManagers;

namespace TESTCS.actors;

public enum PlayerCharacterAnims
{
    attack,
    block,
    idle,
    run
}

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
 */

public partial class PlayerCharacter : Actor, IHittable
{
    public AnimatedSprite2D MainSprite;
    public SkillChargingRing SkillChargingRing;
    public Camera2D Camera;
    private HealthBar _healthBar;
    private Sprite2D _blockIndicator;
    private bool _isBlocking;
    private CpuParticles2D _dragParticles;
    public SpriteAnimationManager AnimationManager;
    
    public bool DontProcessInput;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MainSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        MainSprite.Play();
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

        AnimationManager = new SpriteAnimationManager(MainSprite);
        
        // Delete contents of 'Skills' child node. DONT DELETE THIS.
        var SkillsNode = GetNode<Node>("Skills");
        foreach (Node child in SkillsNode.GetChildren())
        {
            SkillsNode.RemoveChild(child);
            child.QueueFree();
        }
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

        /**
         * If im attacking, i can either be attacking in a static position or moving.
         * In this case, i set the animation to like:
         * anim = attack, dontMove = true
         *
         * What if i implement an interrupt, so that my attack gets stopped. The anim needs to get reset.
         * 
         */

        if (DontProcessInput)
        {
            // Dont process input;
            MoveAndSlide();
            return;
        }
        
        // TODO: Update aim direction indicator
        if (_isBlocking)
        {
            _blockIndicator.Rotation = Controller.GetAimDirection(this.Position).Angle();
        }
        
        if (IsKnockedBack)
        {
            _dragParticles.SetEmitting(true);
            Height += VerticalVelocity * (float)delta;
            // VerticalVelocity -= GV.Gravity * (float)delta;
            
            // Update sprites
            // TODO: HANDLE HEIGHT
            // Sprite.Position = new Vector2(Sprite.Position.X, -Height);
            
            // Check if the enemy "lands"
            if (Height <= 0)
            {
                Height = 0;
                IsKnockedBack = false;
                // VerticalVelocity = 0;
                OnLand();
            }
                        
            // Decay knockback horizontal velocity
            Velocity = Velocity.MoveToward(Vector2.Zero, 50f * (float)delta);
            
            MoveAndSlide();
        }
        else
        {
            var velocity = Controller.GetMovementInput(this.Position);
    
            if (velocity.Length() > 0)
            {
                AnimationManager.PlayAnimation(nameof(PlayerCharacterAnims.run));
                // MainSprite.Animation = "run";

                // Update sprite based on velocity
                if (velocity.X != 0)
                {
                    MainSprite.FlipH = velocity.X < 0;
                }

                velocity = velocity.Normalized() * MovementSpeed;
            }
            else
            {
                AnimationManager.PlayAnimation(nameof(PlayerCharacterAnims.idle));
            }
            
            Velocity = velocity;
            MoveAndSlide();
        }
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
        Vector2 knockbackDirection = (GlobalPosition - hitInformation.PositionOfHit).Normalized();
        
        // Apply knockback force to Vector
        Vector2 knockbackVector = knockbackDirection * knockbackForce;
        
        // Calculate the vertical lift (based on force magnitude and a multiplier)
        // float verticalLift = knockbackForce * 0.5f; // Lift multiplier

        // Apply knockback
        Velocity += knockbackVector;
        // VerticalVelocity = verticalLift;
        IsKnockedBack = true;
        
        // TODO: MOVE SOMEWHERE ELSE
        
        AnimationManager.InterruptAnimation();
        
        if (Health <= 0)
        {
            GD.Print("YOU DIED");
            Position = GV.ActiveMainSceneContainer.GlobalPosition;
            Health = MaxHealth;
        }
        
    }

    public Vector2 GetAimDirection()
    {
        return Controller.GetAimDirection(this.Position);
    }
}