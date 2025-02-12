using Godot;
using TESTCS.actors;
using TESTCS.helpers;
using TESTCS.projectiles;
using TESTCS.skills;
using TESTCS.skills.Fireball;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class FireballSkill : PlayerSkill, IProjectileSkill
{
    public FireballSkillData FireballSkillData;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void Execute()
    {
        FireProjectile();
    }

    public override void Interrupt()
    {
        
    }

    public void FireProjectile()
    {
        var modifiers = SkillModifierHandler.SkillModifierResults;
        
        var target = MiscHelper.GetActiveMainSceneMousePosition().Value;
        var origin = GV.PlayerCharacter.GlobalPosition;
        
        var level = GV.ActiveMainSceneContainer;
        Vector2 direction = (target - origin).Normalized();

        var finalNumProjectiles = FireballSkillData.BaseProjectiles + modifiers.AdditionalProjectiles;
        var spreadPositions = ProjectileHelper.GetSpreadPositions(origin, direction, finalNumProjectiles, 20f);

        // PROOF OF CONCEPT FOR ALTERING SKILL BASED ON CHARGING
        var finalSpeed = FireballSkillData.ProjectileSpeed;
        if (modifiers.CurrentChargeStage > 0)
        {
            finalSpeed += modifiers.CurrentChargeStage * 100;    
        }
        
        for (int i = 0; i < finalNumProjectiles; i++)
        {
            var projectile = GV.GameManager.GameProjectiles.BaseProjectileScene.Instantiate<Projectile>();
            var position = spreadPositions[i];

            projectile.InitialDirection = direction;
            projectile.Speed = finalSpeed;
            projectile.Position = position;
            projectile.ProjectileAnimation = GV.GameManager.GameProjectiles.FireballFrames;
            projectile.ProjectileCollisionAnimation = GV.GameManager.GameProjectiles.Explosion1;
            projectile.Damage = FireballSkillData.BaseDamage + modifiers.AdditionalFlatDamage;
            projectile.Weight = FireballSkillData.BaseWeight;
            projectile.Lifetime = 3f;
            
            level.AddChild(projectile);
        }
    }
}