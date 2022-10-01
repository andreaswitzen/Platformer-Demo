using UnityEngine;

// Rotates a gameObject horizontally (around the y axis).
public class HorizontalRotation : MonoBehaviour
{
	public float _rotationSpeed;

	public void Update()
	{
		transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.up);
	}
}
