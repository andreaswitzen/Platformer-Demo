using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Prime31;
using static Services;

// A game character (i.e. the player character and all enemies).
public class Character : MonoBehaviour
{
	[SerializeField] private float _damageFreezeSeconds;
	[SerializeField] private float _damageInvulSeconds;
	[SerializeField] private float _deathDelaySeconds;
	[SerializeField] private AudioClip _takeDamageSound;

	private WaitForSeconds _damageFreezeDuration;
	private WaitForSeconds _damageInvulDuration;
	private WaitForSeconds _deathDelay;
	private List<SpriteRenderer> _childRenderers = new List<SpriteRenderer>();

	private const int deadLayer = 14;

	public CharacterMovement Movement { get; private set; }
	public CharacterWeapons Weapons { get; private set; }
	public Animator Animator { get; private set; }
	public AudioSource AudioSource { get; private set; }
	public Interactable CurrentInteractable { get; set; }
	[field: SerializeField] public bool IsPlayer { get; private set; }
	[field: SerializeField] public bool IsAlive { get; private set; }
	[field: SerializeField] public bool IsInvulnerable { get; set; }
	[field: SerializeField] public int Health { get; set; }
	[field: SerializeField] public int MaxHealth { get; set; }

	public void Awake()
	{
		Movement = GetComponent<CharacterMovement>();
		Weapons = GetComponent<CharacterWeapons>();
		Animator = GetComponent<Animator>();
		AudioSource = GetComponent<AudioSource>();

		_damageFreezeDuration = new WaitForSeconds(_damageFreezeSeconds);
		_damageInvulDuration = new WaitForSeconds(_damageInvulSeconds);
		_deathDelay = new WaitForSeconds(_deathDelaySeconds);

		CollectChildRenderers();

		if (IsPlayer) {
			// Ensure single instance.
			if (PLAYER_CHARACTER) {
				PLAYER_CHARACTER.transform.position = transform.position;
				Destroy(gameObject);
			}
			else {
				PLAYER_CHARACTER = this;
				DontDestroyOnLoad(gameObject);
			}
		}
	}

	public void TakeDamage(int damage)
	{
		StartCoroutine(TakeDamageRoutine(damage));
	}

	private IEnumerator TakeDamageRoutine(int damage)
	{
		if (Weapons)
			Weapons.TryUnequip();
		
		Movement.TryRise();
		
		Health -= damage;
		AudioSource.TryPlay(_takeDamageSound);
		Animator.TryPlay("Hit");

		// If health is <= 0 after taking damage stop collisions/allow bullets to pass through.
		if (Health <= 0) {
			gameObject.layer = deadLayer;
		}
		
		// If damageInvulSeconds > 0 make invulnerable (and transparent) for duration.
		if (_damageInvulSeconds > 0) {
			StartCoroutine(BecomeInvulnerableFor(_damageInvulDuration));
		}

		// If damageFreezeSeconds > 0 also freeze movement and yield until unfrozen.
		if (_damageFreezeSeconds > 0) {
			yield return FreezeFor(_damageFreezeDuration);
		}

		// If health < 0 when unfrozen, die properly.
		if (Health <= 0) {
			StartCoroutine(Die());
			yield break;
		}

		Animator.TryPlay("Idle");

		if (Weapons)
			Weapons.TryReequip();
	}

	private IEnumerator BecomeInvulnerableFor(WaitForSeconds invulDuration)
	{
		IsInvulnerable = true;
		SetAlpha(0.5f);
		yield return invulDuration;
		IsInvulnerable = false;
		SetAlpha(1f);
	}

	private IEnumerator FreezeFor(WaitForSeconds freezeDuration)
	{
		Movement.Freeze();
		yield return freezeDuration;
		Movement.Unfreeze();
	}

	private IEnumerator Die()
	{
		IsAlive = false;
		GetComponent<SpriteRenderer>().sortingLayerName = "Dead";
		Animator.TryPlay("Die");
		Destroy(GetComponent<ShadowCaster2D>());

		if (IsPlayer) {
			IsInvulnerable = true;
			GAME_MANAGER.GoToGameOver();
		} else {
			Destroy(GetComponent<Behaviour>());
		}

		yield return _deathDelay;

		if (!IsPlayer) {
			Destroy(AudioSource);
			Destroy(Animator);
			Destroy(GetComponent<CharacterController2D>());
			Destroy(GetComponent<Rigidbody2D>());
			Destroy(GetComponent<BoxCollider2D>());
			Destroy(GetComponent<CharacterMovement>());
			Destroy(GetComponent<Character>());
		}
	}

	private void SetAlpha(float alpha)
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();
		var color = spriteRenderer.material.color;
		color.a = alpha;
		spriteRenderer.color = color;

		foreach (var childRenderer in _childRenderers) {
			childRenderer.color = color;
		}
	}

	private void CollectChildRenderers()
	{
		foreach (Transform child in transform) {
			if (child.TryGetComponent(out SpriteRenderer childRenderer)) {
				_childRenderers.Add(childRenderer);
			}
		}
	}
}
