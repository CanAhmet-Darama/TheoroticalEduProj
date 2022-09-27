using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static byte gameDifficulty = 2;
    public GameObject emptyForNull;
    public Vector3 enemySpawnPos;

    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public ParticleSystem exploParticle;
    public AudioClip exploSound;
    public Slider healthBar;
    public TextMeshProUGUI healthBarText;

    public TextMeshProUGUI scoreText;
    public short gameScore;

    public bool existsEnemy = false;
    public float whenToSpawnEnemy;
    public float spawnRateEnemy;
    void Start()
    {
        enemySpawnPos.y = 0.27f;
        enemySpawnPos.z = 3;
        if (SceneManager.GetActiveScene().name == "Main Game Scene")
        {
            InvokeRepeating("CallEnemy",whenToSpawnEnemy,spawnRateEnemy);
        }
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
    void CallEnemy()
    {
        enemySpawnPos.x = Random.Range(-10, 11);
        if (!existsEnemy)
        {
            switch(GetRandomEnemyNum())
            {
                case 1:
                    Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 2:
                    Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 3:
                    Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 4:
                    Instantiate(enemyPrefab2, enemySpawnPos, enemyPrefab2.transform.rotation);
                    existsEnemy = true;
                    break;
                case 5:
                    Instantiate(enemyPrefab2, enemySpawnPos, enemyPrefab2.transform.rotation);
                    existsEnemy = true;
                    break;
                case 6:
                    Instantiate(enemyPrefab3, enemySpawnPos, enemyPrefab3.transform.rotation);
                    existsEnemy = true;
                    break;
            }
        }
    }
    byte GetRandomEnemyNum()
    {
        return (byte)Random.Range(1, 7);
    }

}
