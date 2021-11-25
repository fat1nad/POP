// Author: Fatima Nadeem

using System.Collections;
using UnityEngine;

public class GameScreenManager : MonoBehaviour
/*
    This class is used to manage the play/replay, pause, game over, quit and 
    screen switching systems.
*/
{
    public BubbleMovement playerBubble;

    public string[] tags; // Tags of repetitive objects
    
    public GameObject gameOverCanvas;
    public GameObject pauseCanvas;


    bool unlocked; // This dictates if game is frozen/locked or not
    
    bool pauseUnlocked; // This dictates if the game can be paused or not
    
    float cameraZPos; // Camera z position

    void Start()
    {
        cameraZPos = Camera.main.transform.position.z;

        Freeze(); // Freezing the game since the app starts with main menu

        MainMenu(); // Going to main menu
    }

    void Update()
    {
        if (pauseUnlocked && Input.GetButtonDown("Fire2")) 
            // If pause functionality is enabled and Fire2 is pressed
        {
            if (unlocked) // If game is unfrozen/unlocked
            {
                Pause();
            }
            else
            {
                Unpause();
            }           
        }
    }
    
    void Freeze()
    /*
        This function freezes the game for pause, game over and main menu.
    */
    {
        unlocked = false; // Informing this manager that the game is
                          // locked/frozen

        playerBubble.Lock();

        Time.timeScale = 0f; // Locking/freezing any physics oriented objects

        // Locking/freezing instantiated repetitive objects
        GameObject[] objects;
        foreach (string tag in tags)
        {
            objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects)
            {
                obj.GetComponent<Locker>().Lock();
            }
        }

        Spawner.instance.Lock();

        AudioManager.instance.Stop("Atmosphere"); // Stopping BG music
    }

    void Unfreeze()
    /*
        This function unfreezes the game for an active game.
    */
    {
        playerBubble.Unlock();

        Time.timeScale = 1f; // Unlocking/unfreezing any physics oriented
                             // objects

        // Unlocking/unfreezing instantiated repetitive objects
        GameObject[] objects;
        foreach (string tag in tags)
        {
            objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects)
            {
                obj.GetComponent<Locker>().Unlock();
            }
        }

        Spawner.instance.Unlock();

        AudioManager.instance.Play("Atmosphere"); // Playing BG music

        unlocked = true; // Informing this manager that the game is
                         // unlocked/unfrozen
    }

    public void MainMenu()
    /*
        This function takes to main menu - except enabling its canvas because
        that is not done through code.
    */
    {
        pauseUnlocked = false; // Disabling pause functionality
        AudioManager.instance.Play("Main Menu"); // Playing main menu music
    }

    public void Replay()
    /*
        This function plays the game after resetting it.
    */
    {
        playerBubble.ResetNow();

        ScoreManager.instance.ResetScore(); // Resetting in-game score

        Spawner.instance.ResetNow();

        // Shifting instantiated repetitive objects past the end of the route
        GameObject[] objects;
        foreach (string tag in tags)
        {
            objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects)
            {
                Vector3 objPos = obj.transform.position;
                obj.transform.position = new 
                    Vector3(objPos.x, objPos.y, cameraZPos - 3);
            }
        }

        gameOverCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        AudioManager.instance.Stop("Main Menu"); // Stopping main menu music

        Unfreeze();
 
        pauseUnlocked = true; // Enabling pause functionality
    }

    void Pause()
    {
        Freeze();
        pauseCanvas.SetActive(true);
    }

    void Unpause()
    {
        Unfreeze();
        pauseCanvas.SetActive(false);
    }

    public void GameOver()
    {
        pauseUnlocked = false; // Disabling pause functionality
        StartCoroutine(GameOverScreen()); // Enabling game over screen after
                                          // some delay
        Freeze();
        ScoreManager.instance.UpdateHighScore();
    }

    IEnumerator GameOverScreen()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameOverCanvas.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
