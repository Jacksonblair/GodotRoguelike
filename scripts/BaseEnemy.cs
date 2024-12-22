using Godot;
using TESTCS.enums;

public partial class BaseEnemy : CharacterBody2D
{
    public EnemyType enemyType;

    protected BaseEnemy(EnemyType type)
    {
        enemyType = type;
    }

    public void Die()
    {
        GlobalVariables.Instance.KillTrackingManager.TrackKill(enemyType);
    }
}
