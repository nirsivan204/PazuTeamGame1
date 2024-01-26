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

    [SerializeField] private Image frame1;
    [SerializeField] private Image frame2;

    private HumanPlayer _controller1;
    private HumanPlayer _controller2;

    public event Action OnCheerAction;
    public event Action OnCheerEndAction;
    int _currentOption;
    int _currentManu;
    bool player2Show = false;
    bool player1Show = false;



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
            if (player2Show && player1Show)
            {
                StartGame();
            }
            else
            {
                showSecondPlayer();
            }
        }
    }

    

    private void OnSelect()
    {
        if (_currentManu == 3)
        {
            if (player2Show && player1Show)
            {
                StartGame();
            }
            else
            {
                showFirstPlayer();
                player1Show = true;
            }
        }
    }
    private void showSecondPlayer()
    {
        player2Show = true;
        print("nir");
        LeanTween.scale(frame2.gameObject, 7 * Vector3.one, 1);

    }
    private void showFirstPlayer()
    {
        player1Show = true;
        LeanTween.scale(frame1.gameObject,7*Vector3.one,2);
    }
    bool isGameStarted = false;
    private void StartGame()
    {
        if (isGameStarted)
            return;
        isGameStarted = true;
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