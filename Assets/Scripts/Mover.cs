using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    
    public float speed;
    private Vector3 correctedPosition;
    public GameController gameController;
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


        GetComponent<Rigidbody >().velocity = transform.forward*speed;
    }
    private void Update()
    {
        StartCoroutine(Destroying());
        if (transform.position.y<-0.3||transform.position.y>0.3)
        {

            correctedPosition = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = correctedPosition;
        }
    }
    IEnumerator Destroying()
    {
        if (gameController.gameOveer == true)
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
    }
}
