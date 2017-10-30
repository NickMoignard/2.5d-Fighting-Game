using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public GameObject mainCamera;
    public GameObject player2;
    public Fighter playerPrefab;
    public float CAMERA_SPEED;

    private Vector3 cameraInitPos;
    private Vector3 playerInitPos;
    private Vector3 moveCameraVector;
    public float cameraVerticalDistancetoPlayer;
    public Vector3 spawn1 = new Vector3(-1, 0, 0);
    public Vector3 spawn2 = new Vector3(1, 0, 0);
    public Quaternion spawnRotation = Quaternion.identity;

    // Use this for initialization
    void Start () {
        //SpawnPlayer();


        mainCamera = transform.Find("Camera").gameObject;
        player2 = transform.Find("Player 2").gameObject;

        cameraInitPos = mainCamera.transform.position;
        playerInitPos = player2.transform.position;

        // lock camera to the player
        moveCameraVector = new Vector3(playerInitPos.x - cameraInitPos.x, 0, 0);
        mainCamera.transform.Translate(moveCameraVector);

        Debug.Log(cameraInitPos + " <-- camera }{ player --> " + playerInitPos);
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 player2Pos = transform.Find("Player 2").transform.position;
        Vector3 camPos = transform.Find("Camera").transform.position;


        moveCameraVector = new Vector3(player2Pos.x - camPos.x, player2Pos.y - camPos.y + cameraVerticalDistancetoPlayer, 0);
        mainCamera.transform.Translate(moveCameraVector * Time.deltaTime * CAMERA_SPEED);
    }

    void SpawnPlayer()
    {
        // Load Player 1

        Fighter player2 = Instantiate(playerPrefab, spawn1, spawnRotation) as Fighter;
    }

    void DestroyPlayer1()
    {
        Debug.Log("whats up people");
        DestroyImmediate(player2);
    }

    void OnEnable()
    {
        EventManager.StartListening("Destroy_Player_1", DestroyPlayer1);
    }
    private void OnDisable()
    {
        EventManager.StopListening("Destroy_Player_1", DestroyPlayer1);
    }

}
