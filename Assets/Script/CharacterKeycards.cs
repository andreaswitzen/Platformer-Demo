using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keycards carried by a character (currently only the player). Resets when a new level is loaded.
public class CharacterKeycards : MonoBehaviour
{
	private HashSet<KeycardType> _keycards;

	public void Awake()
	{
		_keycards = new HashSet<KeycardType>();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		_keycards.Clear();
	}

	public bool Contains(KeycardType card)
	{
		return _keycards.Contains(card);
	}

	public void Add(KeycardType card)
	{
		_keycards.Add(card);
	}

	public void Remove(KeycardType card)
	{
		_keycards.Remove(card);
	}
}
