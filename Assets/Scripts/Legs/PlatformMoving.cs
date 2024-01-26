using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legs
{
    public class PlatformMoving : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _distance = 10;

        private void Start()
        {
            LeanTween.moveX(gameObject, gameObject.transform.position.x + _distance, _speed).setLoopPingPong();
        }
    }
}