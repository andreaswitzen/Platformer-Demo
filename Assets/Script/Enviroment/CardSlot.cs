using UnityEngine;

// A slot for keycards that will activate a devide (e.g open a door) when a character with the right keycard is near.
public class CardSlot : Trigger
{
	[SerializeField] private KeycardType _type;
	[SerializeField] private Activatable _device;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Sprite _activatedSprite;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.TryGetComponent(out CharacterKeycards keycards) || !keycards.Contains(_type))
			return;

		_device.Activate();
		_spriteRenderer.sprite = _activatedSprite;

		if (_onlyTriggerOnce)
			Deactivate();
	}
}
