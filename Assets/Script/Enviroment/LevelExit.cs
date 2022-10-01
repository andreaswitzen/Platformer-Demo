using UnityEngine.SceneManagement;
using static Services;

// Lets the player character exit a level and proceed to the next one.
public class LevelExit : Interactable
{
	public override void Interact()
	{
		GAME_MANAGER.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
