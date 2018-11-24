using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] enemies;
    public GameObject[] locations;
    public static int totalEnemiesInWorld = 0;
    [SerializeField]
    private int maxEnemiesInWorld = 20;
    [SerializeField]
    private float timeBetweenSpawn = 1f;
    private int randomEnemy, randomLocation;
    private float currentTime;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnEnemies", 0f, timeBetweenSpawn);
	}

    private void Update()
    {
        if (PlayerController.Health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (totalEnemiesInWorld <= 3)
            SpawnEnemies();
    }

    // Update is called once per frame
    void SpawnEnemies() {
        if (totalEnemiesInWorld < maxEnemiesInWorld) { 
            randomEnemy = Random.Range(0, enemies.Length);
            randomLocation = Random.Range(0, locations.Length);
            Instantiate(enemies[randomEnemy], locations[randomLocation].transform);
            totalEnemiesInWorld++;
        }
	}
}
