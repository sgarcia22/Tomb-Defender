using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float maxDistance = 200f;
    private GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Character");
        gameObject.GetComponent<Rigidbody>().AddForce(player.transform.forward * speed);
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) >= maxDistance)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Spawner.totalEnemiesInWorld--;
            PlayerController.Score++;
            Destroy(gameObject);
        }
        //Destroy(this.gameObject);
    }
}
