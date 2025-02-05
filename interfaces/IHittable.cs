using Godot;

public class HitInformation
{
    public int Damage;
    public int Weight;
    public Vector2 Position;

    public HitInformation(int damage, int weight, Vector2 position)
    {
        Damage = damage;
        Weight = weight;
        Position = position;    
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
