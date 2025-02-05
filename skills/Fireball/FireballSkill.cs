using Godot;
using TESTCS.actors;
using TESTCS.helpers;
using TESTCS.projectiles;
using TESTCS.skills;
using TESTCS.skills.Fireball;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class FireballSkill : Skill, IProjectileSkill
{
    public FireballSkillData SkillData;

    public override void _Process(double delta) {}

    public override void Execute(Actor executedBy, ModifierResults modifiers)
    {
        FireProjectile(executedBy, modifiers);
        // FireProjectileAtMouse(executedBy, modifiers);
    }

    public void FireProjectile(Actor executedBy, ModifierResults modifiers)
    {
        var target = MiscHelper.GetActiveMainSceneMousePosition().Value;
        var origin = executedBy.Position;
        
        var level = GlobalVariables.ActiveMainSceneContainer;
        Vector2 direction = (target - origin).Normalized();

        var finalNumProjectiles = SkillData.BaseProjectiles + modifiers.AdditionalProjectiles;
        var spreadPositions = ProjectileHelper.GetSpreadPositions(origin, direction, finalNumProjectiles, 20f);

        // PROOF OF CONCEPT FOR ALTERING SKILL BASED ON CHARGING
        var finalSpeed = SkillData.ProjectileSpeed;
        if (modifiers.CurrentChargeStage > 0)
        {
            finalSpeed += modifiers.CurrentChargeStage * 100;    
        }
        
        for (int i = 0; i < finalNumProjectiles; i++)
        {
            var projectile = GlobalVariables.GameManager.GameProjectiles.BaseProjectileScene.Instantiate<Projectile>();
            var position = spreadPositions[i];

            projectile.InitialDirection = direction;
            projectile.Speed = finalSpeed;
            projectile.Position = position;
            projectile.ProjectileAnimation = GlobalVariables.GameManager.GameProjectiles.FireballFrames;
            projectile.ProjectileCollisionAnimation = GlobalVariables.GameManager.GameProjectiles.Explosion1;
            projectile.Damage = SkillData.BaseDamage + modifiers.AdditionalFlatDamage;
            projectile.Weight = SkillData.BaseWeight;
            projectile.Lifetime = 3f;
            
            level.AddChild(projectile);
        }
    }
}