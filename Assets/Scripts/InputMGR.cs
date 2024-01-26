using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputMGR : MonoBehaviour
{
    public PlayerInput[] playersInputs = new PlayerInput[4];
    public int playersInGame = 0;
    public PlayerInputManager PIM_component;
    public int keyboardPlayers = 0;
    public int gamepadPlayers = 0;
    public int lastGamePadIDLeft = -1;
    public bool isInit = false;
    [SerializeField] HumanPlayer[] players;
    [SerializeField] AbstractPlayerMovement playerMovement;

    public void Awake()
    {
        InputMGR[] allInputManagers = FindObjectsOfType<InputMGR>();
        if (allInputManagers.Length > 1)
        {
            if (!isInit)
            {
                if(allInputManagers[0] != this)
                {
                    playerMovement.Init(allInputManagers[0].players[0], allInputManagers[0].players[1]);
                }
                else
                {
                    playerMovement.Init(allInputManagers[1].players[0], allInputManagers[1].players[1]);
                }
                DontDestroyOnLoad(playerMovement);
                gameObject.SetActive(false);
                Destroy(players[0].gameObject);
                Destroy(players[1].gameObject);
                Destroy(gameObject);
            }
        }
        else
        {

            PIM_component.enabled = true;
            init();
            playerMovement.Init(players[0], players[1]);
            DontDestroyOnLoad(playerMovement);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(players[0]);
            DontDestroyOnLoad(players[1]);
            gameObject.SetActive(true);

        }
    }

    public void init()
    {
        var p1 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "keyboard1", pairWithDevice: Keyboard.current);
        var p2 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "keyboard2", pairWithDevice: Keyboard.current);
        PIM_component.playerLeftEvent.AddListener(OnPlayerLeft);

        isInit = true;
    }



    public void OnPlayerJoined(PlayerInput PI)
    {
        print("join");
        if (playersInGame < 2)
        {
            int emptySpace;
            if(keyboardPlayers < 2)
            {
                emptySpace = keyboardPlayers;
                keyboardPlayers++;
                playersInputs[emptySpace] = PI;
                players[emptySpace].initKeyboard(this);
            }
            else
            {
                emptySpace = keyboardPlayers + gamepadPlayers;
                playersInputs[emptySpace] = PI;
                players[playersInGame].initController(this);
                gamepadPlayers++;
                playersInGame++;
                if(playersInGame == 2)
                {
                    disableJoin();
                }
                //emptySpace = findNextEmptySpace();
            }
            DontDestroyOnLoad(PI);
        }

    }

    private int findNextEmptySpace()
    {
        for(int i = 2; i < 4; i++)
        {
            if(playersInputs[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnPlayerLeft(PlayerInput PI)
    {
        /*        for(int i=4; i<8 ; i++)
                {
                    if(playersInputs[i] == PI)
                    {
                        playersInputs[i] = null;
                        break;
                    }
                }*/
        if (getIndexOfPI(PI) >= keyboardPlayers)
        {
            lastGamePadIDLeft = getIndexOfPI(PI);
            gamepadPlayers--;
            playersInputs[lastGamePadIDLeft] = null;
            playersInGame--;
        }
        //playersInputs[keyboardPlayers + gamepadPlayers] = null;
    }
    public void ConnectToKeyboardPlayerController(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction OnR1 = null, UnityAction OnL1 = null)
    {
        if(playersInputs[playerId] != null)
        {

            //print(playersInputs[playerId]);
            if (fireAction != null)
            {
                playersInputs[playerId].GetComponent<playerController>().fireEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().fireEvent.AddListener(fireAction);

            }
            if (moveRightAction != null)
            {
                playersInputs[playerId].GetComponent<playerController>().moveRightEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().moveRightEvent.AddListener(moveRightAction);
            }
            if (moveLeftAction != null)
            {
                playersInputs[playerId].GetComponent<playerController>().moveLeftEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().moveLeftEvent.AddListener(moveLeftAction);
            }
            if (circleAction != null)
            {
                playersInputs[playerId].GetComponent<playerController>().circleEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().circleEvent.AddListener(circleAction);
            }
            if (triangleAction != null)
            {
                playersInputs[playerId].GetComponent<playerController>().triangleEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().triangleEvent.AddListener(triangleAction);
            }
            if (squareAction != null)
            {
                print(playersInputs[playerId]);
                playersInputs[playerId].GetComponent<playerController>().squareEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().squareEvent.AddListener(squareAction);
            }
            if (OnR1 != null)
            {
                playersInputs[playerId].GetComponent<playerController>().R1Event.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().R1Event.AddListener(OnR1);
            }
            if (OnL1 != null)
            {
                playersInputs[playerId].GetComponent<playerController>().L1Event.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().L1Event.AddListener(OnL1);
            }
        }
    }

    public void ConnectToGamePadPlayerController(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction = null, UnityAction squareAction = null, UnityAction<Vector2> DigitalMoveAction = null, UnityAction OnR1=null, UnityAction OnL1 = null)
    {
        ConnectToKeyboardPlayerController(playerId + keyboardPlayers, moveRightAction, moveLeftAction, fireAction, circleAction,triangleAction,squareAction,OnR1,OnL1);
        if(DigitalMoveAction != null && playersInputs[playerId + keyboardPlayers] != null)
        {
            playersInputs[playerId + keyboardPlayers].gameObject.GetComponent<playerController>().digitalMoveEvent.AddListener(DigitalMoveAction);
        }
    }

    public void ConnectToAllPlayersGamePads(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction)
    {
        for (int i = 0; i < 2; i++)
        {
            ConnectToGamePadPlayerController(i, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction);
        }
    }


    public void ConnectToAllPlayerControllers(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction, UnityAction OnR1, UnityAction OnL1)
    {
        ConnectToKeyboardPlayerController(playerId, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction,squareAction, OnR1, OnL1);
        ConnectToGamePadPlayerController(playerId, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction, OnR1, OnL1);
    }

/*    public void ConnectToAllPlayersControllers(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction)
    {
        for(int i = 0; i< 2; i++)
        {
           // ConnectToAllPlayerControllers(i, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction, OnR1, OnL1);
        }
    }*/

    public void ConnectToAllPlayersKeyBoards(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction , UnityAction fireAction, UnityAction circleAction)
    {
        for (int i = 0; i < 2; i++)
        {
            //ConnectToKeyboardPlayerController(i, moveRightAction, moveLeftAction, fireAction, circleAction,);
        }
    }

    public int getIndexOfPI(PlayerInput PI)
    {
        for (int i = 0; i < 4; i++)
        {
            if (playersInputs[i] == PI)
            {
                return i;
            }
        }
        return -1;
    }

    public int getMainPlayerGamePadID()
    {
        for(int i = 2; i < 4; i++)
        {
            if (playersInputs[i] != null)
            {
                return i-keyboardPlayers;
            }
        }
        return -1;
    }

    public void disableJoin()
    {
        PIM_component.DisableJoining();
    }

    public void enableJoin()
    {
        PIM_component.EnableJoining();

    }

    public void enableLeave()
    {
        for (int i = keyboardPlayers;  i < playersInputs.Length; i++)
        {
            if (playersInputs[i] != null)
            {
                    //playersInputs[i].GetComponent<PlayerController>().CanLeave = true;
            }

        }

    }

    public void disableLeave()
    {

        for (int i = keyboardPlayers; i < playersInputs.Length; i++)
        {
            if (playersInputs[i] != null)
            {
                playersInputs[i].GetComponent<playerController>().CanLeave = false;

            }

        }

    }
}
