using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector3 shootPos;
    float bulletRange = 25;
    public GameObject shooterOfIt;
    GameManager gameManagerIns;
    void OnEnable()
    {
        shootPos = transform.position;
        gameManagerIns = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        BulletFunc();
    }
    void BulletFunc()
    {
        IsGoneFarBullet();
    }
    void IsGoneFarBullet()
    {
        if((transform.position - shootPos).magnitude > bulletRange)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy Bullet"))
        {
            Instantiate(gameManagerIns.ammoCrashParticle, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
