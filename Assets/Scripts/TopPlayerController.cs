using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopPlayerController : AbstractPlayerMovement, IStunnable
{
    public static TopPlayerController Instance { get; private set; }
    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    public LevelAnimator levelAnimator;

    private float _movementXLeft;
    private float _movementYLeft;
    private float x;
    private float y;

    private float _speed;

    Rigidbody2D _rb;
    public float _regulatSpeed = 1;
    public float _rotationSpeed = 1;
    public float _dashSpeed = 3;
    public float _doubleDashSpeed = 4;
    public float _dashTime = 1f;
    public bool isDashing;
    public bool isDoubleDashing;

    public bool isCheering;
    public float _cheerTime = 2;

    public int StunLevel = 0;
    private bool isStunned;
    private bool _leftStunButtonNeeded;
    private bool _rightStunButtonNeeded;

    private bool _isAboveGap;
    public float GapReactionTime = .1f;

    public event Action OnCheerAction;
    public event Action OnCheerEndAction;

    public bool _isBeingCheerd;

    public AudioSource AudioSource;
    public AudioClip[] DashSounds;
    public AudioClip[] ClapSounds;

    private bool _isDancing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();
        _speed = _regulatSpeed;
        if (LegsPlayerMovment.Instance != null)
        {
            LegsPlayerMovment.Instance.OnCheerAction += () => _isBeingCheerd = true;
            LegsPlayerMovment.Instance.OnCheerEndAction += () => _isBeingCheerd = false;
        }
    }

    bool isStop = false;

    public void stopMovment()
    {
        isStop = true;
    }

    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        print("init Hands");
        _controller1 = controller1;
        _controller2 = controller2;
        _controller2.OnLeftAnalogMove.AddListener(MoveLeftStick);
        _controller1.OnXPress.AddListener(Dash);
        _controller1.OnCirclePress.AddListener(RightStunButton);
        _controller1.OnSquarePress.AddListener(LeftStunButton);
        _controller1.OnTrianglePress.AddListener(Cheer);

        levelAnimator.PlayIdleAnimation();
    }

    public void MoveLeftStick(Vector2 movement)
    {
        _movementXLeft = movement.x;
        _movementYLeft = movement.y;
    }
    public void Dash()
    {
        if (isStunned || isCheering || isStop)
            return;

        _isDancing = false;

        if (!isDashing && !isDoubleDashing)
        {
            isDashing = true;
            _speed = _dashSpeed;

            int rand = Random.Range(0, DashSounds.Length);
            AudioSource.PlayOneShot(DashSounds[rand]);

            levelAnimator.SetAddAnimation("Dash", false, 0, false);

            this.SetTimer(_dashTime, () =>
            {
                if (!isDoubleDashing)
                {
                    isDashing = false;
                    _speed = _regulatSpeed;
                }
            });
        }
        else if(_isBeingCheerd && !isDoubleDashing)
        {
            isDoubleDashing = true;
            _speed = _doubleDashSpeed;
            int rand = Random.Range(0, DashSounds.Length);
            AudioSource.PlayOneShot(DashSounds[rand]);

            levelAnimator.SetAddAnimation("Dash", false, 0, false);

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
        if (isCheering || isStunned || isDashing || isDoubleDashing || isStop)
            return;

        isCheering = true;
        _isDancing = false;
        x = 0;
        y = 0;
        levelAnimator.SetAddAnimation("Cheer", false, 0, false);

        int rand = Random.Range(0, DashSounds.Length);
        AudioSource.PlayOneShot(ClapSounds[rand]);
        this.SetTimer(.4f, () =>
         {
             rand = Random.Range(0, DashSounds.Length);
             AudioSource.PlayOneShot(ClapSounds[rand]);
         });

        this.SetTimer(.8f, () =>
        {
            rand = Random.Range(0, DashSounds.Length);
            AudioSource.PlayOneShot(ClapSounds[rand]);
        });


        OnCheerAction?.Invoke();
        this.SetTimer(_cheerTime, () =>
        {
            isCheering = false;
            _speed = _regulatSpeed;
            OnCheerEndAction?.Invoke();
        });
    }

    public void OnStun(int stunAmount)
    {
        Stun(stunAmount);
        _isDancing = false;
    }

    public void Stun(int stunAmount = 10)
    {
        isStunned = true;
        x = 0;
        y = 0;
        levelAnimator.SetAddAnimation("Stunned", true, 0, false);
        StunLevel = stunAmount;
        _leftStunButtonNeeded = true;
        _rightStunButtonNeeded = false;
    }

    public void LeftStunButton()
    {
        if (_leftStunButtonNeeded)
        {
            StunLevel--;
            if (StunLevel <= 0)
            {
                isStunned = false;
                StunLevel = 0;
                _leftStunButtonNeeded = false;
                _rightStunButtonNeeded = false;
            }
            else
            {
                _leftStunButtonNeeded = false;
                _rightStunButtonNeeded = true;
            }
        }
        else if (!isCheering && !isStunned && !isDashing && !isDoubleDashing)
        {
            _isDancing = true;
            levelAnimator.SetAddAnimation("Dance", true, 0, false);
        }
    }

    public void RightStunButton()
    {
        if (_rightStunButtonNeeded)
        {
            StunLevel--;
            if (StunLevel <= 0)
            {
                isStunned = false;
                StunLevel = 0;
                _leftStunButtonNeeded = false;
                _rightStunButtonNeeded = false;
            }
            else
            {
                _leftStunButtonNeeded = true;
                _rightStunButtonNeeded = false;
            }
        }
        else if(!isCheering && !isStunned && !isDashing && !isDoubleDashing)
        {
            _isDancing = true;
            levelAnimator.SetAddAnimation("Dance", true, 0, false);
        }
    }

    public void OnGap()
    {
        _isAboveGap = true;
        this.SetTimer(GapReactionTime, () =>
         {
             if(_isAboveGap && (!isDashing || !isDoubleDashing))
             {
                 GameManager.Instance.onLose();
             }
         });
    }

    public void OffGap()
    {
        _isAboveGap = false;
    }

    private void FixedUpdate()
    {
        if (!isStunned && !isDashing && !isDoubleDashing && !isCheering && !isStop)
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
        float magnitue = movement.magnitude;
        if (magnitue == 0 && levelAnimator.GetAnimationName() != "Idle" && !isStunned && !isCheering && !_isDancing)
        {
            levelAnimator.PlayIdleAnimation();
        }
        else if(magnitue > .1f)
        {
            _isDancing = false;

            float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), _rotationSpeed * Time.deltaTime);

            if (!isStunned && !isCheering && !isDashing && !isDoubleDashing && levelAnimator.GetAnimationName() != "Walk")
            {
                levelAnimator.SetAddAnimation("Walk", true, 0, false);
            }
        }

        _rb.velocity = movement * _speed;
    }
}
