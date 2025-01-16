// Modifier that adds an extra projectile
namespace TESTCS.skills.Modifiers;

public class FlatDamageModifier : SkillModifier
{
    public int AdditionalDamage;
    public FlatDamageModifier(int dmg)
    {
        AdditionalDamage = dmg;
    }
}