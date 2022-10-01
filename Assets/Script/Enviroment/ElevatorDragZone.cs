using System.Collections.Generic;
using UnityEngine;

// Belongs to an elevator. Anything colliding with this will be "dragged" by the elevator when it moves.
public class ElevatorDragZone : MonoBehaviour {

    public List<Transform> TransformsOnElevator = new List<Transform>();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        TransformsOnElevator.Add(collision.transform);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Collider2D collider) && collider.isActiveAndEnabled)
            TransformsOnElevator.Remove(collision.transform);
    }
}
