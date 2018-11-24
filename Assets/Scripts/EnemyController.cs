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
    private bool nearPlayer = false;

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
            nearPlayer = false;
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
            nearPlayer = true;
            //Attack
            if (attacking == false)
            {
                StartCoroutine("WaitForAttack");
                //StartCoroutine("PlayExplosion");
            }
            else
            {
                anim.SetBool("nearPlayer", true);
            }
        }
        gameObject.transform.LookAt(player.transform);
    }

    IEnumerator WaitForAttack()
    {
        anim.SetBool("attacking", true);
        attacking = true;
        yield return new WaitForSeconds(2); //Wait for a bit before attacking
        anim.SetBool("attacking", false);
        anim.SetBool("nearPlayer", false);
        attacking = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Character" && attacking == true && waitHealthDecrease == false)
        {
            StartCoroutine("WaitForHealthDecrease", collision.gameObject);
        }
        if (collision.tag == "Character")
        {
            Debug.Log("Hitting Character");
            if (collision.gameObject.GetComponent<Animator>().GetBool("punch") == true)
            {
                
                Destroy(gameObject);
                PlayerController.Score++;
                Spawner.totalEnemiesInWorld--;
            }
        }
    }

    //Wait to decrease the next health heart
    IEnumerator PlayerLoseHealth ()
    {
        yield return new WaitForSeconds(2);
        waitHealthDecrease = false;
    }

    //Used to stop the enemy from inflicting damage from the get go
    IEnumerator WaitForHealthDecrease (GameObject player)
    {
        yield return new WaitForSeconds(1);
        if (nearPlayer && waitHealthDecrease == false)
        {
            gameObject.GetComponent<AudioSource>().Play();
            PlayerController.Health -= 1;
            if (PlayerController.Health == 2)
                GameObject.FindGameObjectWithTag("HealthOne").SetActive(false);
            else if (PlayerController.Health == 1)
                GameObject.FindGameObjectWithTag("HealthTwo").SetActive(false);
            else
                GameObject.FindGameObjectWithTag("HealthThree").SetActive(false);
            player.GetComponent<AudioSource>().Play();
            waitHealthDecrease = true;
            StartCoroutine("PlayerLoseHealth");
        }
    }
}
