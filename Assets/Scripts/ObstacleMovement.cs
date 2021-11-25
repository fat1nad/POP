// Author: Fatima Nadeem

using UnityEngine;
using System.Collections;

public class ObstacleMovement : MonoBehaviour
{
    public float rotationSpeed;

    Locker lck; // Locker used to lock/unlock this obstacle
    float routeEnd; // z position at which route ends
    Rigidbody rb;
    bool notReached;

    void Start()
    {
        lck = GetComponent<Locker>();

        Camera mainCamera = Camera.main;
        routeEnd = mainCamera.transform.position.z - 2;

        notReached = true;
    }

    void Update()
    {
        if (lck.IsUnlocked()) // If this obstacle is unlocked
        {
            Rotate();
            InformSpawner();
        }
    }

    void Rotate()
    /*
        This function is used for the obstacle's rotation animation.
    */
    {
        transform.Rotate(new Vector3(rotationSpeed, 0, rotationSpeed)
            * Time.deltaTime);
    }

    void InformSpawner()
    /*
        This function informs the spawner that this obstacle is past the end of
        its route and is used up/ready to be reused
    */
    {
        if (notReached && (transform.position.z < routeEnd)) 
            // If this obstacle is past the end of the route
        {
            notReached = false;
            rb.velocity = Vector3.zero;
            Spawner.instance.obstacleUsed(this.gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.back * speed;
    }

    public void ResetUsedStatus(float speed)
    /*
        This function is called by the spawner and is used to reset used status
    */
    {
        rb.velocity = Vector3.back * speed;
        notReached = true;
    }
}
