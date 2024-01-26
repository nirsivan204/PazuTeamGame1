using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsPlayerMovment : AbstractPlayerMovement
{
    [SerializeField] private float _movmentSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 15f;
    [SerializeField] private RumbleManager rumbleManager;

    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    private Vector2 _movement;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init legs");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller1.OnLeftAnalogMove.AddListener(MoveLeftStick);
        _controller1.OnXPress.AddListener(Jump);
        _controller2.OnRightAnalogMove.AddListener(MoveRightStick);
    }

    public void MoveLeftStick(Vector2 movement)
    {
        print("legsLeft");
        _movement.y = movement.y * _movmentSpeed;
        _rb.velocity = _movement;
    }
    public void MoveRightStick(Vector2 movement)
    {
        print("legsRight");
        _movement.x = movement.x * _movmentSpeed;
        _rb.velocity = _movement;
    }

    private void Jump()
    {
        _movement.y = _jumpSpeed;
        _rb.velocity = _movement;
    }
}
