namespace TESTCS.skills.Modifiers;

public class ExtraProjectileModifier : SkillModifier
{
    public readonly int ExtraProjectiles;
    public ExtraProjectileModifier(int count)
    {
        ExtraProjectiles = count;
    }
}

