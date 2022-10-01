using UnityEngine;
using static Services;

// Makes the camera follow the player character.
// Via: https://answers.unity.com/questions/29183/2d-camera-smooth-follow.html
public class SmoothCamera2D : MonoBehaviour
{
	[SerializeField] private float _smoothTime;
	[SerializeField] private float _maxSpeed;

	private Transform _target;
	private Vector3 _velocity;
	private Vector3 _zOffset;

	public void Start()
	{
		_zOffset = new Vector3(0, 0, transform.position.z);

		_target = PLAYER_CHARACTER.transform.Find("Camera target position");

		if (_target) {
			transform.position = _target.position + _zOffset;
		}
		else {
			Debug.LogError($"{gameObject.name} {GetType()} can't find target to follow.");
		}
	}

	public void LateUpdate()
	{
		if (!_target)
			return;

		var destination = _target.transform.position + _zOffset;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _smoothTime, _maxSpeed,
			Time.deltaTime);
	}
}
