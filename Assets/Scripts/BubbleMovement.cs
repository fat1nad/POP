// Author: Fatima Nadeem

using UnityEngine;

public class BubbleMovement : MonoBehaviour
/*
    This class is responsible for player bubble's movement, actions, 
    interactions, death and revival.
*/
{
    public GameScreenManager gameManager;
    public PopTextToggle popText;

    bool unlocked;
    Camera mainCamera;

    ParticleSystem trailParticles; // Bubbly trail behind the player bubble
    ParticleSystem popParticles; // Particle system for pop animation
    Renderer rend; // For visual death
    Animator anim; // For collectible eating animation

    void Awake()
    {
        mainCamera = Camera.main;

        trailParticles = transform.Find("Trail Particles").
            GetComponent<ParticleSystem>();
    }

    void Start()
    {
        popParticles = transform.Find("Pop Particles").
            GetComponent<ParticleSystem>();
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (unlocked)
        {
            Move();
        }       
    }

    void Move()
    /*
        This function is used to move the player/main bubble using mouse 
        movement.
    */
    {
        Vector3 vpPos = mainCamera.WorldToViewportPoint(transform.position);
        // player bubble position in viewport

        Vector3 mousePos = mainCamera.ScreenToViewportPoint
            (Input.mousePosition); // mouse position in viewport

        // Clamping to within visual bounds
        mousePos.x = Mathf.Clamp01(mousePos.x);
        mousePos.y = Mathf.Clamp01(mousePos.y);

        // Setting player bubble position in world according to clamped mouse
        // position
        transform.position = mainCamera.ViewportToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            vpPos.z));
    }

    void OnTriggerEnter(Collider other)
    {
        if (unlocked)
        {
            if (other.CompareTag("Obstacle")) // If player bubble hit obstacle
            {
                // Making player bubble pop
                popParticles.Play();
                rend.enabled = false;
                popText.FollowAndAppear(transform.position);
                AudioManager.instance.Play("Pop");

                gameManager.GameOver();
            }
            else if (other.CompareTag("Collectible")) // If player bubble hits
                                                      // collectible
            {
                ScoreManager.instance.AddScore(1);

                // Making player bubble absorb collectible
                anim.SetTrigger("Absorb Bubble");
                AudioManager.instance.Play("Eat Bubble");
                other.GetComponent<Renderer>().enabled = false;

                // Informing spawner that this collectible can be reused
                other.GetComponent<CollectibleMovement>().ShiftToRouteEnd();
            }
        }     
    }

    public void Lock()
    {
        unlocked = false;
        trailParticles.Stop();
    }

    public void Unlock()
    {
        trailParticles.Play();
        unlocked = true;
    }

    public void ResetNow()
    {
        transform.position = Vector3.zero;
        rend.enabled = true;
        popText.Disappear();
    }
}
