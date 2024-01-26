using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
/*    bool isInit;
    public void Awake()
    {
        InputMGR[] allInputManagers = FindObjectsOfType<InputMGR>();
        if (allInputManagers.Length > 1)
        {
            if (!isInit)
            {
                if (allInputManagers[0] != this)
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

            DontDestroyOnLoad(playerMovement);
            SceneManager.LoadScene(1 - SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Additive);
            isInit = true;
        }
    }*/

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
