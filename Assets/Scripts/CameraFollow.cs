using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = gameObject.transform.position - player.transform.position;
        Debug.Log(offset);
	}
	
	// Update is called once per frame
	void LateUpdate() {
        gameObject.transform.position = player.transform.position + offset;
	}
}
