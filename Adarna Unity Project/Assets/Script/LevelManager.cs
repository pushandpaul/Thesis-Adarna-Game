using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public static bool exitInRight = true;
	public static bool isDoor;
	public static int doorIndex = 0;
	//public static bool loadPlayerPos = false;

	public string sceneName;
	private PlayerPosition playerPos;
	public DoorHandler[] door;

	private PlayerController player;
	private CameraController camera;
	private Location location;
	private GameManager gameManager;
	public ObjectData[] objectData;
	void Awake(){
		gameManager = FindObjectOfType<GameManager>();
		objectData = FindObjectsOfType<ObjectData>();

		if(gameManager != null && objectData.Length > 0){
			gameManager.currentScene = sceneName;
			gameManager.updateSceneList();	

		}
	}
	void Start() {
		if(objectData != null){
			gameManager.loadCoordinates(objectData);
		}
			

		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		location = FindObjectOfType<Location>();
		playerPos = FindObjectOfType<PlayerPosition>();
		//door = FindObjectsOfType<DoorHandler>();

		if(playerPos == null ||!playerPos.loadThis){
			if(isDoor && door != null){
				player.transform.position = new Vector3(door[doorIndex].transform.position.x, location.playerSpawnY, location.playerSpawnZ);
				camera.transform.position = new Vector3(door[doorIndex].transform.position.x, location.cameraSpawnY, location.cameraSpawnZ);
			}
			else if(!isDoor){
				if(exitInRight){
					player.transform.position = new Vector3(location.playerSpawnLeftX, location.playerSpawnY, location.playerSpawnZ);
					camera.transform.position = new Vector3(location.cameraSpawnLeftX, location.cameraSpawnY, location.cameraSpawnZ);
				}
				else{
					player.transform.position = new Vector3(location.playerSpawnRightX, location.playerSpawnY, location.playerSpawnZ);
					player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, 0f);
					camera.transform.position = new Vector3(location.cameraSpawnRightX, location.cameraSpawnY, location.cameraSpawnZ);
					camera.flipped = true;
				}
			}
		}
		else if(playerPos.loadThis){
			Debug.Log("There is saved");

			player.transform.position = new Vector3(playerPos.playerX, playerPos.playerY, playerPos.playerZ);
			player.transform.localScale = new Vector3(playerPos.playerScale, playerPos.playerScale, 0f);
			camera.transform.position = new Vector3(playerPos.cameraX, playerPos.cameraY, playerPos.cameraZ);

			playerPos.clearInBetweenData();
		}
	}

	public void savePosition(){
		if(playerPos != null)
			playerPos.saveInBetweenData();
	}
}
