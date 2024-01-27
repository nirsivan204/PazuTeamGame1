using Legs;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegsPlayerMovment : AbstractPlayerMovement, IStunnable
{
    [SerializeField] private float _movmentSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 15f;

    public static LegsPlayerMovment Instance { get; private set; }

    private LevelAnimator _levelAnimator;
    private HumanPlayer _controller1;
    private HumanPlayer _controller2;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    private bool _isWalking = false;
    private bool _isBeingCheerd = false;
    private bool _isCheering = false;
    private bool _isStunned = false;
    private bool _isObjectBeingPulled = false;
    private bool _isJumping = false;

    public int StunLevel = 0;
    public float _jumpCount = 0;
    private int _dirX;

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
        _levelAnimator = GetComponentInChildren<LevelAnimator>();
    }

    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        _controller1 = controller1;
        _controller2 = controller2;
        _controller2.OnXPress.AddListener(CheckJump);
        _controller2.OnTrianglePress.AddListener(Cheer);

        _controller2.OnCirclePress.AddListener(OnCirclePress);
        _controller2.OnSquarePress.AddListener(OnSquarePress);
    }

    private void Start()
    {
        _levelAnimator.PlayIdleAnimation();

        if (TopPlayerController.Instance != null)
        {
            TopPlayerController.Instance.OnCheerAction += () =>  _isBeingCheerd = true;
            TopPlayerController.Instance.OnCheerEndAction += () => _isBeingCheerd = false;
        }
    }

    private void Update()
    {
        if (_isStunned)
            return;

        if (_isCheering)
            return;

        if (_controller1 != null)
        {
            CheckDirection(_controller1.movementXLeft);

            if (_controller1.movementXLeft > -0.25 && _controller1.movementXLeft < 0.25)
            {
                _controller1.movementXLeft = 0;
                _isWalking = false;
            }
            else
            {
                _isWalking = true;
            }

            PlayWalkingAnimation();
            _rb.velocity = new Vector2(_controller1.movementXLeft * _movmentSpeed, _rb.velocity.y);
        }
    }

    private void LateUpdate()
    {
        if (_isStunned)
            return;

        if (_rb.velocity.y > 0 && _levelAnimator.GetAnimationName() != "Jump_Cycle_Up")
        {
            _isJumping = true;
            _levelAnimator.SetAddAnimation("Jump_Cycle_Up", false, 0, false);
        }
        else if (_rb.velocity.y < 0 && _levelAnimator.GetAnimationName() != "Jump_Cycle_Down")
        {
            _levelAnimator.SetAddAnimation("Jump_Cycle_Down", false, 0, false);
            _rb.gravityScale = 5;
        }
        else
        {
            _isJumping = false;
            _rb.gravityScale = 1;
        }
    }

    private void PlayWalkingAnimation()
    {
        if (_isJumping || _isStunned || _isCheering)
            return;

        if (_isWalking && _levelAnimator.GetAnimationName()!= "Walking_Loop_Full")
        {
            _levelAnimator.SetAddAnimation("Walking_Loop_Full", true, 0, false);
        }
        else if(!_isWalking && _levelAnimator.GetAnimationName() != "Idle")
        {
            _levelAnimator.PlayIdleAnimation();
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
            if (_isObjectBeingPulled)
                return;

            int direction = movement > 0 ? 1 : -1;
            if (direction != _dirX)
            {
                _dirX = direction;  
                Vector3 scale = gameObject.transform.lossyScale;
                scale.x = -(_dirX);
                gameObject.transform.localScale = scale;
            }
        }
    }

    public void Rumble()
    {
        RumbleManager.RumblePulse(true,0,0.25f,1);
    }

    public void StopRumble()
    {
        RumbleManager.RumblePulse(false, 0, 0.25f, 1);
    }

    private void CheckJump()
    {
        if (_isCheering || _isStunned)
            return;

        if (_jumpCount == 0 || (_jumpCount == 1 && _isBeingCheerd))
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

        _isCheering = true;
        OnCheerAction?.Invoke();
        _levelAnimator.SetAddAnimation("Cheer", true, 0, false);

        this.SetTimer(_cheerTime, () =>
        {
            _isCheering = false;
            OnCheerEndAction?.Invoke();
            _levelAnimator.PlayIdleAnimation();
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _jumpCount = 0;
        }
        //if(collision.gameObject.CompareTag("MovingObject"))
        //{
        //    _movingObject = collision.gameObject;
        //}
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("MovingObject"))
    //    {
    //        _movingObject = null;
    //    }
    //}

    public void Stun(int stunAmount = 10)
    {
        _isStunned = true;
        StunLevel = stunAmount;
    }

    private void OnCirclePress()
    {
        if (!_isStunned)
            return;

        if (StunLevel % 2 == 0)
        {
            StunLevel = 10;
            return;
        }

        StunLevel--;
        if (StunLevel == 0)
            _isStunned = false;
    }

    private void OnSquarePress()
    {
        if (!_isStunned)
            return;

        if (StunLevel % 2 == 1)
        {
            StunLevel = 10;
            return;
        }

        StunLevel--;
        if (StunLevel == 0)
            _isStunned = false;
    }

    public void OnStun(int stunAmount)
    {
        _isStunned = true;
        _levelAnimator.SetAddAnimation("Stun", true, 0, false);
    }
}