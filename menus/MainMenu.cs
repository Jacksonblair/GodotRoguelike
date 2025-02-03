using Godot;

namespace TESTCS.menus;

public partial class MainMenu : Node2D
{
	public Button StartButton;
	public Button OptionsButton;
	public Button HostButton;
	public Button JoinButton;
	public LineEdit ServerAddress;
	
	public override void _Ready()
	{	
		StartButton = GetNode<Button>("%StartButton");
		OptionsButton = GetNode<Button>("%OptionsButton");
		// ServerAddress = GetNode<LineEdit>("%ServerAddress");
		// HostButton = GetNode<Button>("%HostButton");
		// JoinButton = GetNode<Button>("%JoinButton");
		
		StartButton.Pressed += OnStartPressed;
		OptionsButton.Pressed += OnOptionsPressed;
		// HostButton.Pressed += OnHostPressed;
		// JoinButton.Pressed += OnJoinPressed;
	}

	// private void OnJoinPressed()
	// {
	// 	GlobalVariables.GameManager.OnJoinButtonPressed(ServerAddress.Text);
	// }
	//
	// private void OnHostPressed()
	// {
	// 	GlobalVariables.GameManager.OnHostButtonPressed();
	// }

	private void OnOptionsPressed()
	{
		// TODO
	}

	private void OnStartPressed()
	{
		GlobalVariables.GameManager.OnStartGame();
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