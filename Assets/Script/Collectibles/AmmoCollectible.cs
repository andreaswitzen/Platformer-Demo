using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Collectible ammunition.
public class AmmoCollectible : Collectible
{
	[SerializeField] private WeaponType _type;
	[SerializeField] private int _amount;

	public override void TryCollect(Character collector)
	{
		if (!collector.TryGetComponent(out CharacterWeapons weapons))
			return;

		var weapon = weapons.Get((int)_type);

		if (weapon.InventoryAmmo < weapon.MaxInventoryAmmo) {
			weapon.AddInventoryAmmo(_amount);
			Consume();
		}
	}
}
