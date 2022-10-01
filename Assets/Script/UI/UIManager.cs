using UnityEngine;
using static Services;

// Manages UI.
public class UIManager : MonoBehaviour
{
    public void Awake()
    {
        if (!UI_MANAGER) {
            UI_MANAGER = this;
        }
    }

    public void OnMainMenu()
    {
        HideHud();
        HidePauseMenu();
        HideGameOverOverlay();
        ShowCursor();
    }

    public void OnPlay()
    {
        HidePauseMenu();
        HideGameOverOverlay();
        ShowHud();
        HideCursor();
    }

    public void OnPause()
    {
        HideHud();
        HideGameOverOverlay();
        ShowPauseMenu();
        ShowCursor();
    }

    public void OnGameOver()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.GameOverScreen.SetActive(true);
        }
    }

    public void ShowHud()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.HUD.SetActive(true);
        }
    }

    public void HideHud()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.HUD.SetActive(false);
        }
    }

    public void ShowPauseMenu()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.PauseScreen.SetActive(true);
        }
    }

    public void HidePauseMenu()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.PauseScreen.SetActive(false);
        }
    }

    public void ShowGameOverOverlay()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.GameOverScreen.SetActive(true);
        }
    }

    public void HideGameOverOverlay()
    {
        if (PLAY_MODE_UI) {
            PLAY_MODE_UI.GameOverScreen.SetActive(false);
        }
    }

    public void ShowCursor()
    {
        if (!Application.isEditor) {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        if (!Application.isEditor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
