using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixedRotation : MonoBehaviour
{
    public float XMin;
    public float XMax;
    public float YMin;
    public float YMax;

    void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        Debug.Log(transform.position);
        float targetX = Mathf.Clamp(transform.position.x, XMin, XMax);
        float targetY = Mathf.Clamp(transform.position.y, YMin, YMax);
        transform.position = new Vector3(targetX, targetY, -10);

        Debug.Log(targetX);
    }
}
