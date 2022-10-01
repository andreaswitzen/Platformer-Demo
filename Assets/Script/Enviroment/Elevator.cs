using System.Collections.Generic;
using UnityEngine;

// An elevator. Does what elevators do.
public class Elevator : Activatable
{
	[SerializeField] private ElevatorDragZone _dragZone;
	[SerializeField] private Transform _platform;
	[SerializeField] private Transform _bottomPosition;
	[SerializeField] private Transform _topPosition;
	[SerializeField] private List<Door> _topDoors;
	[SerializeField] private List<Door> _bottomDoors;
	[SerializeField] private bool _goingDown;
	[SerializeField] private float _speed;

	private bool _isActive;

	public void Update()
	{
		if (!_isActive)
			return;

		var direction = _goingDown ? Vector3.down : Vector3.up;

		_platform.Translate(_speed * Time.deltaTime * direction);

		foreach (var transform in _dragZone.TransformsOnElevator) {
			if (transform && transform != _platform) {
				transform.Translate(_speed * Time.deltaTime * direction, Space.World);
			}
		}

		if (_platform.position.y <= _bottomPosition.position.y
		 || _platform.position.y >= _topPosition.position.y) {
			Deactivate();
		}
	}

	public override void Activate()
	{
		_isActive = true;
		CloseAllDoors();
		WasActivated.Invoke();
	}

	public override void Deactivate()
	{
		_isActive = false;
		_goingDown = !_goingDown;
		CorrectPosition();

		// Elevator is at top position.
		if (_goingDown) {
			OpenTopDoors();
		}
		else {
			OpenBottomDoors();
		}

		WasDeactivated.Invoke();
	}

	private void CorrectPosition()
	{
		var diff = Vector3.zero;

		if (_platform.position.y < _bottomPosition.position.y) {
			diff = _bottomPosition.position - _platform.position;
			_platform.position = new Vector3(_platform.position.x, _bottomPosition.position.y, 0);
		}
		else if (_platform.position.y > _topPosition.position.y) {
			diff = _topPosition.position - _platform.position;
			_platform.position = new Vector3(_platform.position.x, _topPosition.position.y, 0);
		}

		foreach (var transform in _dragZone.TransformsOnElevator) {
			if (transform && transform != _platform)
				transform.position += diff;
		}
	}

	private void CloseAllDoors()
	{
		foreach (var door in _topDoors) {
			door.Close();
		}
		foreach (var door in _bottomDoors) {
			door.Close();
		}
	}

	private void OpenTopDoors()
	{
		foreach (var door in _topDoors) {
			door.Open();
		}
	}

	private void OpenBottomDoors()
	{
		foreach (var door in _bottomDoors) {
			door.Open();
		}
	}
}
