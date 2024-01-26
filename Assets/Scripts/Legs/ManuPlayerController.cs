using Legs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static playerController;


public class ManuPlayerController : AbstractPlayerMovement
{
    [SerializeField] private GameObject[] Manus;
    [SerializeField] Button[] mainManuButtons;

    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    public event Action OnCheerAction;
    public event Action OnCheerEndAction;
    int _currentOption;
    int _currentManu;
    bool player2Show = true;



    public override void Init(HumanPlayer controller1, HumanPlayer controller2)
    {
        _controller1 = controller1;
        _controller2 = controller2;
        _controller2.OnXPress.AddListener(OnSelect2);
        _controller1.OnXPress.AddListener(OnSelect);
        //_controller2.OnRightAnalogMove.AddListener(MoveRightStick);
        _controller1.OnCirclePress.AddListener(OnBack);
       // _controller1.OnLeftAnalogMove.AddListener(Move);
    }

    private void OnSelect2()
    {
        if(_currentManu == 3)
        {
            if (player2Show)
            {
                StartGame();
            }
            else
            {
                showSecondPlayer();
            }
        }
    }

    private void showSecondPlayer()
    {
        player2Show = true;
    }


/*    bool iszero = true;
    public void Move(Vector2 movement)
    {
        if (movement.y < 0.1 && movement.y > -0.1)
        {
            iszero = true;
        }
        if (iszero) {
            if (movement.y > 0.9)
            {
                _currentOption--;
                iszero = false;
            }
            else
            {
                if (_currentOption < 2)
                {
                    if (movement.y < -0.9)
                    {
                        _currentOption++;
                        iszero = false;
                    }
                }
            }
            UpdateManuButtons();
        } 
    }*/

    private void UpdateManuButtons()
    {
        //mainManuButtons[_currentOption].Select();
/*        for (int i = 0; i < Manus.Length; i++)
        {
            if (_currentOption == i)
            {
                
            }
            else
            {
                mainManuButtons[i];
            }
        }
        mainManuButtons[_currentOption]*/
    }

    private void OnSelect()
    {
        switch (_currentManu)
        {
            case 0:
                SelectOnHomeScreen();
                break;
/*            case 1:
                SelectOnControllsScreen();
                break;
            case 2:
                SelectOnCreditsScreen();
                break;
            case 3:
                if (player2Show)
                {
                    StartGame();
                }
                break;
*/
        }

    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    public void SelectOnHomeScreen()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (button == null)
            return;
        button.OnPointerClick(new PointerEventData(EventSystem.current));
/*        if(_currentOption == 0)
        {
            _currentManu = 3;
            UpdateManu();
        }else
        if (_currentOption == 1)
        {
            _currentManu = 1;
            UpdateManu();
        }
        else
        {
            _currentManu = 2;
            UpdateManu();
        }*/
        //EventManagerCurrentButton
    }

    private void OnBack()
    {

            _currentManu = 0;
            UpdateManu(0);
    }

    public void UpdateManu(int index)
    {
        for(int i=0;i< Manus.Length;i++)
        {
            if(index == i)
            {
                Manus[index].SetActive(true);
            }
            else
            {
                Manus[i].SetActive(false);
            }
        }
        _currentManu = index;
    }
}