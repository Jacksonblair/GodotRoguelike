using System;
using Godot;
using TESTCS.enums;

public partial class Quest1 : Node
{
    private bool started;
    private bool finished;
    private int ghostsKilled;

    public void StartQuest()
    {
        this.started = true;
    }

    public override void _Ready()
    {
        GD.Print("QUEST 1 is READY");
        Global.KillTrackingManagerer.EnemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled(EnemyType type, int killCount)
    {
        GD.Print(type);
        GD.Print(killCount);
        GD.Print("Quest 1 was notified than an enemy was kiklled");
        ghostsKilled++;

        if (ghostsKilled >= 3)
        {
            GD.Print("QUEST IS COMPLETE");
        }
    }

    public void FinishQuest()
    {
        this.finished = true;
    }
}
