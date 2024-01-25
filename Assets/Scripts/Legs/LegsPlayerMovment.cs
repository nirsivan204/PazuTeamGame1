using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsPlayerMovment : MonoBehaviour
{
    [SerializeField] private float _movmentSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 15f;

    private HumanPlayer _humanPlayer;

    private Vector2 _movement;
    private Rigidbody2D _rb;


    private void Awake()
    {
        _humanPlayer = GetComponent<HumanPlayer>();
        _humanPlayer.OnLeftAnalogMove.AddListener(Move);
        _humanPlayer.OnXPress.AddListener(Jump);
    }
    void Start()
    {
        _movement = Vector2.zero;
    }

    private void Move(Vector2 movement)
    {
        print(movement);
        _movement.x = movement.x * _movmentSpeed;
        _rb.velocity = _movement;

    }
    private void Jump()
    {
        _movement.y = _jumpSpeed;
        _rb.velocity = _movement;
    }
}
