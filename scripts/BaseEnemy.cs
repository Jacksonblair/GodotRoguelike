using Godot;
using TESTCS.enums;

public partial class BaseEnemy : CharacterBody2D
{
    public EnemyType enemyType;

    public void Die()
    {
        GlobalVariables.KillTrackingManagerer.TrackKill(enemyType);
    }

    public void Init(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }
}
