using UnityEngine;
using Prime31;
using UnityEngine.Events;
using static MoveState;
using static Services;

// Handles character movement (with the use of a CharacterController2D).
public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private float _gravity;
	[SerializeField] private float _runSpeed;
	[SerializeField] private float _groundDamping;
	[SerializeField] private float _inAirDamping;
	[SerializeField] private float _jumpHeight;
	[SerializeField] private float _bounceHeight;
	[SerializeField] private float _jumpAbortStrength;
	[SerializeField] private int _maxCoyoteBuffer;
	[SerializeField] private AudioClip _jump;
	[SerializeField] private AudioClip _landing;
	[SerializeField] private float _takeDamageShakeAmount;
	[SerializeField] private float _headBounceShakeAmount;
	[SerializeField] private int meleeDamage;
	[SerializeField] private int headJumpDamage;

	private const float _fallThresholdSpeed = -0.1f;

	private Character _character;
	private Animator _animator;
	private AudioSource _audioSource;
	private Vector3 _velocity;
	private float _normalizedHorizontalSpeed = 0;
	private int _coyoteBuffer;
	private bool _isOnVerticalEdge;
	private bool _wasOnVerticalEdgeLastFrame;

	[HideInInspector] public UnityEvent CharacterDucked;
	[HideInInspector] public UnityEvent CharacterRose;
	[HideInInspector] public UnityEvent WasFrozen;
	[HideInInspector] public UnityEvent WasUnfrozen;

	public MoveState State { get; private set; }
	public CharacterController2D Controller { get; private set; }
	public bool EncounteredVerticalEdge => _isOnVerticalEdge && !_wasOnVerticalEdgeLastFrame;

	public void Awake()
	{
		Controller = GetComponent<CharacterController2D>();
		_character = GetComponent<Character>();
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}

	public void Update()
	{
		if (State == Frozen)
			return;

		UpdateState();
		UpdateCoyoteBuffer();

		var damping = Controller.isGrounded ? _groundDamping : _inAirDamping;
		_velocity.x = Mathf.Lerp(_velocity.x, _normalizedHorizontalSpeed * _runSpeed, Time.deltaTime * damping);

		_velocity.y += _gravity * Time.deltaTime;

		Controller.move(_velocity * Time.deltaTime);

		_velocity = Controller.velocity;

		_normalizedHorizontalSpeed = 0;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		var other = collision.gameObject.GetComponent<Character>();

		if (_character.IsPlayer) {
			// Jumping on head.
			if (CollidingFromAbove(collision) && other && !other.IsInvulnerable) {
				other.TakeDamage(headJumpDamage);
				Bounce();
				SCREEN_SHAKER.Shake(_headBounceShakeAmount);
			}
			// Colliding with enemy horizontally.
			else if (CollidingHorizontally(collision) && other && !_character.IsInvulnerable) {
				_character.TakeDamage(meleeDamage);
				SCREEN_SHAKER.Shake(_takeDamageShakeAmount);
			}
		}
	}

	public void TryMove(float horizontalDirection)
	{
		if (State == Frozen || State == Ducking)
			return;

		_normalizedHorizontalSpeed = horizontalDirection;

		if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(horizontalDirection))
			FlipHorizontally();

		if (Controller.isGrounded) {
			State = Running;
			_animator.TryPlay("Run");
		}
	}

	public void Stop()
	{
		_normalizedHorizontalSpeed = 0;
	}

	public void Freeze()
	{
		State = Frozen;
		WasFrozen.Invoke();
	}

	public void Unfreeze()
	{
		State = Idle;
		WasUnfrozen.Invoke();
	}

	public void FlipHorizontally()
	{
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	public void TryJump()
	{
		if (State == Jumping || State == Frozen || State == Ducking || _coyoteBuffer <= 0)
			return;

		State = Jumping;
		_velocity.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
		_coyoteBuffer = 0;
		_audioSource.TryPlay(_jump);
		_animator.TryPlay("Jump");
	}

	public void TryAbortJump()
	{
		if (State != Jumping || Controller.isGrounded)
			return;

		_velocity.y -= _velocity.y / _jumpAbortStrength;
	}

	public void Bounce()
	{
		_velocity.y = Mathf.Sqrt(2f * _bounceHeight * -_gravity);
		_animator.TryPlay("Jump");
	}

	public void TryDuck()
	{
		if (State == Frozen || !Controller.isGrounded)
			return;

		State = Ducking;
		_animator.TryPlay("Duck");
		CharacterDucked.Invoke();
	}

	public void TryRise()
	{
		if (State != Ducking)
			return;

		State = Idle;
		_animator.TryPlay("Idle");
		CharacterRose.Invoke();
	}

	// Respond to changes to state that has happened since the previous frame.
	private void UpdateState()
	{
		if (!_character.IsAlive)
			return;

		// Falling.
		if (!Controller.isGrounded && Controller.velocity.y < _fallThresholdSpeed) {
			State = Falling;
			_animator.TryPlay("Fall");
		}
		// Became grounded (landed).
		else if (Controller.collisionState.becameGroundedThisFrame) {
			State = Idle;
			_animator.TryPlay("Idle");
			_audioSource.TryPlay(_landing);
		}
		// Stopped running;
		else if (State == Running && _normalizedHorizontalSpeed == 0) {
			State = Idle;
			_animator.TryPlay("Idle");
		}

		// Check if character is standing on a vertical edge/ledge (save data from previous frame first).
		// Used for turning AI characters around instead of having them fall off.
		_wasOnVerticalEdgeLastFrame = _isOnVerticalEdge;
		_isOnVerticalEdge = IsOnVerticalEdge();
	}

	private void UpdateCoyoteBuffer()
	{
		if (Controller.isGrounded) {
			_coyoteBuffer = _maxCoyoteBuffer;
		}
		else if (!Controller.isGrounded && _coyoteBuffer > 0) {
			_coyoteBuffer--;
		}
	}

	private bool CollidingFromAbove(Collision2D collision)
	{
		var contactPoint = collision.contacts[0];
		return contactPoint.normal.y >= 0.25f;
	}

	private bool CollidingHorizontally(Collision2D collision)
	{
		var contactPoint = collision.contacts[0];
		return contactPoint.normal.x >= 0.5f || contactPoint.normal.x <= -0.5f;
	}

	private bool IsOnVerticalEdge()
	{
		if (!Controller.isGrounded)
			return false;

		var mask = Controller.platformMask;
		var bounds = Controller.boxCollider.bounds;
		var bottomLeft = bounds.min;
		var bottomRight = new Vector2(bounds.max.x, bounds.min.y);

		var _raycastHitBottomLeft = Physics2D.Raycast(bottomLeft, Vector2.down, 0.25f, mask);
		var _raycastHitBottomRight = Physics2D.Raycast(bottomRight, Vector2.down, 0.25f, mask);

		return !_raycastHitBottomLeft.collider || !_raycastHitBottomRight.collider;
	}
}
