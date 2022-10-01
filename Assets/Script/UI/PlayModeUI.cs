using UnityEngine;
using static Services;

// Play mode UI general functionality. Mainly acts as a relay for UI buttons and managers.
public class PlayModeUI : MonoBehaviour
{
	public GameObject HUD;
	public GameObject PauseScreen;
	public GameObject GameOverScreen;

	public void Awake()
	{
		PLAY_MODE_UI = this;
	}

	// The below wrappers are here so that a PlayModeUI prefab can be dropped into a level scene without having to
	// hook it up to the GameManager manually in each.
	public void Resume()
	{
		GAME_MANAGER.Play();
	}

	public void QuitToMenu()
	{
		GAME_MANAGER.QuitToMainMenu();
	}

	public void QuitApplication()
	{
		GAME_MANAGER.QuitApplication();
	}
}
