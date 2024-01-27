using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;

    [SerializeField] GameObject win1Object;
    [SerializeField] GameObject win2Object;


    public static GameManager instance;
    bool isInit;
    public static bool isCredits;


    public static void  OnWin()
    {
        instance.onWIn();
    }
    public static void OnLose()
    {
        instance.onLose();
    }

    public void StopPlayersInput()
    {
        TopPlayerController topPlayer= FindObjectOfType<TopPlayerController>();
        LegsPlayerMovment legPlayer = FindObjectOfType<LegsPlayerMovment>();
        topPlayer.stopMovment();
        legPlayer.stopMovment();

    }

    public void OnCredits()
    {
        isCredits = true;
        SceneManager.LoadScene(2);
    }

    public void  onWIn(bool isGood = true)
    {
        Win.SetActive(true);
        win1Object.SetActive(isGood);
        win2Object.SetActive(!isGood);

    }

    public void onLose()
    {
        Lose.SetActive(true);
        StopPlayersInput();
    }

    public void OnReturnToMainManu()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GameManager[] allInputManagers = FindObjectsOfType<GameManager>();
        if (allInputManagers.Length > 1)
        {
            if (!isInit)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
        else
        {
            if (!isInit)
            {
                isInit = true;
                DontDestroyOnLoad(this);
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SceneManager.LoadScene(0, LoadSceneMode.Additive);

                }
                else
                {
                    SceneManager.LoadScene(1, LoadSceneMode.Additive);

                }
                instance = this;
            }

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0, LoadSceneMode.Additive);
        }
    }

    private void Start()
    {
        //onWIn();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
