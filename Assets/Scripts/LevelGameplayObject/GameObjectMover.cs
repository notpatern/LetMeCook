
using UnityEngine;

public class GameObjectMover : TriggerEffectZone
{
    [SerializeField] GameObject objectToMove;
    [SerializeField] Transform newPosition;

    protected override void TriggerFunc(Collider other)
    {
        objectToMove.transform.position = newPosition.position;
    }
}
