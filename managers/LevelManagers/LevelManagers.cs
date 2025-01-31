using Godot;

namespace TESTCS.managers.LevelManagers;

public partial class LevelManagers : Node
{
	public SkillSlotManager SkillSlotManager { get; set; }
	
	public override void _Ready()
	{
		SkillSlotManager = GetNode<SkillSlotManager>("%SkillSlotManager");
	}
	
	// public override void _Process(double delta)
}