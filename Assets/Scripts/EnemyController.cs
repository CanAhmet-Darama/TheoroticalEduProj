using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameManager gameManagerIns;
    Rigidbody enemyRb;
    public float enemySpeed;
    public float enemyZPosLimit;
    public float enemyShootingCooldown;
    GameObject playerShip;
    PlayerController playerCont;
    public GameObject enemyBullet;
    public byte scorePerEnemy;
    public AudioClip enemyShootingSound;
    public AudioClip enemyAimingSound;

    public bool canShoot;

    [SerializeField] byte multiplierEnemyPower;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        gameManagerIns = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerShip = GameObject.Find("PlayerShip");
        playerCont = playerShip.GetComponent<PlayerController>();
        GetComponent<GeneralShipScript>().healthOfShip = GameManager.gameDifficulty;
        GetComponent<GeneralShipScript>().healthOfShip += (byte)(multiplierEnemyPower-1);
        GetComponent<GeneralShipScript>().damageOfShip = (byte)(5 * GameManager.gameDifficulty);
        GetComponent<GeneralShipScript>().damageOfShip *= (byte)(2 + multiplierEnemyPower);
    }


    void Update()
    {
        EnemyMover();
    }
    void EnemyMover()
    {
        if (transform.position.z > enemyZPosLimit)
        {
            transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
        }
        else
        {
            if (playerShip.transform.position.x < transform.position.x)
            {
                enemyRb.AddForce(-enemySpeed * Time.deltaTime * 25, 0, 0);
            }
            else if (playerShip.transform.position.x > transform.position.x)
            {
                enemyRb.AddForce(enemySpeed * Time.deltaTime * 25, 0, 0);
            }

            if (Mathf.Abs(playerShip.transform.position.x - transform.position.x) < 1)
            {
                if (GetComponent<GeneralShipScript>().canShoot)
                {
                    playerShip.GetComponent<PlayerController>().ShootABullet(enemyBullet, enemyShootingCooldown, this.gameObject, enemyShootingSound, GetComponent<GeneralShipScript>().bulletSpeed, 0.5f);
                }
            }
        }
        CheckPlayerBorders();
        CheckHealth();
    }
    void CheckPlayerBorders()
    {
        float playerXBorder = 11;
        if (transform.position.x < -playerXBorder)
        {
            transform.position = new Vector3(-playerXBorder, transform.position.y, transform.position.z);
            enemyRb.velocity = new(0, 0, enemyRb.velocity.z);
        }
        if (transform.position.x > playerXBorder)
        {
            transform.position = new Vector3(playerXBorder, transform.position.y, transform.position.z);
            enemyRb.velocity = new(0, 0, enemyRb.velocity.z);
        }
        /*if (transform.position.z > -0.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
            enemyRb.velocity = new(enemyRb.velocity.x, 0, 0);
        }*/
        if (transform.position.z < enemyZPosLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, enemyZPosLimit);
            enemyRb.velocity = new(enemyRb.velocity.x, 0, 0);
        }


    }
    void CheckHealth()
    {
        if(GetComponent<GeneralShipScript>().healthOfShip <= 0 || GetComponent<GeneralShipScript>().healthOfShip == 255)
        {
            playerShip.GetComponent<PlayerController>().ExplosionFunc(this.transform.position);
            gameManagerIns.existsEnemy = false;
            gameManagerIns.gameScore += scorePerEnemy;
            gameManagerIns.scoreText.text = "Score : " + gameManagerIns.gameScore / 2;
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            playerCont.HealthReducer(playerShip.GetComponent<GeneralShipScript>().damageOfShip,this.gameObject);
            Debug.Log("Remaining Enemy Health : " + GetComponent<GeneralShipScript>().healthOfShip);
            Instantiate(gameManagerIns.impactParticle, transform.position, gameManagerIns.impactParticle.transform.rotation);
            Destroy(collision.gameObject);
        }
    }
}
