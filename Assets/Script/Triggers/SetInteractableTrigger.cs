using UnityEngine;

// Sets the current interactable of a character when triggered.
// Allows the player to interact with an interactable when near it.
public class SetInteractableTrigger : Trigger
{
	[SerializeField] private Interactable interactable;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Character character))
			character.CurrentInteractable = interactable;
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Character character)
		 && character.CurrentInteractable == interactable)
			character.CurrentInteractable = null;
	}
}
