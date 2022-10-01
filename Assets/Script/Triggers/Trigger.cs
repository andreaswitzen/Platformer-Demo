using System.Collections;
using UnityEngine;

// Common behavior for triggers.
public abstract class Trigger : Activatable
{
	[SerializeField] protected bool _onlyTriggerOnce;
	[SerializeField] protected float _cooldownSeconds;

	protected bool _isCoolingDown;

	private WaitForSeconds _cooldown;

	public virtual void Awake()
	{
		if (_cooldownSeconds > 0)
			_cooldown = new WaitForSeconds(_cooldownSeconds);
	}

	public override void Activate()
	{
		GetComponent<BoxCollider2D>().enabled = true;
	}

	public override void Deactivate()
	{
		GetComponent<BoxCollider2D>().enabled = false;
	}

	protected IEnumerator ActivateCooldown()
	{
		_isCoolingDown = true;
		yield return _cooldown;
		_isCoolingDown = false;
	}
}
