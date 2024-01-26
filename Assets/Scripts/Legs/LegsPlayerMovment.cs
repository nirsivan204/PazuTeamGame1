using Legs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsPlayerMovment : AbstractPlayerMovement
{
    [SerializeField] private float _movmentSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 15f;
    [SerializeField] private RumbleManager rumbleManager;
    public static LegsPlayerMovment Instance { get; private set; }

    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    private Vector2 _movement;
    private Rigidbody2D _rb;

    private bool _isBeingCheerd = true;
    private bool _isCheering = false;

    private float _jumpCount = 0;

    private float _cheerTime = 2f;
    public event Action OnCheerAction;
    public event Action OnCheerEndAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();

        if(TopPlayerController.Instance != null)
        {
            TopPlayerController.Instance.OnCheerAction += () => _isBeingCheerd = true;
            TopPlayerController.Instance.OnCheerEndAction += () => _isBeingCheerd = false;
        }
    }


    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init legs");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller1.OnLeftAnalogMove.AddListener(MoveLeftStick);
        _controller2.OnXPress.AddListener(CheckJump);
        //_controller2.OnRightAnalogMove.AddListener(MoveRightStick);
        _controller2.OnTrianglePress.AddListener(Cheer);
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

    private void CheckJump()
    {
        if (_jumpCount == 0 || (_jumpCount == 1 & _isBeingCheerd))
        {
            Jump();            
        }
    }

    private void Jump()
    {
        _movement.y = _jumpSpeed;
        _rb.velocity = _movement;
        _jumpCount++;
    }


    private void Cheer()
    {
        if (_jumpCount != 0)
            return;

        Debug.Log("Cheer!");

        _isCheering = true;
        OnCheerAction?.Invoke();
        this.SetTimer(_cheerTime, () =>
        {
            _isCheering = false;
            OnCheerEndAction?.Invoke();
        });
    }    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            _jumpCount = 0;
            Platform platform = collision.gameObject.GetComponent<Platform>();
            transform.parent = platform.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
    }
}
