using UnityEngine;

// Bullet shell casing. Ejected when firing a gun.
public class Shell : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidBody;
	[SerializeField] private BoxCollider2D _collider;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private float _spread;
	[SerializeField] private float _force;
	[SerializeField] private float _forceVariance;
	[SerializeField] private float _torque;
	[SerializeField] private float _torqueVariance;
	[SerializeField] private float _destroySeconds;

	public void Update()
	{
		if (_destroySeconds > 0) {
			_destroySeconds -= Time.deltaTime;
		}
		else {
			Destroy(_audioSource);
			Destroy(_collider);
			Destroy(_rigidBody);
			Destroy(this);
		}
	}

	public void Initialize(float horizontalDirection, Vector3 startPosition)
	{
		var verticalDirection = Random.Range(1 - _spread, 1 + _spread);
		var force = Random.Range(_force * (1 - _forceVariance), _force * (1 + _forceVariance));
		var torque = Random.Range(_torque * (1 - _torqueVariance), _torque * (1 + _torqueVariance));

		gameObject.transform.position = startPosition;

		_rigidBody.AddForce(new Vector2(horizontalDirection, verticalDirection) * force);
		_rigidBody.AddTorque(torque);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		_audioSource.Play();
	}
}
