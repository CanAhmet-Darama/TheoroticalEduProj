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

    public GameObject healthGiver;
    public byte healthPerGiver;
    public float duratForHealth;
    public bool closeToDeath;

    public ParticleSystem exploParticle;
    public ParticleSystem impactParticle;
    public ParticleSystem ammoCrashParticle;

    public AudioSource AudioSourceIns;
    public AudioClip exploSound;
    public AudioClip gameOverSound;

    public Slider healthBar;
    public TextMeshProUGUI healthBarText;

    public TextMeshProUGUI scoreText;
    public short gameScore;

    public TextMeshProUGUI enHText;
    public TextMeshProUGUI enDText;
    public TextMeshProUGUI diffiText;
    public GameObject gameOverPanel;

    public static GameObject currentEnemy;

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
            diffiText = GameObject.Find("Difficulty Text").GetComponent<TextMeshProUGUI>();
            string hardnessTyped = "Yarrak";
            switch (gameDifficulty)
            {
                case 1:
                    hardnessTyped = "Easy";
                    break;
                case 2:
                    hardnessTyped = "Normal";
                    break;
                case 3:
                    hardnessTyped = "Hard";
                    break;
            }
            diffiText.text = "Difficulty : " + hardnessTyped;
            closeToDeath = false;
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
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu Scene");
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
    public void GameOver()
    {
        GroundMover.groundSpeed = 0;
        currentEnemy.GetComponent<EnemyController>().enemySpeed = 0;
        currentEnemy.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        gameOverPanel.SetActive(true);
        StartCoroutine(PlayGOSound(0.5f));

    }
    IEnumerator PlayGOSound(float howManySec)
    {
        yield return new WaitForSeconds(howManySec);
        AudioSourceIns.clip = gameOverSound;
        AudioSourceIns.Play();
    }
    void CallEnemy()
    {
        enemySpawnPos.x = Random.Range(-10, 11);
        if (!existsEnemy)
        {
            switch(GetRandomEnemyNum())
            {
                case 1:
                    currentEnemy = Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 2:
                    currentEnemy = Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 3:
                    currentEnemy = Instantiate(enemyPrefab1, enemySpawnPos, enemyPrefab1.transform.rotation);
                    existsEnemy = true;
                    break;
                case 4:
                    currentEnemy = Instantiate(enemyPrefab2, enemySpawnPos, enemyPrefab2.transform.rotation);
                    existsEnemy = true;
                    break;
                case 5:
                    currentEnemy = Instantiate(enemyPrefab2, enemySpawnPos, enemyPrefab2.transform.rotation);
                    existsEnemy = true;
                    break;
                case 6:
                    currentEnemy = Instantiate(enemyPrefab3, enemySpawnPos, enemyPrefab3.transform.rotation);
                    existsEnemy = true;
                    break;
            }
            UpdateEnemyTexts();
        }
    }
    byte GetRandomEnemyNum()
    {
        return (byte)Random.Range(1, 7);
    }
    public void UpdateEnemyTexts()
    {
        enHText.text = "E. Health : " + currentEnemy.GetComponent<GeneralShipScript>().healthOfShip;
        enDText.text = "E. Damage : " + currentEnemy.GetComponent<GeneralShipScript>().damageOfShip;
    }
    public void HealShip(GameObject shipToHeal)
    {
        shipToHeal.GetComponent<GeneralShipScript>().healthOfShip += healthPerGiver;
        healthBar.value = shipToHeal.GetComponent<GeneralShipScript>().healthOfShip;
        healthBarText.text = "" + shipToHeal.GetComponent<GeneralShipScript>().healthOfShip;

    }
}
