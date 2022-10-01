using System.Collections;
using UnityEngine;
using static Services;

// A player weapon.
// (enemies use hard coded weapons, see RiflemanBehavior class). 
public abstract class Weapon : MonoBehaviour
{
	[SerializeField] protected GameObject _projectilePrefab;
	[SerializeField] protected GameObject _shellPrefab;
	[SerializeField] protected Transform _firingPosition;
	[SerializeField] protected Transform _shellEjectPosition;
	[SerializeField] protected SpriteRenderer _spriteRenderer;
	[SerializeField] protected Animator _animator;
	[SerializeField] protected AudioSource _audioSource;
	[SerializeField] protected AudioClip _fire;
	[SerializeField] protected AudioClip _weaponClick;
	[SerializeField] protected AudioClip _reloadStart;
	[SerializeField] protected AudioClip _reloadFinish;
	[SerializeField] protected float _kickback;
	[SerializeField] protected float _fireRateSeconds;
	[SerializeField] protected float _reloadSeconds;
	[SerializeField] protected float _screenShakeAmount;

	protected WaitForSeconds _fireRateDelay;
	protected WaitForSeconds _reloadDelay;

	protected State _state;

	protected enum State { Ready, Firing, Reloading, Unequipped }

	[field: SerializeField] public int Ammo { get; protected set; }
	[field: SerializeField] public int MaxAmmo { get; protected set; }
	[field: SerializeField] public int InventoryAmmo { get; protected set; }
	[field: SerializeField] public int MaxInventoryAmmo { get; protected set; }
	[field: SerializeField] public int Id { get; protected set; }
	[field: SerializeField] public bool AllowAutoFire { get; protected set; }

	protected float Direction => transform.parent.localScale.x;

	protected bool CanReload => _state == State.Ready && InventoryAmmo > 0 && Ammo != MaxAmmo;

	public virtual void Awake()
	{
		_fireRateDelay = new WaitForSeconds(_fireRateSeconds);
		_reloadDelay = new WaitForSeconds(_reloadSeconds);
	}

	public virtual void OnEquip()
	{
		_state = State.Ready;
		_spriteRenderer.enabled = true;
		_animator.enabled = true;
		_animator.TryPlay("Idle");
	}

	public virtual void OnUnequip()
	{
		_state = State.Unequipped;
		StopAllCoroutines();
		_spriteRenderer.enabled = false;
		_animator.enabled = false;
	}

	public void Fire()
	{
		if (_state != State.Ready)
			return;

		StartCoroutine(FireRoutine());
	}

	public virtual IEnumerator Reload()
	{
		if (!CanReload)
			yield break;

		_state = State.Reloading;

		_animator.TryPlay("Reload");
		_audioSource.TryPlay(_reloadStart);

		yield return _reloadDelay;

		var desiredAmmo = MaxAmmo - Ammo;
		var loadedAmmo = desiredAmmo < InventoryAmmo ? desiredAmmo : InventoryAmmo;

		Ammo += loadedAmmo;
		InventoryAmmo -= loadedAmmo;

		_animator.TryPlay("Idle");
		_audioSource.TryPlay(_reloadFinish);

		_state = State.Ready;
	}

	public void AddInventoryAmmo(int amount)
	{
		var newTotal = InventoryAmmo + amount;
		InventoryAmmo = newTotal > MaxInventoryAmmo ? MaxInventoryAmmo : newTotal;
	}

	public void RemoveInventoryAmmo(int amount)
	{
		var newTotal = InventoryAmmo - amount;
		InventoryAmmo = newTotal < 0 ? 0 : newTotal;
	}

	protected abstract IEnumerator FireRoutine();

	protected virtual void EmptyFire()
	{
		_audioSource.TryPlay(_weaponClick);
	}

	protected virtual void LaunchProjectile()
	{
		var bullet = Instantiate(_projectilePrefab, INSTANTIATED_OBJS_PARENT.transform);
		bullet.GetComponent<Projectile>().Launch(Direction, _firingPosition.position);
		Ammo--;

		SCREEN_SHAKER.Shake(_screenShakeAmount);
		transform.parent.Translate(new Vector2(_kickback * -Direction, 0));
	}

	protected void EjectShell()
	{
		var shellCase = Instantiate(_shellPrefab, INSTANTIATED_OBJS_PARENT.transform);
		shellCase.GetComponent<Shell>().Initialize(Direction * -1, _shellEjectPosition.position);
	}
}
