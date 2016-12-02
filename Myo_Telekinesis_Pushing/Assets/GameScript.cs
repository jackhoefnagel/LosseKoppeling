using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public static GameScript instance = null;
    public Canvas startCanvas;
    public Canvas GameCanvas;
    public Canvas endCanvas;

    public int Score = 0;
    public Text scoreText;
    public Text timeText;

    public Text endScoreText;
    public Text endTimeText;

    float timeBefore = 0;
    float timer = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startCanvas.enabled = true;
        GameCanvas.enabled = false;
        endCanvas.enabled = false;
    }

    public bool gameStarted = false;
    public bool gameEnded = false;

    void Update()
    {
        //End the game
        if (Input.GetKeyDown(KeyCode.Space) && gameStarted)
        {
            EndGame();
        }

        //Start the game
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
            StartGame();

        //Restart the level
        if (Input.GetKeyDown(KeyCode.R))
            Restart();


        //Time Calculations
        if (!gameStarted)
            timeBefore = Time.time;

        if (!gameEnded && gameStarted)
        {
            timer = Time.time - timeBefore;

            //Update the text
            scoreText.text = (Score).ToString();
            timeText.text = Mathf.Floor(timer / 60).ToString("00") + ":" + Mathf.Floor(timer % 60).ToString("00");
        }
            
    }

    void StartGame()
    {
        Debug.Log("Game Start");
        gameStarted = true;

        startCanvas.enabled = false;
        GameCanvas.enabled = true;
    }

    void EndGame()
    {
        Debug.Log("Game Ended");

        gameEnded = true;
        GameCanvas.enabled = false;
        endCanvas.enabled = true;

        endScoreText.text = Score.ToString();
        endTimeText.text = timeText.text = Mathf.Floor(timer / 60).ToString("00") + ":" + Mathf.Floor(timer % 60).ToString("00");
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
