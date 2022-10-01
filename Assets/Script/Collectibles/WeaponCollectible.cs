using UnityEngine;

// A collectible weapon.
public class WeaponCollectible : Collectible
{
	[SerializeField] private WeaponType _type;

	public override void TryCollect(Character collector)
	{
		if (!collector.TryGetComponent(out CharacterWeapons weapons))
			return;

		var weaponID = (int) _type;

		weapons.Add(weaponID);
		weapons.Equip(weapons.Get(weaponID));
		Consume();
	}
}
