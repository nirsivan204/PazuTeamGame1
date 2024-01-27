using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class SquenceManager : MonoBehaviour
{
    [SerializeField] private HumanPlayer _controller1;
    [SerializeField] private HumanPlayer _controller2;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] Sprites;
    

    [SerializeField] private int _whatSequence;

    public static SquenceManager instance;

    public void OnOtherEnter()
    {
        _isPlayer2Here = true;
        if (_isPlayer1Here)
        {
            StartTest();
        }
    }
    public UnityEvent<Sprite> ChangeSpriteOnTrigger;
    public UnityEvent RemoveSpriteOnTrigger;

    private int _squenceIndex = 0;
    //private int _buttonValue;

    public bool _isPlayer1Here = false;
    public static bool _isPlayer2Here = false;

    private bool _canFail = true;
    private bool _isStartTest = false;

    private int[] _sequence1 = {8, 4, 5, 11 };
//Sequence 1 - LegsTriangle, Hands Up, Hands Down, Legs X

    private int[] _sequence2 = { 0, 5, 15, 14, 6, 6, 12, 0 };
//Sequence 2 - Legs Triangle, Hands Down, Legs R1, Legs L1, Hands L1, Hands L1,Legs  Up, Hands Triangle

    private int[] _sequence3 = { 8, 0, 8, 16, 11, 11, 17, 5, 4};
    //Sequence 3- Legs Triangle, Hands Triangle, Legs Triangle, Hands DONT PRESS(1 sec), Legs X, Legs X, Legs DONT PRESS(1 sec), Hands Down, Hands Up

    private int[] _sequence4 = { 5, 13, 2, 9, 4, 12, 16, 17, 1, 10 };
    //Final Sequence- Fusion dance- Down (5 + 13), Hands Square & Legs Circle (2+ 9), Up (4 + 12), DONT PRESS (16+17), Hands Circle and Legs Square (1 + 10)
    //Squence Values:
    //               HandsTriangle   = 0
    //               HandsCircle     = 1
    //               Handssquare     = 2
    //               Hands X         = 3
    //               Hands Up        = 4
    //               Hands Down      = 5
    //               Hands L1        = 6
    //               Hands R1        = 7

    //               LegsTriangle  = 8
    //               LegsCircle    = 9
    //               Legssquare    = 10
    //               Legs X        = 11
    //               Legs Up       = 12
    //               Legs Down     = 13
    //               Legs L1       = 14
    //               Legs R1       = 15

    // Left DONT PRESS(1 sec)       = 16
    // Right DONT PRESS(1 sec)      = 17

    private int[] _currentSequence;

    //public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    private void Awake()
    {
        instance = this;
        print("init legs");
        _controller1.OnTrianglePress.AddListener(HandsTrianglePress);
        _controller1.OnCirclePress.AddListener(HandsCirclePress);
        _controller1.OnSquarePress.AddListener(HandsSquarePress);
        _controller1.OnXPress.AddListener(HandsXPress);
        _controller1.OnLeftAnalogMove.AddListener(HandsAnalogMove);
        _controller1.OnL1Press.AddListener(HandsR1Press);
        _controller1.OnR1Press.AddListener(HandsL1Press);

        _controller2.OnTrianglePress.AddListener(LegsTrianglePress);
        _controller2.OnCirclePress.AddListener(LegsCirclePress);
        _controller2.OnSquarePress.AddListener(LegsSquarePress);
        _controller2.OnXPress.AddListener(LegsXPress);
        _controller2.OnLeftAnalogMove.AddListener(LegsAnalogMove);
        _controller2.OnL1Press.AddListener(LegsL1Press);
        _controller2.OnR1Press.AddListener(LegsR1Press);

/*        SquenceManager[] sequenceList =  FindObjectsOfType<SquenceManager > ();
        for (int i = 0; i < sequenceList.Length; i++)
        {
            if (sequenceList[i] == this)
                continue;
            if (sequenceList[i]._whatSequence == _whatSequence)
            {
                brother = sequenceList[i];
                sequenceList[i]
            }

        }*/
    }

    //Start function just to test
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hands")
        { _isPlayer1Here = true;
            Debug.Log("Hands here");
        }

        if(_isPlayer1Here & _isPlayer2Here)
        {
            StartTest();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hands")
        {
            _isPlayer1Here = false;
        }
        if (other.gameObject.tag == "Legs")
        {
            _isPlayer2Here = false;
        }
    }
    private void StartTest()
    {
        Debug.Log("Start Test");
        if (_isStartTest)
            return;
        GameManager.Instance.StopPlayersInput();
        _isStartTest = true;

        PickSequence();
        _squenceIndex = 0;
        SpritePicker();
        isHandsTurn();


    }
    private void SpritePicker()
    { int spriteIndex = _currentSequence[_squenceIndex];
        //hands sprite
        if (spriteIndex > 7)
        {
            spriteIndex -= 8;
            _spriteRenderer.enabled = false;
            ChangeSpriteOnTrigger?.Invoke(Sprites[spriteIndex]);


        }
        //Legs sprite
        else
        {
            RemoveSpriteOnTrigger?.Invoke();
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = Sprites[spriteIndex];

        }
    }
    private bool isHandsTurn() 
    {
        if (_currentSequence[_squenceIndex] <= 7)
        {   
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CheckButton(int buttonNum)
    {
        if (_isStartTest) {
            //_spriteRenderer.sprite = Sprites[_currentSequence[_squenceIndex]];
                if (_currentSequence[_squenceIndex] == buttonNum)
                {
                    _squenceIndex++;
                    _squenceIndex %= _currentSequence.Length;
                    Debug.Log("Succses, next number is - " + _currentSequence[_squenceIndex]);
                    SpritePicker();

                if (_squenceIndex == _currentSequence.Length)
                    {
                        Debug.Log("PassedTest");
                        GameManager.Instance.onWIn(true);

                        //PassTestEvent
                    }
                }
                else
                {
                    GameManager.Instance.onWIn(false);

                    //_squenceIndex = 0;
                    Debug.Log("Failed Test value is -" + buttonNum);
                }
            }
    }

    //public function to be triggers from scene object
    public void PickSequence()
    {
        switch (_whatSequence)
        {
            case 1:
                _currentSequence = _sequence1;
                break;
            case 2:
                _currentSequence = _sequence2;
                break;
            case 3:
                _currentSequence = _sequence3;
                break;
            case 4:
                _currentSequence = _sequence4;
                break;
        }
    }

    //Player 1 Button Press

    private void HandsTrianglePress()
    {
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(0);
            }
        }
    }
    private void HandsCirclePress()
    {
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(1);
            }
        }
    }
    private void HandsSquarePress()
    {
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(2);
            }
        }
    }
    private void HandsXPress()
    {
        print("nir");
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(3);
            }
        }
    }

    private void HandsAnalogMove(Vector2 move)
    {
        if (_isStartTest)
        {
            if (!_canFail)
            {
                return;
            }
            _canFail = false;

            LeanTween.delayedCall(0.5f, () => _canFail = true);
            if (isHandsTurn())
            {
                if (move.y > 0.8)
                {
                    CheckButton(4);

                }
                else
                {
                    if (move.y < -0.8)
                        CheckButton(5);
                }
            }
        }
    }
    private void HandsL1Press()
    {
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(6);
            }
        }
    }
    private void HandsR1Press()
    {
        if (_isStartTest)
        {
            if (isHandsTurn())
            {
                CheckButton(7);
            }
        }
    }

    //Player 2 Button Press
    private void LegsTrianglePress()
    {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(8);
            }
        }
    }
    private void LegsCirclePress()
    {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(9);
            }
        }
    }
    private void LegsSquarePress()
    {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(10);
            }
        }
    }
    private void LegsXPress()
    {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(11);
            }
        }
    }
    private void LegsAnalogMove(Vector2 move)
    {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                if (move.y > 0.8)
                {
                    CheckButton(12);
                }
                else
                {
                    if (move.y < -0.8)
                        CheckButton(13);
                }
            }
        }
    }
    private void LegsL1Press()
        {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(14);
            }
        }
    }
    private void LegsR1Press()
        {
        if (_isStartTest)
        {
            if (!isHandsTurn())
            {
                CheckButton(15);
            }
        }
    }
}
