using System.Collections.Generic;
using UnityEngine;

// A trigger which activates other objects when triggered (e.g. other triggers).
public class SetActiveTrigger : Trigger
{
	[SerializeField] private List<Activatable> _activatables;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		foreach (var activatable in _activatables)
			activatable.Activate();
	}
}
