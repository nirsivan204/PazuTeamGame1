using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class playerController : MonoBehaviour
{
    private bool canLeave = true;
    public UnityEvent fireEvent = new UnityEvent();
    public UnityEvent squareEvent = new UnityEvent();
    public UnityEvent triangleEvent = new UnityEvent();
    public UnityEvent circleEvent = new UnityEvent();
    public UnityEvent R1Event = new UnityEvent();
    public UnityEvent L1Event = new UnityEvent();
    public UnityEvent mainManuEvent = new UnityEvent();
    public class MoveEvent : UnityEvent<Vector2>
    {
    }
    public MoveEvent moveLeftEvent = new MoveEvent();
    public MoveEvent moveRightEvent = new MoveEvent();
    public MoveEvent digitalMoveEvent = new MoveEvent();


    public bool CanLeave { get => canLeave; set => canLeave = value; }

    public void OnFire()
    {
        fireEvent.Invoke();
    }

    public void OnMoveLeft(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>().normalized;
        moveLeftEvent.Invoke(movementVector);
       //print(name + " " + movementVector);
    }

    public void OnMoveRight(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>().normalized;
        moveRightEvent.Invoke(movementVector);
        //print(name + " " + movementVector);
    }


    public void OnDigitalMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>().normalized;
        digitalMoveEvent.Invoke(movementVector);
    }

    public void OnCircle()
    {
        circleEvent.Invoke();
    }

    public void OnR1()
    {
        R1Event.Invoke();
    }
    public void OnL1()
    {
        L1Event.Invoke();
    }

    private void leave()
    {
/*        moveEvent.RemoveAllListeners();
        fireEvent.RemoveAllListeners();*/
        Destroy(gameObject);
    }

    public void OnSquare()
    {
        squareEvent.Invoke();
    }
    public void OnTriangle()
    {
        triangleEvent.Invoke();
    }
}
