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
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject[] images;
    [SerializeField] private Text _credits;
    [SerializeField] private GameObject _sprite2;
    [SerializeField] private GameObject _sprite1;


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
        if (GameManager.isCredits)
        {
            UpdateManu(2);
            GameManager.isCredits = false;
        }
        else
        {
            UpdateManu(0);
        }
        // _controller1.OnLeftAnalogMove.AddListener(Move);
    }

    private void OnSelect2()
    {
        if (_currentManu == 3)
        {
            if (player2Show && player1Show)
            {
                StartGame();
            }
            else
            {
                if (!player2Show)
                {
                    showSecondPlayer();
                    player2Show = true;
                }
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
                if (!player1Show)
                {
                    showFirstPlayer();
                    player1Show = true;
                }

            }
        }
    }
    private void showSecondPlayer()
    {
        player2Show = true;
        StartCoroutine(GetBig(frame2.transform,1));
        //LeanTween.scale(frame2.gameObject, 7 * Vector3.one, 2);
        characters[1].SetActive(true);
    }

    IEnumerator GetBig(Transform t,int index)
    {
        int i = 0;
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        while (i < 50)
        {
            t.transform.localScale += Vector3.one * 0.14f;
            images[index].transform.Rotate(Vector3.right,1.8f);
            i++;
            yield return wait;
        }
    }
    private void showFirstPlayer()
    {
        player1Show = true;
        StartCoroutine(GetBig(frame1.transform,0));

        characters[0].SetActive(true);

    }
    bool isGameStarted = false;
    private void StartGame()
    {
        if (isGameStarted)
            return;
        isGameStarted = true;
        SceneManager.LoadScene(1);
    }

    public void OnCredits()
    {
        _credits.rectTransform.position = new Vector2(250,-100);
        StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits()
    {
        int i = 0;
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        while(i < 1650)
        {
            _credits.rectTransform.position += Vector3.up * 5f;
            i++;
            if (i % 10 == 0)
            {
                _sprite1.gameObject.SetActive(i % 20 == 0);
                _sprite2.gameObject.SetActive(i % 20 != 0);
            }

            yield return wait;
        }
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
    bool isChangingManu = false;

    public void UpdateManu(int index)
    {
        if (isChangingManu)
            return;
        isChangingManu = true;
        StartCoroutine(UpdateManuCor(index));


    }

    public IEnumerator  UpdateManuCor(int index)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < Manus.Length; i++)
        {
            if (index == i)
            {
                Manus[index].SetActive(true);
            }
            else
            {
                Manus[i].SetActive(false);
            }
        }
        _currentManu = index;
        if(_currentManu == 2)
        {
            OnCredits();
        }
        isChangingManu = false;
    }

}