using Legs;
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

    private bool _isBeingCheerd = true;
    private bool _isGrounded = false;
    private bool _isDoubleJump = false;
    private bool _isCheering = false; 

    public static LegsPlayerMovment Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();
    }


    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init legs");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller1.OnLeftAnalogMove.AddListener(MoveLeftStick);
        _controller2.OnXPress.AddListener(Jump);
        //_controller2.OnRightAnalogMove.AddListener(MoveRightStick);
        _controller2.OnTrianglePress.AddListener(Cheer);
        _controller1.OnTrianglePress.AddListener(Cheer); // OtherPlayer
    }

    public void MoveLeftStick(Vector2 movement)
    {
        //print("legsLeft");
        _movement.x = movement.x * _movmentSpeed;
        _movement.y = movement.y * _movmentSpeed;

        _rb.velocity = _movement;
    }
    public void MoveRightStick(Vector2 movement)
    {
        //print("legsRight");
    }

    private void Jump()
    {
        _movement.y = _jumpSpeed;
        _rb.velocity = _movement;
    }
    private void Cheer()
    {
        _isCheering = true;
        LeanTween.delayedCall(gameObject, 2f, () => _isCheering = false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            _isGrounded = true;
            Platform platform = collision.gameObject.GetComponent<Platform>();
            transform.parent = platform.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            _isGrounded = false;
            transform.parent = null;
        }
    }
}
