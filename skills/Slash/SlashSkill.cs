using Godot;
using TESTCS.actors;
using TESTCS.skills.Interfaces;

public partial class SlashSkill : PlayerSkill, IProjectileSkill
{
    public SlashSkillData SlashSkillData;
    private Area2D _hitbox;
    private int attackNum = 0;

    private bool _executing;
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
        if (_executing)
        {
            // GD.Print("INTERRUPTED ATTACK NUM: ", attackNum);
        }
        StopExecuting();
    }

    private void ExecuteAbility()
    {
        if (_executing) return;
        _executing = true;
        attackNum++;
        // GD.Print("STARTING ATTACK NUM: ", attackNum);
        
        GV.PlayerCharacter.AnimationManager.PlayAnimation(nameof(PlayerCharacterAnims.attack), true);
        var hitDir = (GV.PlayerCharacter.Controller.GetAimDirection(GV.PlayerCharacter.Position));
        _hitbox.Rotation = hitDir.Angle();
        
        GV.PlayerCharacter.Controller.DisableMovementInput();
        GV.PlayerCharacter.AnimationManager.Sprite.FrameChanged += OnFrameChanged;
    }

    private void StopExecuting()
    {
        if (!_executing) return;
        GV.PlayerCharacter.Controller.EnableMovementInput();
        GV.PlayerCharacter.AnimationManager.Sprite.FrameChanged -= OnFrameChanged;
        GV.PlayerCharacter.AnimationManager.PlayAnimation("idle");
        _executing = false;
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
            
            // GD.Print("ENDED ATTACK NUM: ", attackNum);
            StopExecuting();
        }
    }
    
}

