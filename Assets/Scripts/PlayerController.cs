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
    public static byte healthOfPlayer;
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
        healthOfPlayer = 100;
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
    /*IEnumerator ManageBullet(GameObject bulletToShoot)
    {
        bulletToShoot

        GameObject bulletInUse = Instantiate(bulletPrefab, new Vector3(transform.position.x, 0.3f, transform.position.z), transform.rotation);
        AudioSourceIns.Play();
        bulletInUse.GetComponent<Rigidbody>().velocity = new Vector3(0,0,bulletSpeed);
        bulletInUse.transform.Find("Bullet Trail").GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        Destroy(bulletInUse);
    }*/

    /*public GameObject GetABullet(GameObject[] BulletPrefabs)
    {
        byte ammoNum = (byte)BulletPrefabs.Length;
        for (byte i = 0; i < ammoNum; i++)
        {
            if (!BulletPrefabs[i].activeInHierarchy)
            {
                Debug.Log("Bullet Chosen");
                return BulletPrefabs[i];
            }
        }
        return gameManagerIns.emptyForNull;
    }
    public void ShootBullet(GameObject[] BulletPrefabs)
    {
        GameObject bulletToShoot = GetABullet(BulletPrefabs);
        if (bulletToShoot != gameManagerIns.emptyForNull)
        {
            Debug.Log("Bullet Shot");
            bulletToShoot.SetActive(true);
            bulletToShoot.transform.position = transform.position;
            bulletToShoot.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletSpeed;
            AudioSourceIns.Play();
        }
        else
        {
            if(this.gameObject.name == "PlayerShip")
            {
                Debug.Log("Player is out of ammo");
                AudioSourceIns.clip = noAmmoSound;
                AudioSourceIns.Play();
                AudioSourceIns.clip = shootSound;
            }
        }
    }*/

   /* public void ShootABullet(GameObject[] bulletsToShoot, float coolDown)
    {
        byte ammoAmount = (byte)bulletsToShoot.Length;
        GameObject bulletToShoot = gameManagerIns.emptyForNull;
        for (int i = 0; i < ammoAmount; i++)
        {
            if (bulletsToShoot[i].activeSelf == true)
            {
                Debug.Log("Bullet '" +(i+1)+ "' is not available");
            }
            else
            {
                bulletToShoot = bulletsToShoot[i];
                //Debug.Log("Bullet Decided : " + bulletToShoot.name);
                break;
            }

        }

        if(bulletToShoot.CompareTag("Bullet") && canShoot)
        {
            //Debug.Log("Bullet Shot : " + bulletToShoot.name);
            bulletToShoot.SetActive(true);
            bulletToShoot.SetActive(true);
            Debug.Log(bulletToShoot.active);
            bulletToShoot.transform.position = transform.position;
            bulletToShoot.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletSpeed;
            AudioSourceIns.clip = shootSound;
            AudioSourceIns.volume = volumeDefault;
            AudioSourceIns.Play();
            StartCoroutine(AddCooldown(coolDown));
        }
        else
        {
            if (this.gameObject.name == "PlayerShip")
            {
                AudioSourceIns.volume = volumeDefault;
                Debug.Log("Player is out of ammo");
                AudioSourceIns.clip = noAmmoSound;
                AudioSourceIns.volume /= 3;
                AudioSourceIns.Play();
            }
        }
    }*/
    public void ShootABullet(GameObject bulletOfShot, float cooldown, GameObject shooter, AudioClip shootSoundX, float bulletSpeedG, float extraZAmount)
    {
        if (shooter.GetComponent<GeneralShipScript>().canShoot)
        {
            GameObject shotBullet = Instantiate(bulletOfShot, new Vector3(shooter.transform.position.x,0.25f, shooter.transform.position.z + extraZAmount), bulletOfShot.transform.rotation);
            shotBullet.GetComponent<BulletScript>().shooterOfIt = shooter.gameObject;
            switch (shooter.tag)
            {
                case "Player Ship":
                    AudioSourceIns.clip = shootSoundX;
                    AudioSourceIns.volume = volumeDefault;
                    AudioSourceIns.Play();
                    break;
                case "Enemy Ship":
                    SecondaryAudioSource.clip = shootSoundX;
                    SecondaryAudioSource.volume = volumeDefault;
                    SecondaryAudioSource.Play();
                    break;
            }
            AudioSourceIns.clip = shootSoundX;
            AudioSourceIns.volume = volumeDefault;
            AudioSourceIns.Play();
            shotBullet.GetComponent<Rigidbody>().velocity = new Vector3(0,0,bulletSpeedG);
            StartCoroutine(AddCooldown(cooldown, shooter));
        }
        else
        {
            if (this.gameObject.name == "PlayerShip")
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
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            ExplosionFunc(this.transform.position);
            HealthReducer(healthOfPlayer);
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
    public void HealthReducer(byte reduceAmount)
    {
        healthOfPlayer -= reduceAmount;
        gameManagerIns.healthBar.value = healthOfPlayer;
        gameManagerIns.healthBarText.text = "" + healthOfPlayer;
    }
}
