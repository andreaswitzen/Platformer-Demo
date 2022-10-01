using UnityEngine;
using UnityEngine.Events;

// Something which can be activated, and potentially deactived.
public abstract class Activatable : MonoBehaviour
{
	[HideInInspector] public UnityEvent WasActivated;

	[HideInInspector] public UnityEvent WasDeactivated;

	public abstract void Activate();

	public virtual void Deactivate() { }
}
