using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Services;

// Plays music.
public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private float _fadeSeconds;

	private AudioSource _audioSource;

	private bool _isFading;

	public void Awake()
	{
		if (!MUSIC_PLAYER) {
			MUSIC_PLAYER = this;
			_audioSource = GetComponent<AudioSource>();
			SceneManager.sceneUnloaded += _ => FadeOut();
		}
	}

	private void OnDestroy()
	{
		if (MUSIC_PLAYER && Equals(MUSIC_PLAYER)) {
			SceneManager.sceneUnloaded -= _ => FadeOut();
		}
	}

	public void Play(AudioClip song)
	{
		if (!song)
			return;

		StartCoroutine(PlayOrQueue(song));
	}

	public void FadeOut()
	{
		if (!_audioSource.isPlaying || _isFading)
			return;

		StartCoroutine(Fade(_fadeSeconds, 0));
	}

	private IEnumerator Fade(float fadeSeconds, float targetVolume)
	{
		_isFading = true;

		float currentTime = 0;
		float start = _audioSource.volume;

		while (currentTime < _fadeSeconds) {
			currentTime += Time.deltaTime;
			_audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / fadeSeconds);
			yield return null;
		}
		_audioSource.Stop();
		_isFading = false;
	}

	private IEnumerator PlayOrQueue(AudioClip song)
	{
		while (_audioSource.isPlaying)
			yield return null;

		_audioSource.volume = 1f;
		_audioSource.clip = song;
		_audioSource.Play();
	}
}
