using UnityEngine;
using UnityEngine.UI;
using static Services;

// Displays player health.
public class HealthBarUI : MonoBehaviour
{
	[SerializeField] private Sprite[] _sprites;

	private Image _image;

	public void Awake()
	{
		_image = GetComponent<Image>();
	}

	public void Update()
	{
		if (PLAYER_CHARACTER) {
			_image.sprite = _sprites[PLAYER_CHARACTER.Health];
		}
	}
}
