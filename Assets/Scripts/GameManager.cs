using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static byte gameDifficulty = 2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Main Game Scene");
        Debug.Log(gameDifficulty);
    }
    public void SetGameEasy()
    {
        gameDifficulty = 1;
        Debug.Log("Difficulty is set to : " + "Easy");
    }
    public void SetGameNormal()
    {
        gameDifficulty = 2;
        Debug.Log("Difficulty is set to : " + "Normal");
    }
    public void SetGameDifficult()
    {
        gameDifficulty = 3;
        Debug.Log("Difficulty is set to : " + "Difficult");
    }
}
