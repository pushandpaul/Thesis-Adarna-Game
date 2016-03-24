using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public static bool exitInRight = true;
	public static bool isDoor;
	public static int doorIndex = 0;
	//public static bool loadPlayerPos = false;

	private PlayerPosition playerPos;
	private GameObject[] door;

	private PlayerController player;
	private CameraController camera;
	private Location location;
	// Use this for initialization
	void Start() {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		location = FindObjectOfType<Location>();
		playerPos = FindObjectOfType<PlayerPosition>();

		if(playerPos == null ||!playerPos.loadThis){
			if(isDoor && door.Length > 0){
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
