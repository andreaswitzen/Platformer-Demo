using UnityEngine;
using System.Collections;

// A projectile - normally a bullet fired from a gun.
public class Projectile : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidBody;
	[SerializeField] private Collider2D _collider;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Animator _animator;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private int _damage;
	[SerializeField] private float _speed;
	[SerializeField] private float _speedVariance;
	[SerializeField] private float _spread;
	[SerializeField] private AudioClip _ricochet;

	public void FixedUpdate()
	{
		// Stops shotgun pellets hovering/drifting in the air after hitting invulnerable enemies.
		// This is needed as long as player bullet rigid bodies are dynamic rather than kinematic
		// (which they currently need to be for accurate collision detection).
		if (_rigidBody && _rigidBody.velocity.magnitude < _speed / 2)
			OnImpact();
	}

	public void Launch(float horizontalDirection, Vector3 startPosition)
	{
		gameObject.transform.position = startPosition;

		var verticalDirection = Random.Range(-_spread, +_spread);
		var direction = new Vector2(horizontalDirection, verticalDirection);
		var speed = Random.Range(_speed * (1 - _speedVariance), _speed * (1 + _speedVariance));

		_rigidBody.velocity = direction * speed;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Character character)) {
			if (character.IsInvulnerable || character.Movement.State == MoveState.Ducking)
				return;

			character.TakeDamage(_damage);
		}
		else {
			_audioSource.TryPlay(_ricochet);
		}

		OnImpact();
	}

	private void OnImpact()
	{
		_animator.TryPlay("Impact");
		_collider.enabled = false;
		Destroy(_rigidBody);
		StartCoroutine(DelayedDestroy());
	}

	private IEnumerator DelayedDestroy()
	{
		yield return new WaitForSeconds(0.1f);

		_spriteRenderer.enabled = false;

		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
