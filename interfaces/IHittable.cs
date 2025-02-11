using Godot;

public class HitInformation
{
    public int Damage;
    public int Weight;
    public Vector2 PositionOfHit;

    public HitInformation(int damage, int weight, Vector2 positionOfHit)
    {
        Damage = damage;
        Weight = weight;
        PositionOfHit = positionOfHit;    
    }
}

public class HitResult
{
    public int RemainingHealth;
}

public interface IHittable
{
    void ReceiveHit(HitInformation hitInformation);
}
