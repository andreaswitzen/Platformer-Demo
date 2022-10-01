using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Collectible health.
public class HealthCollectible : Collectible
{
	public override void TryCollect(Character collector)
	{
		if (collector.Health >= collector.MaxHealth)
			return;

		collector.Health = collector.MaxHealth;
		Consume();
	}
}
