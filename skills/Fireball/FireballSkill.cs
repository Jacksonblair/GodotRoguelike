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
        FireProjectileAtMouse(executedBy, modifiers);
    }

    public void FireProjectileAtMouse(Actor executedBy, ModifierResults modifiers)
    {
        // todo: is this overkill.
        var pos = MiscHelper.GetActiveMainSceneMousePosition();
        if (pos.HasValue)
        {
            // TODO: NEED ORIGIN OF PROJECTILE
            FireProjectile(3, 200f, executedBy.Position, pos.Value, modifiers);
        }
    }
    
    public void FireProjectile(int numProjectiles, float speed, Vector2 origin, Vector2 target, ModifierResults modifiers)
    {
        var level = GlobalVariables.ActiveMainSceneContainer;
        var projectileData = GlobalVariables.GameManager.GameProjectiles.FireballProjectileData;
        Vector2 direction = (target - origin).Normalized();

        var finalNumProjectiles = numProjectiles + modifiers.AdditionalProjectiles;
        var spreadPositions = ProjectileHelper.GetSpreadPositions(origin, direction, finalNumProjectiles, 20f);

        // PROOF OF CONCEPT FOR ALTERING SKILL BASED ON CHARGING
        var finalSpeed = speed;
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
            projectile.ProjectileData = projectileData;
            projectile.Lifetime = 10f;
            
            level.AddChild(projectile);
        }
    }
}