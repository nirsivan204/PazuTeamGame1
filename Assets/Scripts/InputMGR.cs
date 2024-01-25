using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputMGR : MonoBehaviour
{
    public PlayerInput[] playersInputs = new PlayerInput[8];
    public int playersInGame = 0;
    public PlayerInputManager PIM_component;
    public int keyboardPlayers = 0;
    public int gamepadPlayers = 0;
    public int lastGamePadIDLeft = -1;
    public bool isInit = false;
    [SerializeField] HumanPlayer[] players;

    public void Awake()
    {
        if (FindObjectsOfType<PlayerInputManager>().Length > 1)
        {
            if (!isInit)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void init()
    {
        var p1 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "Keyboard1", pairWithDevice: Keyboard.current);
        var p2 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "Keyboard2", pairWithDevice: Keyboard.current);
        var p3 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "Keyboard3", pairWithDevice: Keyboard.current);
        var p4 = PlayerInput.Instantiate(PIM_component.playerPrefab,
            controlScheme: "Keyboard4", pairWithDevice: Keyboard.current);
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
            }
            else
            {
                gamepadPlayers++;
                playersInGame++;
                emptySpace = findNextEmptySpace();

            }
            playersInputs[emptySpace] = PI;
            players[emptySpace].init(this);
            DontDestroyOnLoad(PI);
        }

    }

    private int findNextEmptySpace()
    {
        for(int i = 4; i < 8; i++)
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
    public void ConnectToKeyboardPlayerController(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction = null, UnityAction squareAction = null)
    {
        if(playersInputs[playerId] != null)
        {
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
                playersInputs[playerId].GetComponent<playerController>().moveRightEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().moveRightEvent.AddListener(moveLeftAction);
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
                playersInputs[playerId].GetComponent<playerController>().squareEvent.RemoveAllListeners();
                playersInputs[playerId].gameObject.GetComponent<playerController>().squareEvent.AddListener(squareAction);
            }
        }
    }

    public void ConnectToGamePadPlayerController(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction = null, UnityAction squareAction = null, UnityAction<Vector2> DigitalMoveAction = null)
    {
        ConnectToKeyboardPlayerController(playerId + keyboardPlayers, moveRightAction, moveLeftAction, fireAction, circleAction,triangleAction,squareAction);
        if(DigitalMoveAction != null && playersInputs[playerId + keyboardPlayers] != null)
        {
            playersInputs[playerId + keyboardPlayers].gameObject.GetComponent<playerController>().digitalMoveEvent.AddListener(DigitalMoveAction);
        }
    }

    public void ConnectToAllPlayersGamePads(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction)
    {
        for (int i = 0; i < 4; i++)
        {
            ConnectToGamePadPlayerController(i, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction);
        }
    }


    public void ConnectToAllPlayerControllers(int playerId, UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction)
    {
        ConnectToKeyboardPlayerController(playerId, moveRightAction, moveLeftAction, fireAction, circleAction);
        ConnectToGamePadPlayerController(playerId, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction);
    }

    public void ConnectToAllPlayersControllers(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction, UnityAction fireAction, UnityAction circleAction, UnityAction triangleAction, UnityAction squareAction, UnityAction<Vector2> DigitalMoveAction)
    {
        for(int i = 0; i< 4; i++)
        {
            ConnectToAllPlayerControllers(i, moveRightAction, moveLeftAction, fireAction, circleAction, triangleAction, squareAction, DigitalMoveAction);
        }
    }

    public void ConnectToAllPlayersKeyBoards(UnityAction<Vector2> moveRightAction, UnityAction<Vector2> moveLeftAction , UnityAction fireAction, UnityAction circleAction)
    {
        for (int i = 0; i < 4; i++)
        {
            ConnectToKeyboardPlayerController(i, moveRightAction, moveLeftAction, fireAction, circleAction);
        }
    }

    public int getIndexOfPI(PlayerInput PI)
    {
        for (int i = 0; i < 8; i++)
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
        for(int i = 4; i < 8; i++)
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
               // playersInputs[i].GetComponent<PlayerController>().CanLeave = false;

            }

        }

    }
}
