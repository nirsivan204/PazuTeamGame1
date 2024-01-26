using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquenceManager : MonoBehaviour
{
    [SerializeField] private HumanPlayer _controller1;
    [SerializeField] private HumanPlayer _controller2;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] Sprites;
    
    private int _squenceIndex;
    private int _buttonValue;

    private bool _canFail = true;


    private int[] _sequence1 = { 5, 4 };    //= {8, 4, 5, 11 };
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

    }

    //Start function just to test
    private void Start()
    {
        Debug.Log("Started test");
        PickSequence(1);
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
        if (_currentSequence != _sequence4)
        {
            _spriteRenderer.sprite = Sprites[_currentSequence[_squenceIndex]];
            if (_currentSequence[_squenceIndex] == buttonNum)
            {
                _squenceIndex++;
                _squenceIndex %= _currentSequence.Length;
                Debug.Log("Succses, next number is - " + _currentSequence[_squenceIndex]);
                if (_squenceIndex == _currentSequence.Length)
                {
                    Debug.Log("PassedTest");
                    //PassTestEvent
                }
            }
            else
            {
                _squenceIndex = 0;
                Debug.Log("Failed Test value is -" + buttonNum);
            }
     //Only for final Test - Maybe ovride Check isHandsTurn so its both turn and make buttonNum w values HandsbuttonNum And LegsbuttonNum ?? 
        }
        else
        {
            float LegsbuttonNum = 0;
            float HandsbuttonNum = 0;
            if (_currentSequence[_squenceIndex] == HandsbuttonNum & _currentSequence[_squenceIndex + 1] == LegsbuttonNum)
            {
                _squenceIndex += 2 ;
                _squenceIndex %= _currentSequence.Length;
                if (_squenceIndex == _currentSequence.Length)
                {
                    Debug.Log("PassedFinalTest");
                    //FinishGameEvent
                }
            }
            else
            {
                _squenceIndex = 0;
                Debug.Log("Failed Test");
            }
        }
    }

    //public function to be triggers from scene object
    public void PickSequence(int sequenceNum)
    {
        switch (sequenceNum)
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
        Debug.Log("HandsTriangle");
        if (isHandsTurn())
        {
            CheckButton(0);
        }
    }
    private void HandsCirclePress()
    {
        Debug.Log("HandsCircle");
        if (isHandsTurn())
        {
            CheckButton(1);
        }
    }
    private void HandsSquarePress()
    {
        Debug.Log("HandsSquare");
        if (isHandsTurn())
        {
            CheckButton(2);
        }
    }
    private void HandsXPress()
    {
        Debug.Log("HandsX");
        if (isHandsTurn())
        {
            CheckButton(3);
        }
    }

    private void HandsAnalogMove(Vector2 move)
    {   
        if (!_canFail)
        {
            return;
        }
        Debug.Log("HandsAnalog" + move);
        _canFail = false;

        LeanTween.delayedCall(0.5f,() => _canFail = true);
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
    private void HandsL1Press()
    {
        Debug.Log("HandsL1");

        if (isHandsTurn())
        {
            CheckButton(6);
        }
    }
    private void HandsR1Press()
    {
        Debug.Log("HandsR1");

        if (isHandsTurn())
        {
            CheckButton(7);
        }
    }
    //Player 2 Button Press
    private void LegsTrianglePress()
    {
        Debug.Log("LegsTriangle");

        if (!isHandsTurn())
        {
            CheckButton(8);
        }
    }
    private void LegsCirclePress()
    {
        Debug.Log("LegsCircle");

        if (!isHandsTurn())
        {
            CheckButton(9);
        }
    }
    private void LegsSquarePress()
    {
        Debug.Log("LegsSquare");
        if (!isHandsTurn())
        {
            CheckButton(10);
        }
    }
    private void LegsXPress()
    {
        Debug.Log("LegsX");
        if (!isHandsTurn())
        {
            CheckButton(11);
        }
    }
    private void LegsAnalogMove(Vector2 move)
    {
        Debug.Log("LegssAnalog" + move);
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
    private void LegsL1Press()
        {
            Debug.Log("LegsL1");
            if (!isHandsTurn())
            {
            CheckButton(14);
            }
    }
    private void LegsR1Press()
        {
            Debug.Log("LegsR1");
            if (!isHandsTurn())
        {
            CheckButton(15);
        }
    }
}
