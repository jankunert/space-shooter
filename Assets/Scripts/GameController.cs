using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject hazard1;
    public GameObject hazard2;
    public GameObject hazard3;
    //public GameObject hazardG;
    private GameObject actHazard;
    private float whichRandomHazard;
    public Vector3 spawnValues;
    public int HazardsPerWave;
    public float spawnWait;
    public float startWait;
    public float waitBetweenWaves;
    public float numberOfWaves;
    public PlayerController player;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOver;
    public GUIText youWon;
    public GUIText finalScore;
    public GUIText notifyC;
    public int score;
    private int wavesDone;
    private bool allWavesDone;
    public bool gameOveer;
    private bool restart;
    private bool count;
    private float floatShowWavesDone;

    void Start()
    {

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        if (player == null)
        {
            Debug.Log("Cannot find PlayerController Script");
        }

        notifyC.text = "";
        allWavesDone = false;

        StartCoroutine(SpawnWaves());
        
        gameOveer = false;
        restart = false;
        count = true;
        
        score = 0;
        UpdateScore();
        gameOver.text = "";
        youWon.text = "";
        restartText.text = "";
        finalScore.text = "";
        

    }

    IEnumerator  SpawnWaves()
    {
        StartCoroutine(ShowNewWaveStart());
        yield return new WaitForSeconds(startWait);
        for (int v = 0; v < numberOfWaves; v++) 
        {
            for (int i = 0; i < HazardsPerWave; i++)
            {
                if (gameOveer == false)
                {
                    whichRandomHazard = (Random.Range(1, 4));
                    if (whichRandomHazard == 1) actHazard = hazard1;
                    if (whichRandomHazard == 2) actHazard = hazard2;
                    if (whichRandomHazard == 3) actHazard = hazard3;
                    Vector3 spawnPosition = new Vector3(Random.Range(-6.0f, 6.0f), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(actHazard, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
            }
            wavesDone++;
            if (wavesDone == numberOfWaves)
            {
                allWavesDone = true;
            }
            StartCoroutine(ShowNewWave());
            yield return new WaitForSeconds(waitBetweenWaves);

        }
    }
    private void Update()
    {
        Reset();
        StartCoroutine(ShowRestart());
        StartCoroutine(ShowRestartFail());
       


    }
    IEnumerator ShowRestartFail()
    {
        if (gameOveer == true&&restart==false)
        {
            yield return new WaitForSeconds(1); 
            restartText.text = "Press 'R' to restart.";
            restart = true;
            scoreText.text = "";
            
            finalScore.text = "Your Score: " + score;
            while(count)
            {
                yield return new WaitForSeconds(1);
                restartText.text = "";
                yield return new WaitForSeconds(1);
                restartText.text = "Press 'R' to restart.";
            }
        }
    }
    IEnumerator  ShowRestart()
    {
        if (wavesDone == numberOfWaves && gameOveer == false)
        {
            yield return new WaitForSeconds(5);
            if (gameOveer == false)
            {

            
                restartText.text = "Success! Press 'R' to restart.";
                restart = true;
                scoreText.text = "";
                player.notify.text = "";
                youWon.text = "You Won!";
                finalScore.text = "Your Score: " + score;
            }
        }
    }
    

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
    public void GameOver()
    {
        gameOver.text = "Game Over!";
        player.notify.text = "";
        notifyC.text = "";
        gameOveer = true;
        
        
    }
    IEnumerator ShowNewWave()
    {
        if (gameOveer == false&&allWavesDone==false)
        {


            yield return new WaitForSeconds(5);
            floatShowWavesDone = wavesDone + 1;
            notifyC.text = "Wave " + floatShowWavesDone;
            yield return new WaitForSeconds(2);
            notifyC.text = "";
        }
    }
    IEnumerator ShowNewWaveStart()
    {
        if (gameOveer == false)
        {
            notifyC.text = "Wave " + 1;
            yield return new WaitForSeconds(2);
            notifyC.text = "";
        }
    }
    private void Reset()
    {
        if (restart == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                count = false;
                Application.LoadLevel(Application.loadedLevel);
            }

        }
    }

} 

