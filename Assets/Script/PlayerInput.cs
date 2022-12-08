using UnityEngine;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;
using static Services;
using static GameState;
using static MoveState;

// Handles player input.
// Note: this is an old script that should be replaced/rewritten to use Unity's newer input system.
// This would for instance make it easier to add key rebinding or to enable using other controllers than a keyboard.
public class PlayerInput : MonoBehaviour
{
	[SerializeField] private GameObject _graphy;
	[SerializeField] private int _maxJumpBufferFrames;

	private int _jumpBuffer;
	private int _screenShotIndex;

	#region Key bindings

	private bool PausePressed        => GetKeyDown(Escape);

	private bool RestartPressed      => GetKeyDown(Return);

	private bool GraphyPressed       => GetKeyDown(Backslash);

	private bool ScreenshotPressed   => GetKeyDown(F12);

	private bool InteractPressed     => GetKeyDown(E);

	private bool DuckHeld            => GetKey(S)
	                                 || GetKey(DownArrow);

	private bool DuckReleased        => GetKeyUp(S)
	                                 || GetKeyUp(DownArrow);

	private bool LeftHeld            => GetKey(LeftArrow)
	                                 || GetKey(A);

	private bool RightHeld           => GetKey(RightArrow)
	                                 || GetKey(D);

	private bool JumpPressed         => GetKeyDown(UpArrow)
	                                 || GetKeyDown(W)
	                                 || GetKeyDown(KeyCode.Space);

	private bool JumpReleased        => GetKeyUp(UpArrow)
	                                 || GetKeyUp(W)
	                                 || GetKeyUp(KeyCode.Space);

	private bool CycleWeaponsPressed => GetKeyDown(LeftAlt)
	                                 || GetKeyDown(RightAlt);

	private bool FireWeaponPressed   => GetKeyDown(LeftControl)
	                                 || GetKeyDown(RightControl);

	private bool FireWeaponHeld	     => GetKey(LeftControl)
	                                 || GetKey(RightControl);

	private bool ReloadWeaponPressed => GetKeyDown(R)
	                                 || GetKeyDown(LeftShift)
	                                 || GetKeyDown(RightShift);

	#endregion

	public void Update()
	{
		HandleGeneralInput();

		switch (GAME_MANAGER.State) {

			case Playing:
				if (!PLAYER_CHARACTER || !PLAYER_CHARACTER.IsAlive)
					return;

				HandleMovementInput();
				HandleWeaponInput();
				break;

			case GameOver:
				if (RestartPressed) {
					GAME_MANAGER.ReloadScene();
				}
				break;

			case EndState:
				if (anyKeyDown) {
					GAME_MANAGER.QuitToMainMenu();
				}
				break;
			default:
				break;
		}
	}

	private void HandleGeneralInput()
	{
		if (GraphyPressed && _graphy)
			_graphy.SetActive(!_graphy.activeInHierarchy);

		if (ScreenshotPressed) {
			ScreenCapture.CaptureScreenshot($"image{_screenShotIndex}.png");
			_screenShotIndex++;
		}
	}

	private void HandleMovementInput()
	{
		var player = PLAYER_CHARACTER.Movement;

		if (PausePressed) {
			GAME_MANAGER.Pause();
		}
		if (InteractPressed && PLAYER_CHARACTER.CurrentInteractable) {
			PLAYER_CHARACTER.CurrentInteractable.Interact();
		}
		if (DuckReleased) {
			player.TryRise();
		}
		if (DuckHeld) {
			player.TryDuck();
			return;
		}
		if (LeftHeld) {
			player.TryMove(-1);
		}
		if (RightHeld) {
			player.TryMove(1);
		}
		if (JumpPressed) {
			_jumpBuffer = _maxJumpBufferFrames;
		}
		if (_jumpBuffer > 0) {
			player.TryJump();
			_jumpBuffer--;
		}
		if (JumpReleased) {
			player.TryAbortJump();
		}
	}

	private void HandleWeaponInput()
	{
		var playerWeapons = PLAYER_CHARACTER.GetComponent<CharacterWeapons>();

		if (!playerWeapons.EquippedWeapon || PLAYER_CHARACTER.Movement.State == Ducking)
			return;

		if (CycleWeaponsPressed) {
			playerWeapons.Cycle();
		}
		if (ReloadWeaponPressed) {
			playerWeapons.Reload();
		}
		if (playerWeapons.EquippedWeapon.AllowAutoFire && FireWeaponHeld) {
			playerWeapons.EquippedWeapon.Fire();
		}
		else if (FireWeaponPressed) {
			playerWeapons.EquippedWeapon.Fire();
		}
	}
}
