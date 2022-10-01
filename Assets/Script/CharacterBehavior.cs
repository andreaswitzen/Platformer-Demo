using UnityEngine;

// Base behavior of AI controlled characters.
public class CharacterBehavior : MonoBehaviour
{
	protected Character _character;
	protected CharacterMovement _movement;
	protected bool _isActive = true;

	protected float CurrentHorizontalDirection => transform.localScale.x;

	public virtual void Awake()
	{
		_character = GetComponent<Character>();
		_movement = GetComponent<CharacterMovement>();
		_movement.Controller.onControllerCollidedEvent += OnControllerCollider;
	}

	public virtual void OnDestroy()
	{
		_movement.Controller.onControllerCollidedEvent -= OnControllerCollider;
		StopAllCoroutines();
	}

	public void Update()
	{
		if (_character.IsAlive && _isActive)
			Execute();
	}

	public virtual void Execute()
	{
		_movement.TryMove(CurrentHorizontalDirection);

		if (_movement.EncounteredVerticalEdge) {
			_movement.FlipHorizontally();
		}
	}

	private void OnControllerCollider(RaycastHit2D hit)
	{
		// Colliding with wall.
		if ((hit.normal.x >= 0.5f || hit.normal.x <= -0.5f)
		 && hit.transform.gameObject.layer == 0) {
			_movement.FlipHorizontally();
		}
	}
}
