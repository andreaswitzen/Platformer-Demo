using System.Collections;
using UnityEngine;

// Adds a keyboard hotkey to a UI button.
public class MenuButtonWithHotkey : MenuButton
{
    [SerializeField] private KeyCode hotkey;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(hotkey)) {
            _button.onClick.Invoke();
            _audioSource.TryPlay(_submit);
        }
    }
}
