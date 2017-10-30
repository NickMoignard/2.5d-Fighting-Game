using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int lives = 3;
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
    private int totalNoFighters;

    // Use this for initialization
    void Start () {
        
        spawnRotation.SetFromToRotation(new Vector3(0,0,-1), new Vector3(-1,0,0));
        SpawnPlayer();
        
        mainCamera = transform.Find("Camera").gameObject;
        

        cameraInitPos = mainCamera.transform.position;
        playerInitPos = player2.transform.position;

        // lock camera to the player
        moveCameraVector = new Vector3(playerInitPos.x - cameraInitPos.x, 0, 0);
        mainCamera.transform.Translate(moveCameraVector);

        Debug.Log(cameraInitPos + " <-- camera }{ player --> " + playerInitPos);
    }



    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null && player2 != null)
        {
            Vector3 player2Pos = player2.transform.position;
            Vector3 camPos = mainCamera.transform.position;
            moveCameraVector = new Vector3(player2Pos.x - camPos.x, player2Pos.y - camPos.y + cameraVerticalDistancetoPlayer, 0);
            mainCamera.transform.Translate(moveCameraVector * Time.deltaTime * CAMERA_SPEED);
        } else
        {
            checkGameState();
        }
        
    }

    void SpawnPlayer()
    {
        
        // Load Player 
        Fighter clone = Instantiate(playerPrefab, spawn1, spawnRotation) as Fighter;
        clone.transform.parent = transform;
        player2 = clone.gameObject;
        
    }

    void DestroyPlayer1()
    {
        Debug.Log("whats up people");
        Destroy(player2);
    }

    void checkGameState()
    {
        Debug.Log(GetComponentsInChildren<Fighter>().Length);
        if (GetComponentsInChildren<Fighter>().Length < totalNoFighters)
        {
            // we have correct number of fighters
        } else
        {
            SpawnPlayer();
        }
    }


    private void OnEnable()
    {
        EventManager.StartListening("Destroy_Player_1", DestroyPlayer1);
    }
    private void OnDisable()
    {
        EventManager.StopListening("Destroy_Player_1", DestroyPlayer1);
    }

}
