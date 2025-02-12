using Godot;
using TESTCS.actors;
using TESTCS.helpers.Commands;
using TESTCS.skills.Interfaces;

public interface IAnimationCommand
{
    void Execute();
}

public partial class SlashSkill : PlayerSkill, IProjectileSkill
{
    public SlashSkillData SlashSkillData;
    private Area2D _hitbox;
    // private Commands _commands = new();
    
    public override void _Ready()
    {
        base._Ready();
        _hitbox = GetNode<Area2D>("Hitbox");
    }
    
    public override void Execute()
    {
        ExecuteAbility();
    }

    public override void Interrupt()
    {
    }

    /**
     * REQUIREMENTS:
     *
     * If im attacking, what situations do i want the animation to be able to change?
     * - Interrupted (stunned, knocked back, etc)
     * - Cancelled (dive out of the way, etc)
     * - Attack animation is finished
     *
     * So i need to set the current animation, and not allow it to change except via the above, until its finished.
     * So i need to wrap the animations in some manager to prevent conflicts.
     *
     * If im attacking, how do i want manage to whether or not the player can move?
     * - Can i say, im doing a particular attack, i dont any input to come from the controller for now.
     * - Or can i say, im doing this particular attack, but movement input is still welcome until its done/interrupted/cancelled
     *
     *
     * StrikeSkill.Execute();
     * Player.SetAnimation(Attack, lock = true)
     * Controller.LockInput()
     *
     * BUT if this gets interrupted, i need to unlock input and... unlock the animation...
     * So the state imposed by using this skill needs to be reversible.
     * And it needs to tie into interruptions/cancellations.
     * AND i need to make sure that when the skill ends, it undoes the locks, etc.
     *
     *
     * So:
     * - IF interrupted/cancelled
     *      - Cancel() SkillEffect... SkillState...? ...TemporaryEffect?... NFI... applied to player
     *      - OR just set up skill to reverse these effects.
     *
     * When i strike, i dont want the player to be able to move. The strike should go..
     * .. In the direction they are facing, and lock input until after its done.
     *
     */

    
    private void ExecuteAbility()
    {
        GV.PlayerCharacter.AnimationManager.PlayAnimation(nameof(PlayerCharacterAnims.attack), true);

        var hitDir = (GV.PlayerCharacter.Controller.GetAimDirection(GV.PlayerCharacter.Position));
        _hitbox.Rotation = hitDir.Angle();
        
        GV.PlayerCharacter.Controller.DisableMovementInput();
        GV.PlayerCharacter.AnimationManager.AnimationInterruptedOrEnded += HandleInterruptOrEnd;
        GV.PlayerCharacter.AnimationManager.Sprite.FrameChanged += OnFrameChanged;
    }

    private void HandleInterruptOrEnd()
    {
        GV.PlayerCharacter.Controller.EnableMovementInput();
        GV.PlayerCharacter.AnimationManager.AnimationInterruptedOrEnded -= HandleInterruptOrEnd;
        GV.PlayerCharacter.AnimationManager.Sprite.FrameChanged -= OnFrameChanged;
    }

    private void OnFrameChanged()
    {
        if (GV.PlayerCharacter.AnimationManager.Sprite.Frame == 2)
        {
            var enemies = _hitbox.GetOverlappingBodies();
            var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            animPlayer.Play("flashhitbox");
            
            foreach (var area2D in enemies)
            {
                if (area2D is IHittable hittable)
                {
                    hittable.ReceiveHit(new HitInformation(10, 100, GV.PlayerCharacter.Position));
                }
            }
        }
    }
    
}

