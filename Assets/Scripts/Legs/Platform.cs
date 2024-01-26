using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Legs
{
    public class Platform : MonoBehaviour
    {
        private void OnDoorOpen(int direction, float distance, float speed)
        {
            LeanTween.moveX(gameObject, direction * (Mathf.Abs(gameObject.transform.position.x) + distance), speed);
        }

    }
}