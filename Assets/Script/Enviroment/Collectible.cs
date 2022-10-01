using System.Collections;
using UnityEngine;

// Something which can be collected by a character (normally only the player character).
public abstract class Collectible : MonoBehaviour
{
    protected bool _isCollected;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;

    private readonly WaitForSeconds _destroyDelay = new WaitForSeconds(2);

	public abstract void TryCollect(Character collector);

    public void OnTriggerEnter2D(Collider2D collision)
	{
        if (!_isCollected && collision.TryGetComponent(out Character character))
            TryCollect(character);
	}

    protected void Consume()
    {
        _isCollected = true;
        _audioSource.Play();
        _spriteRenderer.enabled = false;
        _boxCollider.enabled = false;
        StartCoroutine(DelayedDestroy());
    }

    protected IEnumerator DelayedDestroy()
    {
        yield return _destroyDelay;
        Destroy(gameObject);
    }
}
