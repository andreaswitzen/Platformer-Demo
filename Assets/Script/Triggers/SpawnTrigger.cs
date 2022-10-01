using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Services;

// A trigger that spawns enemies when entered.
public class SpawnTrigger : Trigger
{
	[SerializeField] private Transform _spawnPosition;
	[SerializeField] private float _secondsBetweenSpawns;
	[SerializeField] private bool _flipSpawnsToLeft;
	[SerializeField] private List<GameObject> _spawnObjects;

	private WaitForSeconds _spawnDelay;

	public override void Awake()
	{
		base.Awake();
		_spawnDelay = new WaitForSeconds(_secondsBetweenSpawns);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_isCoolingDown) {
			StartCoroutine(Spawn());

			if (_cooldownSeconds > 0)
				StartCoroutine(ActivateCooldown());

			if (_onlyTriggerOnce)
				Deactivate();
		}
	}

	private IEnumerator Spawn()
	{
		for (int i = 0; i < _spawnObjects.Count; i++) {
			var spawn = Instantiate(_spawnObjects[i], INSTANTIATED_OBJS_PARENT.transform);
			spawn.transform.position = _spawnPosition.position;

			if (_flipSpawnsToLeft)
				spawn.GetComponent<Character>().Movement.FlipHorizontally();

			yield return _spawnDelay;
		}
	}
}
