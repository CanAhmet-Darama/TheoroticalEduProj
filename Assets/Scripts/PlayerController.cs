using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public GameManager gameManagerIns;
    public GameObject bulletPrefab;
    public AudioSource AudioSourceIns;
    public AudioSource SecondaryAudioSource;
    public AudioClip noAmmoSound;
    public AudioClip shootSound;
    public GameObject playerBulletPrefab;
    ParticleSystem exploParticle;

    Rigidbody playerRb;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerShootCooldown;

    float volumeDefault;
    byte bulletNum;
    #endregion

    void Start()
    {
        exploParticle = gameManagerIns.exploParticle;
        bulletNum = 1;
        volumeDefault = AudioSourceIns.volume;
        playerRb = GetComponent<Rigidbody>();
        GetComponent<GeneralShipScript>().healthOfShip = 100;
        GetComponent<GeneralShipScript>().damageOfShip = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ControlShip();
    }
    void ControlShip()
    {
        ShipControl_Shoot();
        ShipControl_Move();
        CheckPlayerBorders();
        CheckHealth();
    }

    void CheckPlayerBorders()
    {
        float playerXBorder = 11;
        if(transform.position.x < -playerXBorder)
        {
            transform.position = new Vector3(-playerXBorder, transform.position.y,transform.position.z);
            playerRb.velocity = new(0, 0, playerRb.velocity.z);
        }
        if (transform.position.x > playerXBorder)
        {
            transform.position = new Vector3(playerXBorder, transform.position.y, transform.position.z);
            playerRb.velocity = new(0, 0, playerRb.velocity.z);
        }
        if (transform.position.z > -0.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
            playerRb.velocity = new(playerRb.velocity.x, 0, 0);
        }
        if (transform.position.z < -10.4f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10.4f);
            playerRb.velocity = new(playerRb.velocity.x, 0, 0);
        }


    }
    void ShipControl_Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerRb.AddForce(Vector3.forward * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            playerRb.AddForce(Vector3.forward * -playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            playerRb.AddForce(Vector3.left * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            playerRb.AddForce(Vector3.left * -playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        
    }
    void ShipControl_Shoot()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShootABullet(playerBulletPrefab,playerShootCooldown, this.gameObject, shootSound, GetComponent<GeneralShipScript>().bulletSpeed, -0.3f);
        }
    }


    public void ShootABullet(GameObject bulletOfShot, float cooldown, GameObject shooter, AudioClip shootSoundX, float bulletSpeedG, float extraZAmount)
    {
        if (shooter.GetComponent<GeneralShipScript>().canShoot)
        {
            switch (shooter.GetComponent<GeneralShipScript>().isBigEnemy)
            {
                case false:
                    GameObject shotBullet = Instantiate(bulletOfShot, new Vector3(shooter.transform.position.x,0.25f, shooter.transform.position.z + extraZAmount), bulletOfShot.transform.rotation);
                    shotBullet.GetComponent<BulletScript>().shooterOfIt = shooter.gameObject;
                    shotBullet.GetComponent<Rigidbody>().velocity = new Vector3(0,0,bulletSpeedG);
                    break;
                case true:
                    float bigShipXShootExtra = 1.35f;
                    GameObject shotBullet1 = Instantiate(bulletOfShot, new Vector3(shooter.transform.position.x + bigShipXShootExtra, 0.25f, shooter.transform.position.z + extraZAmount), bulletOfShot.transform.rotation);
                    shotBullet1.GetComponent<BulletScript>().shooterOfIt = shooter.gameObject;
                    shotBullet1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, bulletSpeedG);
                    GameObject shotBullet2 = Instantiate(bulletOfShot, new Vector3(shooter.transform.position.x - bigShipXShootExtra, 0.25f, shooter.transform.position.z + extraZAmount), bulletOfShot.transform.rotation);
                    shotBullet2.GetComponent<BulletScript>().shooterOfIt = shooter.gameObject;
                    shotBullet2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, bulletSpeedG);
                    break;
            }
            switch (shooter.tag)
            {
                case "Player Ship":
                    AudioSourceIns.clip = shootSoundX;
                    AudioSourceIns.volume = volumeDefault;
                    AudioSourceIns.Play();
                    break;
                case "Enemy Ship":
                    SecondaryAudioSource.clip = shootSoundX;
                    SecondaryAudioSource.Play();
                    break;
            }
            StartCoroutine(AddCooldown(cooldown, shooter));
        }
        else
        {
            if (this.gameObject.tag == "Player Ship")
            {
                AudioSourceIns.volume = volumeDefault;
                Debug.Log("Player is out of ammo");
                AudioSourceIns.clip = noAmmoSound;
                AudioSourceIns.volume /= 3;
                AudioSourceIns.Play();
            }
        }
    }

    IEnumerator AddCooldown(float seconds, GameObject shooterX)
    {
            shooterX.GetComponent<GeneralShipScript>().canShoot = false;
            yield return new WaitForSeconds(seconds);
            shooterX.GetComponent<GeneralShipScript>().canShoot = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy Ship"))
        {
            ExplosionFunc(this.transform.position);
            HealthReducer(GetComponent<GeneralShipScript>().healthOfShip, this.gameObject);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        if(collision.gameObject.CompareTag("Enemy Bullet"))
        {
            Instantiate(gameManagerIns.impactParticle, transform.position, gameManagerIns.impactParticle.transform.rotation);
            HealthReducer(GameManager.currentEnemy.GetComponent<GeneralShipScript>().damageOfShip, this.gameObject);
            Instantiate(gameManagerIns.impactParticle, transform.position, gameManagerIns.impactParticle.transform.rotation);
            Destroy(collision.gameObject);
        }

    }
    public void ExplosionFunc(Vector3 posOfExp)
    {
        Vector3 expPos = posOfExp;
        Instantiate(exploParticle, expPos, exploParticle.transform.rotation);
        AudioSourceIns.clip = gameManagerIns.exploSound;
        AudioSourceIns.volume = volumeDefault;
        AudioSourceIns.volume /= 3;
        AudioSourceIns.Play();

    }
    public void HealthReducer(byte reduceAmount, GameObject ownerOfHealth)
    {
        ownerOfHealth.GetComponent<GeneralShipScript>().healthOfShip -= reduceAmount;
        if(ownerOfHealth.tag == "Player Ship")
        {
            if(GetComponent<GeneralShipScript>().healthOfShip > 100)
            {
                GetComponent<GeneralShipScript>().healthOfShip = 0;
            }
            gameManagerIns.healthBar.value = ownerOfHealth.GetComponent<GeneralShipScript>().healthOfShip;
            gameManagerIns.healthBarText.text = "" + ownerOfHealth.GetComponent<GeneralShipScript>().healthOfShip;
        }
    }
    void CheckHealth()
    {
        if (GetComponent<GeneralShipScript>().healthOfShip == 0 || GetComponent<GeneralShipScript>().healthOfShip > 100)
        {
            ExplosionFunc(transform.position);
            gameObject.SetActive(false);
            gameManagerIns.GameOver();
        }
    }


}
