using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {

	public float playerX;
	public float playerY;
	public float playerZ;
	public float playerScale;

	public float cameraX;
	public float cameraY;
	public float cameraZ;

	public bool loadThis = false;

	private CameraController camera;
	private PlayerController player;
	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad(this);
	}
	public void saveInBetweenData(){
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		playerX = player.transform.position.x;

		playerY = player.transform.position.y;
		playerZ = player.transform.position.z;
		playerScale = player.transform.localScale.x;

		cameraX = camera.transform.position.x;
		cameraY = camera.transform.position.y;
		cameraZ = camera.transform.position.z;

		this.loadThis = true;
	}

	public void clearInBetweenData(){
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();

		playerX = 0f;
		playerY = 0f;
		playerZ = 0f;
		playerScale = 0f;

		cameraX = 0f;
		cameraY = 0f;
		cameraZ = 0f;

		this.loadThis = false;
	}
}
