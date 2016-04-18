using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	public static bool exitInRight = true;
	public static bool isDoor;
	public static int doorIndex = 0;
	//public static bool loadPlayerPos = false;

	public string sceneName;
	private PlayerPosition playerPos;
	public DoorHandler[] door;

	private PlayerController player;
	private Animator playerAnimator;
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
		
		int j = 0;
		int i = 0;
		float xPosition = 0f;
		GameObject[] LightHolder; 
	
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		location = FindObjectOfType<Location>();
		playerPos = FindObjectOfType<PlayerPosition>();
		LightHolder = GameObject.FindGameObjectsWithTag("Character Light");

		//Level Initialization
		if(objectData.Length > 0){
			gameManager.loadCoordinates(objectData);
		}

		changeTimeOfDay(gameManager.timeOfDay);

		if(gameManager.timeOfDay == 'd' || location.isInterior){
			foreach(GameObject lightHolder in LightHolder)
				lightHolder.GetComponent<Light>().intensity = 0f;
		}

		//Player Initialization
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
					player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, 1f);
					camera.transform.position = new Vector3(location.cameraSpawnRightX, location.cameraSpawnY, location.cameraSpawnZ);
					camera.flipped = true;
				}
			}
		}
		else if(playerPos.loadThis){
			Debug.Log("There is saved");

			player.transform.position = new Vector3(playerPos.playerX, playerPos.playerY, playerPos.playerZ);
			player.transform.localScale = new Vector3(playerPos.playerScale, player.transform.localScale.y, 1f);
			camera.transform.position = new Vector3(playerPos.cameraX, playerPos.cameraY, playerPos.cameraZ);

			playerPos.clearInBetweenData();
		}

		//Follower Initialization
		if(gameManager.Followers != null){
			foreach(GameObject follower in gameManager.Followers){
				j += 3;
				i += 2;
				Debug.Log(player.transform.localScale.ToString());
				follower.GetComponent<FollowTarget>().thisConstructor(player.moveSpeed, j, player.transform, player.transform.localScale);
				if(player.transform.localScale.x < 0)
					xPosition = player.transform.position.x + i;
				else if(player.transform.localScale.x > 0)
					xPosition = player.transform.position.x - i;
				follower.transform.position = new Vector3(xPosition, follower.transform.position.y, follower.transform.position.z);
			}
		}
	}
		
	public void savePosition(){
		if(playerPos != null)
			playerPos.saveInBetweenData();
	}

	public void changeTimeOfDay(char timeOfDay){
		LightController globalLight;
		GameObject globaLightHolder;

		globaLightHolder = GameObject.FindWithTag("Global Light");

		if(globaLightHolder != null){
			globalLight = globaLightHolder.GetComponent<LightController>();
			if(timeOfDay == 'd' && globalLight.lightIntensity != 1.8f){
				globalLight.setLightIntensity(1.8f);
				Debug.Log("Global Light Intensity adjusted to day time");
			}
			else if(timeOfDay == 'n' && globalLight.lightIntensity != 0f){
				globalLight.setLightIntensity(0f);
				Debug.Log("Global Light Intensity adjusted to night time");
			}
		}
	}

	/*public void fadeTimeOfDay(char targetTimeOfDay, float duration){
		LightController globalLight;
		GameObject globaLightHolder;
		float targetLightIntensity = 0f;

		globaLightHolder = GameObject.FindWithTag("Global Light");
		if(globaLightHolder != null){
			globalLight = globaLightHolder.GetComponent<LightController>();
			if(targetTimeOfDay == 'd'){
				targetLightIntensity  = 1.8f;
			}
			else if(targetTimeOfDay == 'n' && location.isInterior){
				targetLightIntensity = 0f;
			}
			else if(targetTimeOfDay == 'n' && !location.isInterior){
				targetLightIntensity = 0.8f;
			}
			globalLight.fadeLightIntensity(targetLightIntensity, duration);
		}
	}*/
}
