using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Legs
{
    public class PlatformMoving : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _distance = 10;

        private float _startingPosition;

        private void Start()
        {
            _startingPosition = transform.position.x;
        }

        private void Update()
        {
            transform.position = new Vector3(_startingPosition + Mathf.PingPong(Time.time * _speed, _distance) - _distance / 2f, transform.position.y, transform.position.z);
        }
    }
}