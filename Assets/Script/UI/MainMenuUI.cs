using UnityEngine;
using TMPro;
using static Services;

// Main menu UI general functionality. Acts as relay between UI buttons and current game manager instance.
public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private TMP_Text _versionLabel;

	public void Start()
	{
		UI_MANAGER.ShowCursor();
		_versionLabel.text = $"Version: {Application.version}";
	}

	public void StartGame()
	{
		GAME_MANAGER.StartNewGame();
	}

	public void QuitApplication()
	{
		GAME_MANAGER.QuitApplication();
	}
}
