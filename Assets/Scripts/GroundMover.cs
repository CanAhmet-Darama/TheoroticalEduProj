using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
    Vector3 startingPosition;
    float distanceToRefresh;
    public static float groundSpeed;
    void Start()
    {
        startingPosition = transform.position;
        groundSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        MoveGround();
    }
    void MoveGround()
    {
        transform.Translate(new Vector3(0,0,-groundSpeed*Time.deltaTime));
        CheckIfPassedMuch();
    }
    void CheckIfPassedMuch()
    {
        if(transform.position.z <(startingPosition.z - 50))
        {
            transform.position = startingPosition;
        }
    }
}
