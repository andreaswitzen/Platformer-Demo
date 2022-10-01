using UnityEngine;

// Something that the player can interact with by pressing the interact button ('E' key by default).
public abstract class Interactable : MonoBehaviour
{
	public abstract void Interact();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Character character)) {
			character.CurrentInteractable = this;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Character character) && character.CurrentInteractable == this) {
			character.CurrentInteractable = null;
		}
	}
}
