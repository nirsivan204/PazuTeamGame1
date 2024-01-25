using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HumanPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;

    public UnityEvent OnXPress;
    public UnityEvent OnCirclePress;
    public UnityEvent OnSquarePress;
    public UnityEvent OnTrianglePress;
    public UnityEvent<Vector2>  OnRightAnalogMove;
    public UnityEvent<Vector2> OnLeftAnalogMove;


    public float movementXRight;
    public float movementYRight;
    public float movementXLeft;
    public float movementyLeft;
    

    public void init(InputMGR inputMGR)
    {
        inputMGR.ConnectToAllPlayerControllers(playerIndex, OnMoveRight, OnMoveLeft, OnFire,OnCircle,OnTriangle,OnSquare, OnMoveRight);
    }

    private void OnMoveLeft(Vector2 movementVector)
    {
        movementXLeft = movementVector.x;
        movementyLeft = movementVector.y;
        OnLeftAnalogMove.Invoke(movementVector);
    }

    private void OnMoveRight(Vector2 movementVector)
    {
        movementXRight = movementVector.x;
        movementYRight = movementVector.y;
        OnRightAnalogMove.Invoke(movementVector);
    }
    public void OnFire()
    {
        OnXPress.Invoke();
    }

    public void OnCircle()
    {
        OnCirclePress.Invoke();
    }
    public void OnTriangle()
    {
        OnTrianglePress.Invoke();

    }
    public void OnSquare()
    {
        OnSquarePress.Invoke();

    }
}
