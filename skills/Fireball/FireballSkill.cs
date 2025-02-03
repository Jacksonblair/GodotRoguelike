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
        
        var spreadPositions = ProjectileHelper.GetSpreadPositions(origin, direction, numProjectiles, 20f);
        
        for (int i = 0; i < numProjectiles; i++)
        {
            var projectile = GlobalVariables.GameManager.GameProjectiles.BaseProjectileScene.Instantiate<Projectile>();
            var position = spreadPositions[i];

            projectile.InitialDirection = direction;
            projectile.Speed = speed;
            projectile.Position = position;
            projectile.ProjectileData = projectileData;
            projectile.Lifetime = 10f;
            
            level.AddChild(projectile);
        }
    }
}