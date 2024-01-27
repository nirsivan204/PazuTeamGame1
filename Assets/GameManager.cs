using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;


    bool isInit;

    public void  onWIn()
    {
        Win.SetActive(true);
    }

    public void onLose()
    {
        Lose.SetActive(true);
    }

    public void OnReturnToMainManu()
    {
        SceneManager.LoadScene(2);
    }

    public void Awake()
    {
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
                print(SceneManager.GetActiveScene().buildIndex);
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
