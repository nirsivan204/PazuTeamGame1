using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPlayerController : MonoBehaviour
{
    public HumanPlayer Controller_1;
    public HumanPlayer Controller_2;

    private void FixedUpdate()
    {
        if (Controller_1.movementXLeft < -0.25f)
        {
            Debug.Log("Left");
        }
        if (Controller_1.movementXLeft > 0.25f)
        {
            Debug.Log("Right");
        }
        if (Controller_1.movementyLeft > 0.25f)
        {
            Debug.Log("Up");
        }
        if (Controller_1.movementyLeft < -0.25f)
        {
            Debug.Log("Down");
        }
    }
}
