using UnityEngine;
using UnityEngine.EventSystems;

// Additional behavior for menu/UI buttons.
public class MenuButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] protected UnityEngine.UI.Button _button;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _submit;
    [SerializeField] protected AudioClip _select;

    [SerializeField] private bool _selectOnEnable;

	public void OnEnable()
	{
        // This is a workaround as EventSystems "First Selected" has two problems:
        // 1) Audio will play when a button is selected by the EventSystem (not only the player)
        // 2) No button is selected when disabling a panel/screen and enabling another (like when going from
        // the main menus main screen to the credits screen).
        if (_selectOnEnable) {
            var selectClip = _select;
            _select = null;
            _button.Select();
            _select = selectClip;
        }
	}

	public void OnSelect(BaseEventData eventData)
	{
        _audioSource.TryPlay(_select);
	}

    public void OnSubmit(BaseEventData eventData)
    {
        _audioSource.TryPlay(_submit);
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
        _button.Select();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
        OnSubmit(eventData);
    }
}
