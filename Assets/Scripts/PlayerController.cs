using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable ]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public GUIText notify;
    public float speed;
    public float tilt;
    public float upgradeAfterWhichScore;
    private Rigidbody rb;
    private AudioSource ad;
    public Boundary boundary;
    public GameObject shot1;
    public GameObject shot2;
    public GameObject shot3;
    public GameObject shot4;
    public Transform shotSpawn;
    public float fireRate;
    private float shotLvl;
    
    private float nextFire;
    private float scoreNow;
    private Color color = Color.cyan;




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


        rb = GetComponent<Rigidbody>();
        ad = GetComponent<AudioSource>();

        notify.text = "";
        shotLvl = 7;
        scoreNow = gameController.score;
        
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + (fireRate*shotLvl);

            if(shotLvl==7) Instantiate(shot1, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 6) Instantiate(shot1, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 5) Instantiate(shot2, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 4) Instantiate(shot2, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 3) Instantiate(shot3, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 2) Instantiate(shot3, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 1) Instantiate(shot4, shotSpawn.position, shotSpawn.rotation);
            ad.Play();
        }

        StartCoroutine(UpgradeWeapon());

        if (Input.GetKeyDown(KeyCode.F))
        {

        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement*speed);
        rb.position = new Vector3
            (Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax) , 
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f,rb.velocity.x*-tilt);
    }
    IEnumerator UpgradeWeapon()
    {
        if (gameController.score >= scoreNow+upgradeAfterWhichScore&&shotLvl>1)
        {
            notify.text = "Your weapon has been upgraded!";
            shotLvl = shotLvl-1;
            scoreNow = gameController.score;
            yield return new WaitForSeconds(2);
            notify.text = "";
            if (shotLvl==1)
            {
                StartCoroutine(BestWeapon());
            }
        }
    }
    IEnumerator BestWeapon()
    {
        notify.color = color;
        notify.fontSize = 40;
        notify.text = "You have the best weapon now!";
        yield return new WaitForSeconds(2);
        notify.text = "";
    }
}