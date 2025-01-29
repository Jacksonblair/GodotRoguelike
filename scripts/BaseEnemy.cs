using Godot;
using TESTCS.enums;

public partial class BaseEnemy : CharacterBody2D
{
    public EnemyType enemyType;
    public int Weight = 10;
    public float Height = 0;
    public bool IsAirborne = false;

    protected BaseEnemy(EnemyType type)
    {
        enemyType = type;
    }

    public void Die()
    {
        // GlobalVariables.Instance.KillTrackingManager.TrackKill(enemyType);
    }
}
