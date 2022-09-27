using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector3 shootPos;
    float bulletRange = 25;
    public GameObject shooterOfIt;
    void OnEnable()
    {
        shootPos = transform.position;
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
}
