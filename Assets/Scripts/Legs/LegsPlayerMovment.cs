using Legs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static playerController;


public class LegsPlayerMovment : AbstractPlayerMovement, IStunnable
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
    public int StunLevel = 0;
    private bool isStunned;

    public float _jumpCount = 0;
    private int _dirX;

    private float _cheerTime = 2f;
    public event Action OnCheerAction;
    public event Action OnCheerEndAction;

    private float _movmentX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();

        if (TopPlayerController.Instance != null)
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
        _controller2.OnXPress.AddListener(CheckJump);
        //_controller2.OnRightAnalogMove.AddListener(MoveRightStick);
        _controller2.OnTrianglePress.AddListener(Cheer);
        _controller2.OnCirclePress.AddListener(OnCirclePress);
        _controller2.OnSquarePress.AddListener(OnSquarePress);
    }

    private void Update()
    {
        if(_controller1 != null)
        {
            CheckDirection(_controller1.movementXLeft);

            if (_controller1.movementXLeft > -0.25 && _controller1.movementXLeft < 0.25)
                _controller1.movementXLeft = 0;

            if (_jumpCount > 0)
                return;

            _rb.velocity = new Vector2(_controller1.movementXLeft * _movmentSpeed, _rb.velocity.y);
        }
    }

    private void CheckDirection(float movement)
    {
        if (movement < 0.25 && movement > -0.25)
        {
            _dirX = 0;
        }
        else
        {
            _dirX = movement > 0 ? 1 : -1;
        }
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
        _movement.x = _dirX * _movmentSpeed;
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
        if (collision.gameObject.tag == "Platform")
        {
            _jumpCount = 0;
        }
    }

    public void Stun(int stunAmount = 10)
    {
        isStunned = true;
        StunLevel = stunAmount;
    }

    private void OnCirclePress()
    {
        if (!isStunned)
            return;

        if (StunLevel % 2 == 0)
        {
            StunLevel = 0;
            return;
        }

        StunLevel++;
    }

    private void OnSquarePress()
    {
        if (!isStunned)
            return;

        if (StunLevel % 2 == 1)
        {
            StunLevel = 0;
            return;
        }

        StunLevel++;
    }

    public void OnStun(int stunAmount)
    {
        throw new NotImplementedException();
    }
}