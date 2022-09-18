using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField] private float playerSpeed;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        ControlShip();
    }
    void ControlShip()
    {
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        {
            playerRb.AddForce(Vector3.forward*playerSpeed*Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow))
        {
            playerRb.AddForce(Vector3.forward * -playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow))
        {
            playerRb.AddForce(Vector3.left * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
        {
            playerRb.AddForce(Vector3.left * -playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        CheckPlayerBorders();
    }
    void CheckPlayerBorders()
    {
        if(transform.position.x < -12.2f)
        {
            transform.position = new Vector3(-12.2f,transform.position.y,transform.position.z);
            playerRb.velocity = new(0, 0, playerRb.velocity.z);
        }
        if (transform.position.x > 12.2f)
        {
            transform.position = new Vector3(12.2f, transform.position.y, transform.position.z);
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
}
