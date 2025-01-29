using Godot;

namespace TESTCS.menus;

public partial class MainMenu : Node2D
{
	public Button StartButton;
	public Button OptionsButton;
	
	public override void _Ready()
	{	
		StartButton = GetNode<Button>("%StartButton");
		OptionsButton = GetNode<Button>("%OptionsButton");
		StartButton.Pressed += OnStartPressed;
		OptionsButton.Pressed += OnOptionsPressed;
	}

	private void OnOptionsPressed()
	{
		// TODO
	}

	private void OnStartPressed()
	{
		GlobalVariables.GameManager.LoadLastSave();
		// GlobalVariables.Instance.GameManager.LoadLevel();
		/**
		 * When i press start, i want to:
		 * - Using the GameState, load up the last level i was in.
		 * - Add a scene including all the managers required for the game
		 *		- SkilLSlotMgr
		 *		- Etc...
		 *
		 * Assuming i dont need those manager soutside game scenes...
		 * 
		 */
	}

	public override void _Process(double delta)
	{
	}
}