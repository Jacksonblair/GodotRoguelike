using Godot;
using System.Collections.Generic;
using TESTCS.helpers;
using TESTCS.skills.Interfaces;
using TESTCS.skills.Modifiers;

public partial class FireballSkill : Skill, IProjectileSkill
{
	public int Damage { get; set; }
	public int BaseProjectiles { get; set; }
	public int ExtraProjectiles { get; set; }
	public int ExtraDamage { get; set; }
	
	/**
	 * LocalModifers and GlobalModifiers
	 * When skill is executed, all modifiers are evaluated
	 * Modifers are evaluated according to FireballSkill, and are applied to local variables
	 *
	 *
	 * MODIFIERS AS RESOURCES NOT A GOOD IDEA APPARENTLY.
	 *
	 * I was really enjoying thew idea of being able to set their values individually. I guess it doesnt make sense now.
	 *
	 * I think i can expose the base values of the skill (Fireball, Iceball), and the modifiers just need to be a class, objects of which i can add/remove
	 */

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{}

	public override void _Ready()
	{
		// Test modifiers to add
		// TODO: Remove this.
		LocalModifiers.Add(new ExtraProjectileModifier(1));
		LocalModifiers.Add(new ExtraProjectileModifier(2));
		LocalModifiers.Add(new FlatDamageModifier(20));
	}
	
	public override void ApplyModifiers()
	{
		// TODO: Move these functions into a generic handler
		
		// Handle local skill modifiers
		HandleModifiers(LocalModifiers);
		
		// Handle global skill modifiers
		HandleModifiers(GlobalVariables.Instance.SkillManager.GlobalModifiers);
		
		GD.Print("Final numb projectiles", ExtraProjectiles + BaseProjectiles);
		GD.Print("Final damage", ExtraDamage + ExtraDamage);
	}
	
	// Specific method for handling how modifiers are applied to this particular skill. 
	private void HandleModifiers(List<SkillModifier> modifiers)
	{
		var extraProjectiles = 0;
		var extraDamage = 0;
		
		// Handle all modifiers.		
		foreach (var modifier in modifiers)
		{
			switch (modifier)
			{
				case ExtraProjectileModifier projModifier:
					extraProjectiles = projModifier.ExtraProjectiles;
					break;
				case FlatDamageModifier dmgModifier:
					extraDamage = dmgModifier.AdditionalDamage;
					break;
			}
		}
		
		ExtraProjectiles += extraProjectiles;
		ExtraDamage += extraDamage;
	}

	public override void Execute()
	{
		ApplyModifiers();
		// FireProjectileAtClosestEnemy(ExtraProjectiles + BaseProjectiles);
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
