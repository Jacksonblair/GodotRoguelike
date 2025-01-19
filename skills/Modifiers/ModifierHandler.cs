using System.Collections.Generic;
using Godot;

namespace TESTCS.skills.Modifiers;

public class ModifierResults
{
    public int AdditionalProjectiles = 0;
    public int AdditionalFlatDamage = 0;
    public int AdditionalCharges = 0;
    public int CooldownReduction = 0;

    public ModifierResults() {}
}

/**
 * Requirement: Avoid having to apply modifiers to each skill individually, if i can help uit
 * Takes in a list of modifiers, evaluates them and then returns an object with final values.
 *
 * Ex. FireballSkill ModifierHandler takes all the modifiers in, then
 * Returns { proj: 3, damage: 10, size: 20 } etc
 *
 * Ex. WeaponSwipeSkill,
 * Returns { proj: 0, damage: 50, size: 50, weight: 25 }
 *
 * So if i want to create a new skill, I can just create a new EtcSkill scene, and then add a script that inherits from Skill
 * I can have a default EvaluatedModifiers property, that i can just access, that should automatically be calculated before i execute the skill
 * And then i write about the actual skill logic, interpreting the calculated modifiers.
 *
 */
public class ModifierHandler
{
    public ModifierResults Results;
    
    public void CalculateModifierResults(ModifierResults results, List<SkillModifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            switch (modifier)
            {
                case ExtraProjectileModifier projModifier:
                    results.AdditionalProjectiles += projModifier.ExtraProjectiles;
                    break;
                case FlatDamageModifier dmgModifier:
                    results.AdditionalFlatDamage += dmgModifier.AdditionalDamage;
                    break;
                default:
                    GD.PushWarning("Modifier type: " + modifier.GetType().Name + " is not being handled.");
                    break;
            }
        }
    }
}