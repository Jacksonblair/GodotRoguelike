using System.Collections.Generic;
using Godot;
using TESTCS.enums;
using TESTCS.helpers;
using TESTCS.managers;

public partial class PlayerCharacter : CharacterBody2D
{
    float speed = 200;
    string current_terrain;
    
    CollisionShape2D collision;
    AnimatedSprite2D sprite;
    public ClosestEnemyGetter closestEnemyGetter;
    Timer getEnemyTimer;
    Area2D NPCArea2D;

    // Nearby NPC
    IInteractable nearbyNPC;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalVariables.Instance.Character = this;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play();
        closestEnemyGetter = GetNode<ClosestEnemyGetter>("ClosestEnemyGetter");
        
        // Set up basic default skill
        
        // Automatically use skill
        // getEnemyTimer = GetNode<Timer>("GetEnemyTimer");
        // getEnemyTimer.Timeout += UseAutoSkill;
        
        NPCArea2D = GetNode<Area2D>("NPCArea2D");
        NPCArea2D.AreaEntered += onNPCAreaEntered;
        NPCArea2D.AreaExited += onNPCAreaExited;
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
        Vector2 velocity = Vector2.Zero;

        // Get input and adjust velocity.
        // TODO: Move to handler
        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }
        
        if (Input.IsActionJustPressed(EnumHelper.GetEnumName(PlayerInputs.Skill1)))
        {
            GD.Print("Execute skill 1");
            GlobalVariables.Instance.SkillManager.ActivateSkill(0);
        }
        
        if (Input.IsActionJustPressed(EnumHelper.GetEnumName(PlayerInputs.Skill2)))
        {
            GD.Print("Execute skill 2");
            GlobalVariables.Instance.SkillManager.ActivateSkill(1);
            // GlobalVariables.Instance.SkillManager.ExecuteSkill(SkillSlotsEnum.Skill2);
        }

        if (Input.IsActionJustPressed(EnumHelper.GetEnumName(PlayerInputs.Skill3)))
        {
            // GlobalVariables.Instance.SkillManager.ExecuteSkill(SkillSlotsEnum.Skill3);
        }
        
        if (Input.IsActionJustPressed("interact"))
        {
            nearbyNPC?.Interact();
        }

        // Seperate parts
        // - The ui 
        // - The actual ability
        
        /**
            If a character's input or other factors affect velocity, normalizing it and applying speed means the character’s movement remains consistent regardless of input strength.
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

            velocity = velocity.Normalized() * speed;
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

}
