// Author: Fatima Nadeem

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
/*
    This singleton class is used to spawn obstacles and collectibles at 
    different intervals.
*/
{
    public static Spawner instance;


    public GameObject obstacle; // Obstacle prefab to spawn
    public float initialObstacleSpnGap; // Initial interval between each
                                        // obstacle spawn
    public float obstacleSpnGapDec; // Decrement in the interval between
                                    // each obstacle spawn
    public float minObstacleSpnGap; // Min interval between each obstacle spawn

    public GameObject collectible; // Collectible prefab to spawn
    public float collectibleSpnGap; // Interval between each collectible spawn

    public float SpawnPointZ; // Spawn point Z value

    [Range(0f, 10f)]
    public float spawnAreaFrustumPos; // Z position of spawn area(only in XY
                                      // plane) frustum
    
    public float initialSpeed; // Initial speed of each obstacle or collectible
                               // spawned
    public float speedIncrement; // Increment in the speed of each obstacle or
                                 // collectible spawned


    bool unlocked;

    Camera mainCamera;
    float[] spawnArea; // array with xMin, xMax, yMin and yMax of spawn area

    bool allowObstacle; // Bool to control obstacle spawn rate
    bool allowCollectible; // Bool to control collectible spawn rate

    Stack<GameObject> reachedObstacles; // Obstacles that have crossed near
                                        // clip plane
    Stack<GameObject> reachedCollectibles; // Collectibles that have crossed
                                           // near clip plane

    float itemSpeed; // Speed of each obstacle or collectible spawned
    float obstacleSpnGap; // Interval between each obstacle spawn

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;

        Vector3[] frustumCorners = new Vector3[4];
        mainCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1),
            spawnAreaFrustumPos, 
            Camera.MonoOrStereoscopicEye.Mono, frustumCorners); 
            // frustum corners at player bubble position

        spawnArea = new float[4];
        spawnArea[0] = frustumCorners[0].x; // xMin
        spawnArea[1] = frustumCorners[2].x; // xMax
        spawnArea[2] = frustumCorners[0].y; // yMin
        spawnArea[3] = frustumCorners[1].y; // yMax

        reachedObstacles = new Stack<GameObject>();
        reachedCollectibles = new Stack<GameObject>();
    }

    void Update()
    {
        if (unlocked)
        {
            StartCoroutine(Spawn());

            itemSpeed += (speedIncrement * Time.deltaTime); // Increasing speed
                                                            // of each spawned
                                                            // item

            if (obstacleSpnGap > minObstacleSpnGap)
            {
                obstacleSpnGap -= (obstacleSpnGapDec * Time.deltaTime);
                // Reducing interval between each obstacle spawn until minimum
                // defined threshold
            }
        }       
    }

    IEnumerator Spawn()
    {
        if (allowObstacle)
        {
            allowObstacle = false;

            SpawnObstacle();
            yield return new WaitForSeconds(obstacleSpnGap);

            allowObstacle = true;
        }

        if (allowCollectible)
        {
            allowCollectible = false;

            SpawnCollectible();
            yield return new WaitForSeconds(collectibleSpnGap);

            allowCollectible = true;
        }
    }

    void SpawnObstacle()
    {
        Vector3 pos = RandomSpawnPoint(); // Generating random spawn point

        GameObject obstacleInstance;
        if (reachedObstacles.Count <= 0) // If no obstacles have crossed near
                                         // clip plane
        {
            // Creating a new obstacle
            obstacleInstance = Instantiate(obstacle, pos, Quaternion.identity);
            
            obstacleInstance.GetComponent<ObstacleMovement>().
                SetSpeed(itemSpeed); // Setting obstacle speed
        }
        else
        {
            // Using an existing obstacle that has crossed near clip plane
            obstacleInstance = reachedObstacles.Pop();
            obstacleInstance.transform.position = pos;

            obstacleInstance.GetComponent<ObstacleMovement>().
                ResetUsedStatus(itemSpeed);
                // Resetting used status
        }
    }

    void SpawnCollectible()
    {
        Vector3 pos = RandomSpawnPoint(); // Generating random spawn point

        GameObject collectibleInstance;
        if (reachedCollectibles.Count <= 0) // If no collectibles have crossed
                                            // near clip plane
        {
            // Creating a new collectible
            collectibleInstance = Instantiate(collectible, pos, Quaternion.
                identity);

            collectibleInstance.GetComponent<CollectibleMovement>().
                SetSpeed(itemSpeed); // Setting collectible speed
        }
        else
        {
            // Using an existing collectible that has crossed near clip plane
            collectibleInstance = reachedCollectibles.Pop();
            collectibleInstance.transform.position = pos;

            collectibleInstance.GetComponent<CollectibleMovement>().
                ResetUsedStatus(itemSpeed);
                // Resetting used status
        } 
    }

    Vector3 RandomSpawnPoint()
    {
        return new Vector3(
            Random.Range(spawnArea[0], spawnArea[1]),
            Random.Range(spawnArea[2], spawnArea[3]),
            SpawnPointZ);
    }

    public void obstacleUsed(GameObject obs)
    {
        reachedObstacles.Push(obs);
    }

    public void collectibleUsed(GameObject col)
    {
        reachedCollectibles.Push(col);
    }

    public float GetItemSpeed()
    {
        return itemSpeed;
    }

    public void ResetNow()
    {
        StopAllCoroutines();
        
        allowObstacle = true;
        allowCollectible = true;

        itemSpeed = initialSpeed;
        obstacleSpnGap = initialObstacleSpnGap;
    }

    public void Lock()
    {
        unlocked = false;
    }

    public void Unlock()
    {
        unlocked = true;
    }
}
