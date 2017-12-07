using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {


    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    public bool thisEnemyGivesYouTokens;
    public GameController gameController;
    public PlayerController player;


    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find gameController Script");
        }


        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        if (player == null)
        {
            Debug.Log("Cannot find PlayerController Script");
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Astroid"&&other.tag!="EnemyBolt"&&other.tag!="Enemy")  
        {
            


            if (other.tag == "Boundary")
            {
                return;
            }

            Instantiate(explosion, transform.position, transform.rotation);
            if (other.tag == "Player")
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.AddScore(-scoreValue);
                gameController.GameOver();
            }

            gameController.AddScore(scoreValue);
            if (thisEnemyGivesYouTokens) player.tokens++;
            Destroy(other.gameObject);
            Destroy(gameObject);
            
        }
    }



}

