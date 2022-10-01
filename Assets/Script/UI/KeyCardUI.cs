using UnityEngine;
using UnityEngine.UI;
using static Services;

// Displays player key cards in the UI.
public class KeyCardUI : MonoBehaviour
{
	[SerializeField] private KeycardType _type;

	private CharacterKeycards _playerKeycards;
	private Image _image;

	public void Start()
	{
		_playerKeycards = PLAYER_CHARACTER.GetComponent<CharacterKeycards>();
		_image = GetComponent<Image>();
	}

	public void Update()
	{
		if (_playerKeycards && _playerKeycards.Contains(_type)) {
			_image.enabled = true;
		}
		else {
			_image.enabled = false;
		}
	}
}

