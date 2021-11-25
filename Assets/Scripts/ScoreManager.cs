// Author: Fatima Nadeem

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
/*
    This singleton class is used to manage both the score for the current 
    active level and the high score.
*/
{
    public static ScoreManager instance;
    
    public Text highScoreText; // High score display on main menu
    public Text inGameHighScoreText; // High score display in-game
    public Text inGameScoreText; // Score display for current active game

    int highScore;
    int inGameScore; // Score for current active game
    Animator inGameScoreAnim; // In-game score display animator
    bool highScoreExceeded; // Bool that dictates if in-game score has exceeded
                            // high score

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        inGameScoreAnim = inGameScoreText.GetComponent<Animator>();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void ResetScore()
    /*
        This function resets current active game score to zero and also resets 
        in-game high score display (for replay).
    */
    {
        inGameScore = 0;

        inGameScoreText.text = inGameScore.ToString();      
        inGameHighScoreText.text = "High Score: " + highScore.ToString();
        
        highScoreExceeded = false;
        inGameScoreText.color = new Color(1f, 0f, 1f); // Pink to highlight that
                                                       // score has not exceeded
    }

    public void AddScore(int score)
    /*
        This function adds obtained score to current active total score and 
        tells when high score is exceeded.
    */
    {
        inGameScore += score;
        inGameScoreAnim.SetTrigger("New Score"); // Playing score increment
                                                 // animation
        inGameScoreText.text = inGameScore.ToString();

        if (inGameScore > highScore)
        {
            inGameHighScoreText.text = "High Score: " + inGameScore.ToString();

            if (!highScoreExceeded)
            {
                highScoreExceeded = true;
                AudioManager.instance.Play("New High Score"); // Playing high
                                                              // score exceeded
                                                              // music
                inGameScoreText.color = new Color(0f, 0.930851f, 1f); 
                // Cyan to highlight that score has exceeded
            }           
        }
    }

    public void UpdateHighScore()
    /*
        This function updates the high score permanently and displays that 
        on main menu.
    */
    {
        if (inGameScore > highScore)
        {
            highScore = inGameScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    public void ResetHighScore()
    /*
        This function resets high score to zero and displays accordingly.
    */
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
