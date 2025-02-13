using System.Security.Cryptography.X509Certificates;
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
    private CpuParticles2D _dragParticles;
    
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
        IsBlocking = false;
    }

    private void OnStartedBlocking()
    {
        IsBlocking = true;
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
        if (IsBlocking)
        {
            _blockIndicator.Rotation = Controller.GetAimDirection(this.Position).Angle();
        }
        
        if (IsKnockedBack)
        {
            _dragParticles.SetEmitting(true);
            KnockbackHandler.ApplyKnockbackFriction(delta, this);

            if (!IsKnockedBack)
            {
                _dragParticles.SetEmitting(false);
            }
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
        }
        
        MoveAndSlide();
    }

    private void OnLand()
    {
        _dragParticles.SetEmitting(false);
    }


    public void ReceiveHit(HitInformation hitInformation)
    {
        KnockbackHandler.ApplyKnockbackForce(hitInformation, this);

        // Interrupt current anim when knocked back
        if (IsKnockedBack)
        {
            // TODO: Play knockback animation
            AnimationManager.InterruptAnimation();
        }
        
        ProcessHitDamage(hitInformation);
        
        // Update healthbar
        _healthBar.MaxValue = MaxHealth;
        _healthBar.SetValue(Health);
        
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