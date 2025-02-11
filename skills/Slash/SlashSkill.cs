using Godot;
using TESTCS.actors;
using TESTCS.skills.Interfaces;

public partial class SlashSkill : PlayerSkill, IProjectileSkill
{
    public SlashSkillData SlashSkillData;
    private Area2D _hitbox;

    public override void _Ready()
    {
        base._Ready();
        _hitbox = GetNode<Area2D>("Hitbox");
    }
    
    public override void Execute()
    {
        ExecuteAbility();
    }

    private async void ExecuteAbility()
    {
        /**
         * Play animation
         * Listen for event where the halfway point is reached
         * If its reached, execute the hit 
         * If its not, dont execute the hit
         */
        
        // Direction is... vector between my position and the aim direction
        var aimDir = GV.PlayerCharacter.GetAimDirection();
        var animName = nameof(PlayerCharacterAnims.attack);

        if (!GV.PlayerCharacter.MainSprite.SpriteFrames.HasAnimation(animName))
        {
            GD.PrintErr("Player does not have animation: ", animName);
            return;
        }
        
        GV.PlayerCharacter.IsAttacking = true;
        GV.PlayerCharacter.MainSprite.Play(animName);
        GV.PlayerCharacter.MainSprite.FrameChanged += CheckIfFrameToPlayDamage;
        _hitbox.SetRotation(aimDir.Angle());

    }

    private void CheckIfFrameToPlayDamage()
    {
        if (GV.PlayerCharacter.MainSprite.Frame == 2)
        {
            GD.Print("FRAME 2. APPLY DMG");
            
            var enemies = _hitbox.GetOverlappingBodies();
            foreach (var area2D in enemies)
            {
                if (area2D is IHittable hittable)
                {
                    hittable.ReceiveHit(new HitInformation(10, 100, GV.PlayerCharacter.Position));
                }
            }
            
            GV.PlayerCharacter.IsAttacking = false;
        }
    }
}

