// Author: Fatima Nadeem

using UnityEngine;

public class SeabedMovement : MonoBehaviour
/*
    This class takes care of the repetitive movement of the seabed, which gives 
    the illusion that the player bubble is moving forward continuously.
*/
{
    Rigidbody rb;
    Vector3 initialPos; // Initial position

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialPos = transform.position;
    }

    void Update()
    {
        rb.velocity = Vector3.back * (Spawner.instance.GetItemSpeed());

        if (transform.position.z <= 0f)
        {
            transform.position = initialPos;
        }
    }
}
