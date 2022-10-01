using System.Collections;
using UnityEngine;
using static Services;

// The player shotgun.
public class Shotgun : Weapon
{
	public int _projectiles;
	public float _ejectSeconds;
	public AudioClip _shellEject;

	private WaitForSeconds _ejectDelay;

	public override void Awake()
	{
		base.Awake();
		_ejectDelay = new WaitForSeconds(_ejectSeconds);
	}

	protected override IEnumerator FireRoutine()
	{
		_state = State.Firing;

		if (Ammo > 0) {
			LaunchProjectile();
			StartCoroutine(EjectShell());
			_animator.TryPlay("Fire");
			_audioSource.TryPlay(_fire);
		}
		else {
			EmptyFire();
		}

		yield return _fireRateDelay;

		_state = State.Ready;
	}

	protected override void LaunchProjectile()
	{
		for (int i = 0; i < _projectiles; i++) {
			var pellet = Instantiate(_projectilePrefab, INSTANTIATED_OBJS_PARENT.transform);
			pellet.GetComponent<Projectile>().Launch(Direction, _firingPosition.position);
		}

		Ammo--;

		SCREEN_SHAKER.Shake(_screenShakeAmount);
		transform.parent.Translate(new Vector2(_kickback * -Direction, 0));
	}

	private new IEnumerator EjectShell()
	{
		yield return _ejectDelay;

		var shellCase = Instantiate(_shellPrefab, INSTANTIATED_OBJS_PARENT.transform);
		shellCase.GetComponent<Shell>().Initialize(-Direction, _shellEjectPosition.position);
		_animator.TryPlay("Eject");
		_audioSource.TryPlay(_shellEject);
	}
}
