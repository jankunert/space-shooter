using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public int HazardsPerWave;
    public float spawnWait;
    public float startWait;
    public float waitBetweenWaves;
    public float numberOfWaves;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOver;
    private  int score;
    private int wavesDone;
    private bool gameOveer;
    private bool restart;

    void Start()
    {
        StartCoroutine(SpawnWaves());
        
        gameOveer = false;
        restart = false;
        score = 0;
        UpdateScore();
        gameOver.text = "";
        restartText.text = "";

    }

    IEnumerator  SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        for (int v = 0; v < numberOfWaves; v++) 
        {
            for (int i = 0; i < HazardsPerWave ; i++) {
                Vector3 spawnPosition = new Vector3(Random.Range(-6.0f, 6.0f), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            wavesDone++;
            yield return new WaitForSeconds(waitBetweenWaves);

        }
    }
    private void Update()
    {
        if (restart ==true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }

        }
        StartCoroutine(ShowRestart());
        StartCoroutine(ShowRestartFail());




    }
    IEnumerator ShowRestartFail()
    {
        if (gameOveer == true)
        {
            yield return new WaitForSeconds(1); 
            restartText.text = "Press 'R' to restart.";
            restart = true;
        }
    }
    IEnumerator  ShowRestart()
    {
        if (wavesDone == numberOfWaves && gameOveer == false)
        {
            yield return new WaitForSeconds(5);
            if(gameOveer==false)restartText.text = "Success! Press 'R' to restart.";
            scoreText.text = "";
            restart = true;

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
        gameOveer = true;
        
        
    }
}

