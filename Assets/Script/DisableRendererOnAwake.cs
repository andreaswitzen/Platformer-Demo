using UnityEngine;

// Disables a gameObjects renderer on awake.
public class DisableRendererOnAwake : MonoBehaviour
{
	public void Awake()
	{
		if (TryGetComponent(out Renderer renderer))
			renderer.enabled = false;
	}
}
