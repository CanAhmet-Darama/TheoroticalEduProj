using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralShipScript : MonoBehaviour
{
    public bool canShoot;
    public bool isBigEnemy;
    public float bulletSpeed;
    public byte healthOfShip;
    public byte damageOfShip;
    void Start()
    {
        canShoot = true;
    }
}
