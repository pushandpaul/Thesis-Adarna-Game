using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public static bool exitInRight = true;

	public PlayerController player;
	public CameraController camera;

	public Location location;
	// Use this for initialization
	void Awake () {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		location = FindObjectOfType<Location>();

		if(exitInRight){
			player.transform.position = new Vector3(location.playerSpawnLeftX, location.playerSpawnY, location.playerSpawnZ);
			camera.transform.position = new Vector3(location.cameraSpawnLeftX, location.cameraSpawnY, location.cameraSpawnZ);
		}
		else{
			player.transform.position = new Vector3(location.playerSpawnRightX, location.playerSpawnY, location.playerSpawnZ);
			player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, 0f);
			//camera.transform.position = new Vector3(location.cameraSpawnRightX, location.cameraSpawnY, location.cameraSpawnZ);
			camera.flipped = true;
		}

	}
}
