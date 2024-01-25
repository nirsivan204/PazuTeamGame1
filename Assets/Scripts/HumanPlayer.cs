using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    float movementXRight;
    float movementYRight;
    float movementXLeft;
    float movementyLeft;

    public void init(InputMGR inputMGR)
    {
        inputMGR.ConnectToAllPlayerControllers(playerIndex, OnMoveRight, OnMoveLeft, OnFire,OnCircle,OnTriangle,OnSquare, OnMoveRight);
    }

    private void OnMoveLeft(Vector2 movementVector)
    {
        movementXLeft = movementVector.x;
        movementyLeft = movementVector.y;
    }

    private void OnMoveRight(Vector2 movementVector)
    {
        movementXRight = movementVector.x;
        movementYRight = movementVector.y;
    }
    public void OnFire()
    {
    }

    public void OnCircle()
    {

    }
    public void OnTriangle()
    {

    }
    public void OnSquare()
    {
    }
}
