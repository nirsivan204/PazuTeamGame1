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

    private float _speed;

    Rigidbody2D _rb;
    public float _regulatSpeed = 1;
    public float _dashSpeed = 3;
    public float _dashTime = 1f;
    public bool isDashing;
    public bool isDoubleDashing;

    public bool isCheering;
    public float _cheerTime = 2;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _speed = _regulatSpeed;
    }

    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init Hands");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller2.OnLeftAnalogMove.AddListener(MoveLeftStick);
    }

    public void MoveLeftStick(Vector2 movement)
    {
        _movementXLeft = movement.x;
        _movementYLeft = movement.y;
    }
    public void Dash()
    {
        if (isCheering)
            return;

        if (!isDashing && !isDoubleDashing)
        {
            isDashing = true;
            _speed = _dashSpeed;

            this.SetTimer(_dashTime, () =>
            {
                if (!isDoubleDashing)
                {
                    isDashing = false;
                    _speed = _regulatSpeed;
                }
            });
        }
        else if(!isDoubleDashing)
        {
            isDoubleDashing = true;
            _speed = _dashSpeed * 1.5f;

            this.SetTimer(_dashTime, () =>
            {
                isDashing = false;
                isDoubleDashing = false;
                _speed = _regulatSpeed;
            });
        }
    }

    public void Cheer()
    {
        Debug.Log("Cheer!");
        if (isDashing || isDoubleDashing)
            return;

        isCheering = true;
        this.SetTimer(_cheerTime, () =>
        {
            isCheering = false;
            _speed = _regulatSpeed;
        });
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isDoubleDashing && !isCheering)
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
        }

        Vector2 movement = new Vector2(x, y).normalized;
        _rb.velocity = movement * _speed;
    }
}
