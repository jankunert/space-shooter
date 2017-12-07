using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private AudioSource ad;
    private Rigidbody rb;
    public Boundary boundary;
    public Transform shotSpawn;
    public PlayerController player;
    public GameController gameController;
    public GameObject shot;
    public float speed;
    public float tilt;
    public float horizontalSpeed;
    public float verticalSpeed;
    private float moveHorizontal;
    private Vector3 movement;
    

    void Start () {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        if (player == null)
        {
            Debug.Log("Cannot find PlayerController Script");
        }


        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find gameController Script");
        }


        ad = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Shoot());
    }
	
	void Update () {

        StartCoroutine(Destroying());

    }
    private void FixedUpdate()
    {
        if (gameController.gameOveer==false)
        {
            ApplyForce();
        }
        else
        {
            StartCoroutine(Destroying());
        }

    }
    private void ApplyForce()
    {
        
        moveHorizontal = player.transform.position.x * horizontalSpeed;
        float moveVertical = verticalSpeed*-10;
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement*speed*0.01f;
        rb.position = new Vector3
            (Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 180, rb.velocity.x * -tilt);

    }
    private IEnumerator Shoot()
    {
        bool d = true;
        while (d=true&&gameController.gameOveer==false)
        {
            yield return new WaitForSeconds(1);
            ad.Play();
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            yield return new WaitForSeconds(1);
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
