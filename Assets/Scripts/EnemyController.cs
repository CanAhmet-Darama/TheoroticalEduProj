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
    public GameObject enemyBullet;
    public byte scorePerEnemy;
    public AudioClip enemyShootingSound;

    public bool canShoot;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        gameManagerIns = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Awake()
    {
        playerShip = GameObject.Find("PlayerShip");
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
                playerShip.GetComponent<PlayerController>().ShootABullet(enemyBullet, enemyShootingCooldown, this.gameObject, enemyShootingSound, GetComponent<GeneralShipScript>().bulletSpeed, 0.5f);
            }
        }
        CheckPlayerBorders();
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
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            playerShip.GetComponent<PlayerController>().ExplosionFunc(this.transform.position);
            gameManagerIns.existsEnemy = false;
            gameManagerIns.gameScore += scorePerEnemy;
            gameManagerIns.scoreText.text = "Score : " + gameManagerIns.gameScore / 2;
        }
    }
}
