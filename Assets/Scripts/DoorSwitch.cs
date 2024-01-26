using UniRx;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] private Door _door;

    private void OnTriggerEnter2D(Collider2D other)
    {
        MessageBroker.Default.Publish(new DoorSwitchedEvent { DoorNumber = _door, OpenDoor = true });
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MessageBroker.Default.Publish(new DoorSwitchedEvent { DoorNumber = _door, OpenDoor = false });
    }
}
