using UnityEngine;

// Shakes the camera.
// Via: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/
public class CameraShaker : MonoBehaviour
{
	private Camera _camera;
	private float _shakeAmount;

	public void Awake()
	{
		_camera = gameObject.GetComponent<Camera>();
		Services.SCREEN_SHAKER = this;
	}

	public void Shake(float shakeAmount)
	{
		_shakeAmount = shakeAmount;
		InvokeRepeating("CameraTwitch", 0, .01f);
		Invoke("StopShaking", 0.3f);
	}

	private void CameraTwitch()
	{
		if (_shakeAmount > 0) {
			var verticalTwitchAmount = Random.value * _shakeAmount * 2 - _shakeAmount;
			var horizontalTwitchAmount = Random.value * _shakeAmount * 2 - _shakeAmount;
			var position = _camera.transform.position;
			position.y += verticalTwitchAmount;
			position.x += horizontalTwitchAmount;
			_camera.transform.position = position;
		}
	}

	private void StopShaking()
	{
		CancelInvoke("CameraTwitch");
	}
}
