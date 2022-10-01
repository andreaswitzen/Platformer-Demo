using UnityEngine;

// Plays a song on enable.
public class PlaySongOnStart : Activatable
{
	[SerializeField] private AudioClip _song;

	public void Start()
	{
		Services.MUSIC_PLAYER.Play(_song);
	}

	public override void Activate()
	{
		enabled = true;
	}
}
