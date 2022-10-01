using UnityEngine;

// An interactable button.
public class Button : Interactable
{
	[SerializeField] private Activatable _device;
	[SerializeField] private Sprite _readySprite;
	[SerializeField] private Sprite _busySprite;
	[SerializeField] private AudioClip _buttonConfirm;
	[SerializeField] private AudioClip _buttonDeny;

	private bool _isBusy;

	public void Awake()
	{
		_device.WasActivated.AddListener(SetBusy);
		_device.WasDeactivated.AddListener(SetReady);
	}

	public void OnDestroy()
	{
		_device.WasActivated.RemoveListener(SetBusy);
		_device.WasDeactivated.RemoveListener(SetReady);
	}

	public override void Interact()
	{
		if (_isBusy) {
			GetComponent<AudioSource>().TryPlay(_buttonDeny);
			return;
		}

		_device.Activate();
		GetComponent<AudioSource>().TryPlay(_buttonConfirm);
	}

	private void SetReady()
	{
		_isBusy = false;
		GetComponent<SpriteRenderer>().sprite = _readySprite;
	}

	private void SetBusy()
	{
		_isBusy = true;
		GetComponent<SpriteRenderer>().sprite = _busySprite;
	}
}
