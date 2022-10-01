using System.Collections;
using UnityEngine;

// The player pistol.
public class Pistol : Weapon
{
	[SerializeField] private float _holsterSeconds;

	private WaitForSeconds _holsterDelay;

	public override void Awake()
	{
		base.Awake();
		_holsterDelay = new WaitForSeconds(_holsterSeconds);
	}

	public override void OnEquip()
	{
		base.OnEquip();
		// Using a string as parameter here since it stops _all_ coroutines of this type.
		// Fixes bugs where the pistol is otherwise holstered in situations where it shouldn't.
		// Todo: could maybe be solved by caching the holster routine instead.
		StartCoroutine("HolsterRoutine");
	}

	protected override IEnumerator FireRoutine()
	{
		_state = State.Firing;

		if (Ammo > 0) {
			LaunchProjectile();
			EjectShell();
			_animator.TryPlay("Fire");
			_audioSource.TryPlay(_fire);
		}
		else {
			EmptyFire();
		}

		// Using a string as parameter here since it stops _all_ coroutines of this type.
		// Fixes bugs where the pistol is otherwise holstered in situations where it shouldn't.
		StopCoroutine("HolsterRoutine");

		yield return _fireRateDelay;

		_animator.TryPlay("Idle");
		_state = State.Ready;
		StartCoroutine("HolsterRoutine");
	}

	private IEnumerator HolsterRoutine()
	{
		yield return _holsterDelay;
		_animator.TryPlay("Holster");
	}
}
