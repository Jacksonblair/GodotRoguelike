using Godot;
using Godot.Collections;
using TESTCS.enums;

[GlobalClass]
public partial class KillTrackingManager : Node
{
    [Signal]
    public delegate void EnemyKilledEventHandler(EnemyType type, int killCount);

    private Dictionary<EnemyType, int> _killCounts;

    public void TrackKill(EnemyType type)
    {
        // GD.Print("KILLED");

        _killCounts[type]++;
        // GD.Print("Enemy of type: " + type + " died.");
        // GD.Print("Total killed: " + +_killCounts[type]);
        EmitSignal("EnemyKilled", Variant.From(type), _killCounts[type]);
    }

    public int GetKillCount(EnemyType type)
    {
        if (!_killCounts.ContainsKey(type))
        {
            _killCounts[type] = 0;
            return 0;
        }
        return _killCounts[type];
    }

    public override void _Ready()
    {
        this._killCounts = new Dictionary<EnemyType, int>();
        foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
        {
            _killCounts[type] = 0;
        }

        Global.KillTrackingManagerer = this;
        TrackKill(EnemyType.Ghost1);
    }

    public override void _Process(double delta) { }
}
