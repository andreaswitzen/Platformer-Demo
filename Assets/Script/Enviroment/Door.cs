using UnityEngine;

// A door which can be opened and closed.
public class Door : Activatable
{
	[SerializeField] private Sprite _openSprite;
	[SerializeField] private Sprite _closedSprite;
	[SerializeField] private bool _openOnStart;

	private bool _isOpen;

	public void Start()
	{
		if (_openOnStart)
			Open(true);
	}

	public override void Activate()
	{
		if (!_isOpen) {
			Open();
		}
		else {
			Close();
		}

		_isOpen = !_isOpen;
	}

	public void Open(bool openSilently = false)
	{
		gameObject.layer = 12;
		GetComponent<SpriteRenderer>().sprite = _openSprite;

		if (!openSilently)
			GetComponent<AudioSource>().Play();
	}

	public void Close(bool closeSilently = false)
	{
		gameObject.layer = 0;
		GetComponent<SpriteRenderer>().sprite = _closedSprite;

		if (!closeSilently)
			GetComponent<AudioSource>().Play();
	}
}
