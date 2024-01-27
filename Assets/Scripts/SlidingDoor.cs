using System;
using System.Collections;
using UniRx;
using UnityEngine;

public enum Door
{
    HandsDoor1,
    HandsDoor2,
    HandsDoor3,
    HandsDoor4,
    HandsDoor5,
    HandsDoor6,
    HandsDoor7,
    LegsDoor1,
    LegsDoor2,
    LegsDoor3,
    LegsDoor4,
    LegsDoor5,
    LegsDoor6,
    LegsDoor7
}

public class DoorSwitchedEvent
{
    public bool OpenDoor;
    public Door DoorNumber;
}

public class SlidingDoor : MonoBehaviour
{
    private float _startPostionX;
    private IDisposable _doorListener;
    
    [SerializeField] private float _endPositionX;
    [SerializeField] private float _sliderDistancePerFrame;
    [SerializeField] private Door _door;
    
    // Start is called before the first frame update
    private void Start()
    {
        _startPostionX = transform.localPosition.x;
    }

    private void OnEnable()
    {
        _doorListener = MessageBroker.Default.Receive<DoorSwitchedEvent>().ObserveOnMainThread().Subscribe(OnToggleDoor);
    }

    private void OnDisable()
    {
        _doorListener.Dispose();
    }
    
    private void OnToggleDoor(DoorSwitchedEvent doorSwitchedEvent)
    {
        if (doorSwitchedEvent.DoorNumber != _door) return;
        
        if (doorSwitchedEvent.OpenDoor)
        {
            StopCoroutine(SlideDoor(_startPostionX));
            StartCoroutine(SlideDoor(_endPositionX));    
        }
        else
        {
            StopCoroutine(SlideDoor(_endPositionX));
            StartCoroutine(SlideDoor(_startPostionX));    
        }
    }

    private IEnumerator SlideDoor(float endPositionX)
    {
        var currentPosition = transform.localPosition;
        var slideDirection = currentPosition.x < endPositionX ? 1 : -1;

        while (currentPosition.x * slideDirection < endPositionX * slideDirection)
        {
            yield return new WaitForEndOfFrame();

            currentPosition.x += _sliderDistancePerFrame * slideDirection;
            transform.localPosition = currentPosition;
        }
    }
}

