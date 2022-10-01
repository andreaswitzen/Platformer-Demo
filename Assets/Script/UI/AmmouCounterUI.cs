using UnityEngine;
using UnityEngine.UI;
using static Services;

// Displays ammo count for the currently equipped weapon.
public class AmmouCounterUI : MonoBehaviour
{
	[SerializeField] private Text _text;

	public void Update()
	{
		if (PLAYER_CHARACTER
		 && PLAYER_CHARACTER.TryGetComponent(out CharacterWeapons playerWeapons)
		 && playerWeapons.EquippedWeapon) {
			_text.text = $"Ammo: {playerWeapons.EquippedWeapon.Ammo} / {playerWeapons.EquippedWeapon.InventoryAmmo}";
		}
		else {
			_text.text = "";
		}
	}
}
