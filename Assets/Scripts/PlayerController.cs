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
    public GUIText tokenCounter;
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
    public GameObject playerExplosion;
    public Transform shotSpawn;
    public Transform WeaponUpgrade1Left;
    public Transform WeaponUpgrade1Right;
    public float fireRate;
    private float shotLvl;
    private bool upgrade1on;
    private bool upgrade1possible;
    
    private float nextFire;
    public int tokens;
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
        tokenCounter.text = "";
        shotLvl = 7;
        upgrade1on = false;
        upgrade1possible = false;
        
        
    }

    void Update()
    {
        Shooting();

        manageTokenNotify();

        StartCoroutine(UpgradeWeapon());


        tokenCounter.text = "Tokens: " + tokens;
        

        StartCoroutine(Upgrade1());
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

    private IEnumerator Upgrade1()
    {
        if (upgrade1possible&&Input.GetKeyDown(KeyCode.G))
        {
            upgrade1on = true;
            notify.text = "";
            tokens--;
            upgrade1possible = false;
            yield return new WaitForSeconds(5);
            upgrade1on = false;
        }

    }
    void changeSize()
    {
        transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        
    }
    private void Shooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + (fireRate * shotLvl);

            if (shotLvl == 7 && !upgrade1on) Instantiate(shot1, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 6 && !upgrade1on) Instantiate(shot1, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 5 && !upgrade1on) Instantiate(shot2, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 4 && !upgrade1on) Instantiate(shot2, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 3 && !upgrade1on) Instantiate(shot3, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 2 && !upgrade1on) Instantiate(shot3, shotSpawn.position, shotSpawn.rotation);
            if (shotLvl == 1 && !upgrade1on) Instantiate(shot4, shotSpawn.position, shotSpawn.rotation);
            if (upgrade1on)
            {
                Instantiate(shot3, WeaponUpgrade1Left.position, WeaponUpgrade1Left.rotation);
                Instantiate(shot3, WeaponUpgrade1Right.position, WeaponUpgrade1Right.rotation);

                ad.Play();

            }
            ad.Play();
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBolt")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    private void manageTokenNotify()
    {
        if (tokens>=1)
        {
            notify.text = "Press G to use two guns for 5s!";
            upgrade1possible = true;
        }
        else
        {
            StartCoroutine(removeNotify());
        }
          
    }
    IEnumerator removeNotify()
    {
        yield return new WaitForSeconds(1);
        notify.text = "";
    }
}