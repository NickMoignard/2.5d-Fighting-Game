using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer1 : MonoBehaviour {

    public Camera mainCamera;
    public Player player;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        player = GetComponentInParent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

        // follow players location on map
        Vector3 moveVector = transform.position - player.transform.position;
        moveVector.z = 0;



        mainCamera.transform.Translate(moveVector);

            
	}
}
