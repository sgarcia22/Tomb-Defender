using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject player;
    public GameObject arrow;
    [SerializeField]
    private float speed = 3f;
    private Animator anim;
    private bool attacking = false;
    private bool punching = false;

    public static int Health = 3;

    // Use this for initialization
    void Start () {
 
        anim = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = player.transform.position;
        //Move forwards
		if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //pos.z += distance * Time.deltaTime;
            //player.transform.LookAt(Vector3.forward, Vector3.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.back * speed * Time.deltaTime);
           // player.transform.LookAt(camera.transform, Vector3.up);
            //pos.z -= distance * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Rotate(0, 5, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Rotate(0, -5, 0);
        }
        if (Input.GetKey(KeyCode.E) && attacking == false)
        {
            Debug.Log("happening");
            anim.SetBool("attack", true);
            attacking = true;
            StartCoroutine("BowWait");
        }

        if (Input.GetKey(KeyCode.Q) && punching == false)
        {
            anim.SetBool("punch", true);
            punching = true;    
            StartCoroutine("PunchWait");
        }

        //player.transform.Translate(pos * Time.deltaTime);
        if (pos != player.transform.position)
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
       

    }

    IEnumerator BowWait ()
    {
        
        yield return new WaitForSeconds(1);
        //Spawn the arrow
        Vector3 playerPos = player.transform.position;
        playerPos.y += 1.35f;
        GameObject arrowSpawn = Instantiate(arrow, playerPos, player.transform.rotation) as GameObject;
        //Set the booleans
        attacking = false;
        anim.SetBool("attack", false);
    }

    IEnumerator PunchWait()
    {
        yield return new WaitForSeconds(1);
        punching = false;
        anim.SetBool("punch", false);
    }


}
