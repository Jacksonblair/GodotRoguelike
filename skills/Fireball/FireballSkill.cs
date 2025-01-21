using Godot;
using TESTCS.helpers;
using TESTCS.skills.Fireball;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class FireballSkill : Skill, IProjectileSkill
{
	public FireballSkillData SkillData;


	public override void Execute(ModifierResults modifiers)
	{
		var dmg = SkillData.BaseDamage + modifiers.AdditionalFlatDamage;
		var numProj = SkillData.BaseProjectiles + modifiers.AdditionalProjectiles;
		
		GD.Print("Executing fireball, with projectiles: ", numProj, " and Damage: ", dmg);
		
		// TODO: Use properties? 
		FireProjectileAtMouse();
	}

	public void FireProjectileAtClosestEnemy(int numProjectiles)
	{
		// If aiming, fire at mouse, otherwise, fire at closest enemy
		var closestEnemy = GlobalVariables.Instance.Character.closestEnemyGetter.GetClosestEnemy();
		if (closestEnemy != null)
		{
			// GD.Print("FIRE AT CLOSEST ENEMY");
			FireProjectile(closestEnemy.Position);
		}
		else
		{
			// GD.Print("NO CLOSEST ENEMY");
		}
	}

	public void FireProjectileAtMouse()
	{
		var pos = MiscHelper.GetActiveMainSceneMousePosition();
		if (pos.HasValue)
		{
			FireProjectile(pos.Value);
		}
	}

	public void FireProjectile(Vector2 target)
	{
		var level = GlobalVariables.Instance.ActiveMainSceneContainer;
		var projectile = (PackedScene)GD.Load("res://scenes/projectiles/Projectile.tscn");
		var inst2 = projectile.Instantiate<BasicProjectile>();

		Vector2 direction = (target - GlobalVariables.Instance.Character.Position).Normalized();
		inst2.Position = GlobalVariables.Instance.Character.Position;

		// GD.Print(inst2.Position);

		inst2.Init(new TESTCS.scenes.projectiles.LinearProjectileMover(), direction);
		level?.CallDeferred("add_child", inst2);

		// GD.Print(inst2.Position.ToString());
		// var mousePos = level.GetLocalMousePosition();
		// Vector2 direction = (mousePos - Position).Normalized();
		// ((Node2D)inst).Position += mousePos;
	}
}
