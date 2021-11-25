// Author: Fatima Nadeem

using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    Locker lck; // Locker used to lock/unlock this collectible
    float routeEnd; // z position at which route ends
    Rigidbody rb;
    bool notReached;

    void Start()
    {
        lck = GetComponent<Locker>();

        routeEnd = Camera.main.transform.position.z - 2;

        notReached = true;
    }

    void Update()
    {
        if (lck.IsUnlocked())
        {
            InformSpawner();
        }      
    }

    void InformSpawner()
    /*
        This function informs the spawner that this collectible is past the end 
        of its route and is used up/ready to be reused
    */
    {
        if (notReached && (transform.position.z < routeEnd)) 
            // If this collectible is past the end of the route
        {
            notReached = false;
            rb.velocity = Vector3.zero;
            Spawner.instance.collectibleUsed(this.gameObject);
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
        GetComponent<Renderer>().enabled = true;
        rb.velocity = Vector3.back * speed;
        notReached = true;
    }

    public void ShiftToRouteEnd()
    /*
        This function is called by the player bubble. It shifts this 
        collectible to the end of the route on death, for reuse
    */
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, routeEnd);
    }
}
