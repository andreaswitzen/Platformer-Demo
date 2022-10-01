using System.Collections;

// The player cannon/minigun.
public class Cannon : Weapon
{
	protected override IEnumerator FireRoutine()
	{
		_state = State.Firing;

		if (Ammo > 0) {
			LaunchProjectile();
			EjectShell();
			_audioSource.TryPlay(_fire);
		}
		else {
			EmptyFire();
		}

		_animator.TryPlay("Fire");

		yield return _fireRateDelay;

		_state = State.Ready;
	}
}
