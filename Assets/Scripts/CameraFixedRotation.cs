using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixedRotation : MonoBehaviour
{
    public float XMin;
    public float XMax;
    public float YMin;
    public float YMax;

    public Chaser chaser;

    void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        if (chaser == null) return;

        float targetX = Mathf.Clamp(transform.position.x, XMin, XMax);
        float targetY = Mathf.Clamp(transform.position.y, Mathf.Max(YMin, chaser.transform.position.y + 7), YMax);
        transform.position = new Vector3(targetX, targetY, -10);

    }
}
