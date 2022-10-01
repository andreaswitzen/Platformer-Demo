using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Services;
using static GameState;

// Handles game state & loading scenes.
public class GameManager : MonoBehaviour
{
	public GameState State { get; private set; }

	private WaitForSecondsRealtime _sceneLoadDelay = new WaitForSecondsRealtime(0.2f);

	public void Awake()
	{
		// Ensure single instance.
		if (GAME_MANAGER) {
			Debug.Log("More than one GameManager instance in scene, destroying.");
			Destroy(gameObject);
		}
		else {
			GAME_MANAGER = this;
			DontDestroyOnLoad(gameObject);
		}

		State = SceneManager.GetActiveScene().buildIndex == 0 ? MainMenu : Playing;

		Application.targetFrameRate = 60;
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	public void QuitToMainMenu()
	{
		LoadScene(0);
	}

	public void StartNewGame()
	{
		LoadScene(1);
	}

	public void LoadScene(int sceneIndex)
	{
		StartCoroutine(LoadSceneRoutine(sceneIndex));
	}

	public void ReloadScene()
	{
		TryDestroyPlayerCharacter();
		var scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
		Play();
	}

	public void Play()
	{
		State = Playing;
		Time.timeScale = 1.0f;
		UI_MANAGER.OnPlay();
	}

	public void Pause()
	{
		State = Paused;
		Time.timeScale = 0f;
		UI_MANAGER.OnPause();
	}

	public void GoToGameOver()
	{
		State = GameOver;
		UI_MANAGER.OnGameOver();
	}

	private IEnumerator LoadSceneRoutine(int sceneIndex)
	{
		yield return _sceneLoadDelay;

		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);

		// If main menu.
		if (sceneIndex == 0) {
			TryDestroyPlayerCharacter();
			GoToMainMenu();
		}
		// If end scene.
		else if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1) {
			TryDestroyPlayerCharacter();
			GoToEndState();
		}
		// Otherwise.
		else {
			Play();
		}
	}

	private void GoToMainMenu()
	{
		State = MainMenu;
		UI_MANAGER.OnMainMenu();
		Time.timeScale = 1f;
	}

	private void GoToEndState()
	{
		State = EndState;
		Time.timeScale = 0f;
	}

	private void TryDestroyPlayerCharacter()
	{
		if (PLAYER_CHARACTER)
			Destroy(PLAYER_CHARACTER.gameObject);
	}
}
