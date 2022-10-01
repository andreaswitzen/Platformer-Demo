using System.Collections;
using UnityEngine;
using static Services;

// Behavior of Rifleman enemies.
public class RiflemanBehavior : CharacterBehavior
{
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private GameObject _projectilePrefab;
	[SerializeField] private GameObject _shellPrefab;
	[SerializeField] private Transform _firingPosition;
	[SerializeField] private Transform _shellEjectPosition;
	[SerializeField] private int _projectiles;
	[SerializeField] private float _fireRateSeconds;
	[SerializeField] private float _chargeSeconds;
	[SerializeField] private float _rechargeSeconds;
	[SerializeField] private float _range;
	[SerializeField] private float _rangeVariance;
	[SerializeField] private AudioClip _fire;

	private float _defaultRange;
	private WaitForSeconds _chargeDelay;
	private WaitForSeconds _rechargeDelay;
	private WaitForSeconds _fireRateDelay;

	private States State { get; set; }

	private enum States { Searching, Shooting, Recharging }

	public void Start()
	{
		_chargeDelay = new WaitForSeconds(_chargeSeconds);
		_rechargeDelay = new WaitForSeconds(_rechargeSeconds);
		_fireRateDelay = new WaitForSeconds(_fireRateSeconds);
		_defaultRange = _range;
		RandomizeRange();

		_movement.WasFrozen.AddListener(OnFrozen);
		_movement.WasUnfrozen.AddListener(OnUnfrozen);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_movement.WasFrozen.RemoveListener(OnFrozen);
		_movement.WasUnfrozen.RemoveListener(OnUnfrozen);
	}

	public override void Execute()
	{
		if (!_character.IsAlive || !_isActive)
			return;

		if (State == States.Searching) {

			var currentDirection = new Vector2(CurrentHorizontalDirection, 0);

			// If the player character is in front of the rifleman and within range, fire.
			if (PlayerIsInRange(currentDirection)) {
				_movement.Stop();
				StartCoroutine(Fire());
			}
			// If the player character is behind enemy and within range, turn towards him.
			else if (PlayerIsInRange(-currentDirection)) {
				_movement.FlipHorizontally();
			}
			// Otherwise, use default movement.
			else {
				base.Execute();
			}
		}
	}

	private bool PlayerIsInRange(Vector2 direction)
	{
		var layerMask = LayerMask.GetMask("Default", "Player");
		var raycast = Physics2D.Raycast(transform.position, direction, _range, layerMask);
		Debug.DrawRay(_firingPosition.position, direction * _range, Color.green);

		// Todo: this can/should maybe be done without the GetComponent.
		if (raycast.collider && raycast.collider.GetComponent<Character>())
			return true;

		return false;
	}

	private IEnumerator Fire()
	{
		State = States.Shooting;

		yield return _chargeDelay;

		for (int i = 0; i < _projectiles; i++) {
			var projectile = Instantiate(_projectilePrefab, INSTANTIATED_OBJS_PARENT.transform);
			var shellCase = Instantiate(_shellPrefab, INSTANTIATED_OBJS_PARENT.transform);

			projectile.GetComponent<Projectile>().Launch(CurrentHorizontalDirection, _firingPosition.position);
			shellCase.GetComponent<Shell>().Initialize(CurrentHorizontalDirection * -1,
				_shellEjectPosition.position);

			_audioSource.TryPlay(_fire);

			yield return _fireRateDelay;
		}

		StartCoroutine(Recharge());
	}

	private IEnumerator Recharge()
	{
		State = States.Recharging;
		_character.Animator.StopPlayback();

		yield return _rechargeDelay;

		var currentDirection = new Vector2(CurrentHorizontalDirection, 0);

		if (!PlayerIsInRange(currentDirection))
			RandomizeRange();

		State = States.Searching;
	}

	private void RandomizeRange()
	{
		_range = Random.Range(_defaultRange * (1 - _rangeVariance), _defaultRange * (1 + _rangeVariance));
	}

	private void OnFrozen()
	{
		_isActive = false;
		StopAllCoroutines();
	}

	private void OnUnfrozen()
	{
		_isActive = true;
		State = States.Searching;
	}
}
