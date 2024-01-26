using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isInit;
    public void Awake()
    {
        InputMGR[] allInputManagers = FindObjectsOfType<InputMGR>();
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
            isInit = true;
        }
    }

    void Start()
    {
        SceneManager.LoadScene(1 - SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
