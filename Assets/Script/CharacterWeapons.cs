using System.Collections.Generic;
using UnityEngine;

// The weapons carried by a character.
// Currently only used by the player character. Enemy weapons are hardcoded, see RiflemanBehavior class.
public class CharacterWeapons : MonoBehaviour
{
	[SerializeField] private List<Weapon> _weapons;

	private Weapon _lastEquippedWeapon;

	[field: SerializeField] public Weapon EquippedWeapon { get; private set; }

	public void Awake()
	{
		CharacterMovement movement = GetComponent<CharacterMovement>();
		movement.CharacterDucked.AddListener(TryUnequip);
		movement.CharacterRose.AddListener(TryReequip);
	}

	public void OnDestroy()
	{
		CharacterMovement movement = GetComponent<CharacterMovement>();
		movement.CharacterDucked.RemoveListener(TryUnequip);
		movement.CharacterRose.RemoveListener(TryReequip);
	}

	// Cycle through carried weapons.
	public void Cycle()
	{
		if (!EquippedWeapon)
			return;

		var id = EquippedWeapon.Id;

		do {
			id++;
			id = id < _weapons.Count ? id : 0;
		} while (!Contains(id));

		Equip(_weapons[id]);
	}

	public void Add(int id)
	{
		_weapons[id].gameObject.SetActive(true);
	}

	public Weapon Get(int id)
	{
		return _weapons[id];
	}

	public void Reload()
	{
		EquippedWeapon.StartCoroutine(EquippedWeapon.Reload());
	}

	private bool Contains(int id)
	{
		return _weapons[id].gameObject.activeInHierarchy;
	}

	public void Equip(Weapon weapon)
	{
		TryUnequip();
		EquippedWeapon = weapon;
		weapon.OnEquip();
	}

	public void TryUnequip()
	{
		if (!EquippedWeapon)
			return;

		_lastEquippedWeapon = EquippedWeapon;
		EquippedWeapon.OnUnequip();
		EquippedWeapon = null;
	}

	public void TryReequip()
	{
		if (!_lastEquippedWeapon)
			return;

		Equip(_lastEquippedWeapon);
	}
}
