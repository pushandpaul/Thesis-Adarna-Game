﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Local class serves as initialization of the level and connection to the global class - Game Manager

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
	
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		location = FindObjectOfType<Location>();
		playerPos = FindObjectOfType<PlayerPosition>();
		FollowerManager followerManager = FindObjectOfType<FollowerManager>();
		//Level Initialization
		if(objectData.Length > 0){
			gameManager.loadCoordinates(objectData);
		}

		changeTimeOfDay(gameManager.timeOfDay);
		characterLightSwitch();

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

		if(followerManager != null)
			followerManager.setActiveFollowers();
		//Change character avatar
		instantChangePlayer(gameManager.currentCharacterName);
	}

	public void unclonedInstace(GameObject toInstantiate, Vector3 position){
		GameObject spawedObject = (GameObject)Instantiate(toInstantiate, position, Quaternion.Euler(0,0,0));
		spawedObject.name = toInstantiate.name;
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

	void characterLightSwitch(){
		GameObject[] LightHolder = GameObject.FindGameObjectsWithTag("Character Light");
		if(gameManager.timeOfDay == 'd' || location.isInterior){
			foreach(GameObject lightHolder in LightHolder)
				lightHolder.GetComponent<Light>().intensity = 0f;
		}
		else if(gameManager.timeOfDay == 'n'){
			foreach(GameObject lightHolder in LightHolder)
				lightHolder.GetComponent<Light>().intensity = 0f;
		}
	}

	public void changePlayer(Transform newPlayer, bool inScene){
		if(!inScene)
			newPlayer = GameObject.Find (newPlayer.name).transform;
		if(newPlayer.tag == "Playable Character"){
			PlayerSwitch playerSwitch = FindObjectOfType<PlayerSwitch>();
			playerSwitch.actualSwitch(newPlayer);
		}

		else
			Debug.Log("Character trying to switch is not playable.");
	}

	public void changePlayer(Transform newPlayer, bool inScene, Transform holderTransfer){
		if(!inScene)
			newPlayer = GameObject.Find (newPlayer.name).transform;
		if(newPlayer.tag == "Playable Character"){
			PlayerSwitch playerSwitch = FindObjectOfType<PlayerSwitch>();
			playerSwitch.actualSwitch(newPlayer, holderTransfer);
		}

		else
			Debug.Log("Character trying to switch is not playable.");
	}

	public void changeVersion(Transform newPlayer, GameObject currentVersion, bool inScene, bool isNPC, Transform holderTransfer) {
		if (!inScene)
			newPlayer = GameObject.Find (newPlayer.name).transform;
		//ChangePlayerVersion changePlayerVersion = FindObjectOfType<ChangePlayerVersion> ();
		//changePlayerVersion.actualSwitch (newPlayer, currentVersion, holderTransfer);

		Destroy(currentVersion);

		float scaleX = newPlayer.localScale.x;
		float scaleY = newPlayer.localScale.y;
		float rotationZ = 360 - newPlayer.rotation.eulerAngles.z;
		newPlayer.parent = holderTransfer;
		newPlayer.localPosition = Vector3.zero;

		if(holderTransfer.localScale.x < 0)
			newPlayer.localScale = new Vector3(-scaleX, scaleY, 1f);
		else 
			newPlayer.localScale = new Vector3(scaleX, scaleY, 1f);
			
		if (newPlayer.localRotation.z > 0)
			newPlayer.localRotation = Quaternion.Euler (0, 0, -rotationZ);
		else if (newPlayer.localRotation.z < 0)
			newPlayer.localRotation = Quaternion.Euler (0, 0, rotationZ);
		if (isNPC) {
			SpriteRenderer[] New = newPlayer.GetComponentsInChildren<SpriteRenderer> (true);
			foreach (SpriteRenderer _new in New) {
				_new.sortingLayerName = "NPC";
			}
		}
	}

	void instantChangePlayer(string newPlayerName){
		PlayerSwitch playerSwitch = FindObjectOfType<PlayerSwitch>();
		Transform newPlayer = null;
		if(newPlayerName != "Don Juan"){
			foreach(Transform playableCharacter in gameManager.playableCharacters){
				if(newPlayerName == playableCharacter.name){
					newPlayer = GameObject.Find(newPlayerName).transform;
					newPlayer.position = player.transform.position;
					//(Transform) Instantiate(playableCharacter, player.transform.position, player.transform.rotation);
					newPlayer.name = newPlayerName;
					playerSwitch.instantSwitch(newPlayer);
					break;
				}
			}
		}
	}

	public void savePosition(){
		if(playerPos != null)
			playerPos.saveInBetweenData();
	}
}
