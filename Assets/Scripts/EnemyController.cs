using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    private GameObject gameController;
    //private PlayerController playerController;
    private Vector3 enemyLocation, playerLocation;
    private Animator anim;
    [SerializeField]
    private float speed = 2f;
    private bool attacking = false;
    private bool waitHealthDecrease = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Character");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        //playerController = gameController.GetComponent<PlayerController>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        enemyLocation = gameObject.transform.position;
        playerLocation = player.transform.position;

        //Follow the player through the arena 
        if (Vector3.Distance(enemyLocation, playerLocation) >= 3)
        {
            float xDistance = enemyLocation.x - playerLocation.x;
            float zDistance = enemyLocation.z - playerLocation.z;
            if (Mathf.Abs(xDistance) > Mathf.Abs(zDistance))
            {
                if (xDistance < 0)
                    enemyLocation.x += speed * Time.deltaTime;
                else
                    enemyLocation.x -= speed * Time.deltaTime;
            }
            else
            {
                if (zDistance < 0)
                    enemyLocation.z += speed * Time.deltaTime;
                else
                    enemyLocation.z -= speed * Time.deltaTime;
            }
            gameObject.transform.position = enemyLocation;
        }
        else
        {
            //Attack
            if (attacking == false)
            {
                StartCoroutine("Attack");
                //StartCoroutine("PlayExplosion");
            }
            else
            {
                anim.SetBool("nearPlayer", true);
            }
        }
        gameObject.transform.LookAt(player.transform);
    }

    IEnumerator Attack()
    {
        anim.SetBool("attacking", true);
        attacking = true;
        yield return new WaitForSeconds(3); //Wait for a bit before attacking
        anim.SetBool("attacking", false);
        anim.SetBool("nearPlayer", false);
        attacking = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Character" && anim.GetBool("attacking") == true && waitHealthDecrease == false)
        {
            PlayerController.Health -= 1;
            waitHealthDecrease = true;
            if (PlayerController.Health == 2)
                GameObject.FindGameObjectWithTag("HealthOne").SetActive(false);
            else if (PlayerController.Health == 1)
                GameObject.FindGameObjectWithTag("HealthTwo").SetActive(false);
            else
                GameObject.FindGameObjectWithTag("HealthThree").SetActive(false);
            StartCoroutine("PlayerLoseHealth");
        }
        if (collision.tag == "Character")
        {
            Debug.Log(collision.tag);
            Debug.Log(collision.gameObject.GetComponent<Animator>().GetBool("punch"));
            if (collision.gameObject.GetComponent<Animator>().GetBool("punch") == true)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PlayerLoseHealth ()
    {
        yield return new WaitForSeconds(2);
        waitHealthDecrease = false;
    }
}
