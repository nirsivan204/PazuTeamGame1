using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HumanPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] float low;
    [SerializeField] float high;

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

    public void initController(InputMGR inputMGR)
    {
        inputMGR.ConnectToAllPlayerControllers(playerIndex, OnMoveRight, OnMoveLeft, OnFire,OnCircle,OnTriangle,OnSquare, OnMoveRight);
        //inputMGR.ConnectToGamePadPlayerController(playerIndex, OnMoveRight, OnMoveLeft, OnFire,OnCircle,OnTriangle,OnSquare, OnMoveRight);
    }
    public void initKeyboard(InputMGR inputMGR)
    {
        /*        inputMGR.ConnectToKeyboardPlayerController(playerIndex, OnMoveRight, OnMoveLeft, OnFire, null);
        */
        inputMGR.ConnectToKeyboardPlayerController(playerIndex, OnMoveRight, OnMoveLeft, OnFire, OnCircle, OnTriangle, OnSquare);
    }


    private void OnMoveLeft(Vector2 movementVector)
    {
        //print("move left " + movementVector);
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
        //print(name + "fire");
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

    public void StartRumble()
    {
        RumbleManager.RumblePulse(true, playerIndex, low, high);
    }
    public void StopRumble()
    {
        RumbleManager.RumblePulse(false, playerIndex, 0, 0);
    }

}
