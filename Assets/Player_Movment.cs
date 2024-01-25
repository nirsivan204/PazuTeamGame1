using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float _movmentSpeed;
    private Vector2 _movement;
    private float _jumpSpeed = 5f;
    private bool _jump;
    void Start()
    {
        _movement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal") * _movmentSpeed;
        if (_jump) // _jump == Input for jump TBA
        {
            _movement.y += _jumpSpeed;
        }
    }
}
