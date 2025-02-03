using Godot;

namespace TESTCS.managers.LevelManagers;

public partial class LevelManagers : Node
{
	public PlayerSkillSlotManager PlayerSkillSlotManager { get; set; }
		
	public override void _Ready()
	{
		this.PlayerSkillSlotManager = GetNode<PlayerSkillSlotManager>("PlayerSkillSlotManager");
	}
}