using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPlayerController : AbstractPlayerMovement
{
    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    private float _movementXLeft;
    private float _movementYLeft;
    private float _movementXRight;
    private float _movementYRight;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init Hands");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller1.OnRightAnalogMove.AddListener(MoveRightStick);
        _controller2.OnLeftAnalogMove.AddListener(MoveLeftStick);
    }

    public void MoveLeftStick(Vector2 movement)
    {
        print("topLeft");
        _movementXLeft = movement.x;
        _movementYLeft = movement.y;
    }
    public void MoveRightStick(Vector2 movement)
    {
        _movementXRight = movement.x;
        _movementYRight = movement.y;
    }

    private void FixedUpdate()
    {
        if (_movementXRight < -0.25f)
        {
            _rb.velocity = Vector2.left;
            Debug.Log("Left");
        }
        if (_movementXRight > 0.25f)
        {
            _rb.velocity = Vector2.right;
            Debug.Log("Right");
        }
        if (_movementYLeft > 0.25f)
        {
            Debug.Log("Up");
            _rb.velocity = Vector2.up;
        }
        if (_movementYLeft < -0.25f)
        {
            Debug.Log("Down");
            _rb.velocity = Vector2.down;
        }
    }
}
