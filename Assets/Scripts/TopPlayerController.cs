using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPlayerController : AbstractPlayerMovement
{
    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    private float _movementXLeft;
    private float _movementYLeft;
    private float x;
    private float y;

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
        //_controller1.OnLeftAnalogMove.AddListener(MoveRightStick);
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
        //_movementXRight = movement.x;
        //_movementYRight = movement.y;
    }

    private void FixedUpdate()
    {
        x = 0;
        y = 0;

        if (_movementXLeft < -0.25f)
        {
            //_rb.velocity = Vector2.left;
            x--;
        }
        if (_movementXLeft > 0.25f)
        {
            //_rb.velocity = Vector2.right;
            x++;
        }
        if (_movementYLeft > 0.25f)
        {
            //_rb.velocity = Vector2.up;
            y++;
        }
        if (_movementYLeft < -0.25f)
        {
            //_rb.velocity = Vector2.down;
            y--;
        }

        Vector2 movement = new Vector2(x, y).normalized;
        _rb.velocity = movement;
    }
}
