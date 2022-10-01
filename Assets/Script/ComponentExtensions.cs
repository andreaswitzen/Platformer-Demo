using UnityEngine;

// Extends behavior of built-in components.
public static class ComponentExtensions
{
	// Tries to play an animator animation by name.
	// Does nothing if already playing or if animator lacks animation with matching name.
	public static void TryPlay(this Animator animator, string animationName, int layerIndex = 0)
	{
		var id = Animator.StringToHash(animationName);
		var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

		if (id == stateInfo.shortNameHash
		 || !animator.HasState(layerIndex, id)) {
			return;
		}

		animator.Play(id);
	}

	// Tries to play an audio clip, does nothing if reference is null.
	public static void TryPlay(this AudioSource audioSource, AudioClip audioClip)
	{
		if (audioClip == null)
			return;

		audioSource.PlayOneShot(audioClip);
	}
}
