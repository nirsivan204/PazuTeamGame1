using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDividerRipple_2 : MonoBehaviour
{
    void Start()
    {
        LeanTween.scaleY(gameObject, 1.05f, 11).setLoopPingPong();
    }
}
