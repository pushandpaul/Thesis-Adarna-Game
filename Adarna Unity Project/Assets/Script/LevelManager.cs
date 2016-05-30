using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	[System.Serializable]
	public class StructObjectRef{
		public Vector3 coordinates;
		public Vector3 scale;
		public string Name;
		public bool destroyed;
		public string parentName;
	}

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
		MatchTransform [] ToMatch = FindObjectsOfType<MatchTransform>();
		bool allowMatch = false;

		//Level Initialization
		if(objectData.Length > 0){
			gameManager.loadCoordinates(objectData);
		}

		changeTimeOfDay(gameManager.timeOfDay.ToString());

		//Player Initialization
		if(!playerPos.Load()){
			if(isDoor && door.Length > 0){
				player.transform.position = new Vector3(door[doorIndex].transform.position.x, location.playerSpawnRightY, location.playerSpawnZ);
				camera.transform.position = new Vector3(door[doorIndex].transform.position.x, location.cameraSpawnRightY, location.cameraSpawnZ);
			}
			else if(!isDoor){
				if(exitInRight){
					player.transform.position = new Vector3(location.playerSpawnLeftX, location.playerSpawnLeftY, location.playerSpawnZ);
					camera.transform.position = new Vector3(location.cameraSpawnLeftX, location.cameraSpawnLeftY, location.cameraSpawnZ);
				}
				else{
					player.transform.position = new Vector3(location.playerSpawnRightX, location.playerSpawnRightY, location.playerSpawnZ);
					player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, 1f);
					camera.transform.position = new Vector3(location.cameraSpawnRightX, location.cameraSpawnRightY, location.cameraSpawnZ);
					camera.flipped = true;
				}
			}
		}

		//Change player avatar
		instantChangePlayer(gameManager.currentCharacterName);

		//Initialization of character states
		gameManager.loadCharData(FindObjectsOfType<CharacterData>());

		//Follower Initialization
		if(followerManager != null)
			followerManager.setActiveFollowers();

		//Match transforms (scales for now)
		if(ToMatch != null && ToMatch.Length > 0){
			foreach(MatchTransform toMatch in ToMatch){
				if(toMatch.GetComponent<FollowTarget>() != null){
					if(!toMatch.GetComponent<FollowTarget>().enabled)
						allowMatch = true;
					else
						allowMatch = false;
				}
				else
					allowMatch = true;
				if(allowMatch){
					toMatch.scale();
				}
			}
		}
	}

	public void onLevelExit(){
		
		Debug.Log("Exiting level.");
		FollowerManager followerManager = FindObjectOfType<FollowerManager>();
		CharacterData[] charactersData = FindObjectsOfType<CharacterData>();

		if(FindObjectsOfType<ObjectData>().Length > 0)
			gameManager.saveCoordinates(FindObjectsOfType<ObjectData>());
		followerManager.updateFollowerList();

		gameManager.prevScene = gameManager.currentScene;
		gameManager.saveCharData(charactersData);

	}

	public void unclonedInstace(GameObject toInstantiate, Vector3 position){
		GameObject spawedObject = (GameObject)Instantiate(toInstantiate, position, Quaternion.Euler(0,0,0));
		spawedObject.name = toInstantiate.name;
	}

	public void changeTimeOfDay(string timeOfDay){
		LightController globalLight;
		GameObject globaLightHolder;
		GameObject[] characterLight = GameObject.FindGameObjectsWithTag("Character Light");
		GameObject[] nightSprites = GameObject.FindGameObjectsWithTag("Night Sprite");
		GameObject[] nightLights = GameObject.FindGameObjectsWithTag("Night Light");
		char[] myTimeOfDay = timeOfDay.ToCharArray();

		globaLightHolder = GameObject.FindWithTag("Global Light");

		if(globaLightHolder != null){
			globalLight = globaLightHolder.GetComponent<LightController>();
			if(myTimeOfDay[0] == 'd'){
				globalLight.setLightIntensity(1.8f);
				Debug.Log("Global Light Intensity adjusted to day time");
				foreach(GameObject lightHolder in characterLight)
					lightHolder.GetComponent<Light>().intensity = 0f;
				foreach(GameObject nightSprite in nightSprites){
					nightSprite.GetComponent<SpriteRenderer>().enabled = false;
				}
				foreach(GameObject nightLight in nightLights){
					nightLight.GetComponent<Light>().enabled = false;
				}
			}
			else if(myTimeOfDay[0] == 'n'){
				globalLight.setLightIntensity(0f);
				Debug.Log("Global Light Intensity adjusted to night time");
				foreach(GameObject lightHolder in characterLight){
					if(location.isInterior)
						lightHolder.GetComponent<Light>().intensity = 0f;
					else
						lightHolder.GetComponent<Light>().intensity = 1.8f;
				}
				foreach(GameObject nightSprite in nightSprites){
					nightSprite.GetComponent<SpriteRenderer>().enabled = true;
				}
				foreach(GameObject nightLight in nightLights){
					nightLight.GetComponent<Light>().enabled = true;
				}
			}
		}

		gameManager.timeOfDay = myTimeOfDay[0];
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

	public void changePlayerLater(string newPlayer){
		gameManager.currentCharacterName = newPlayer;
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
		if(newPlayerName != gameManager.defaultCharacterName){
			foreach(Transform playableCharacter in gameManager.playableCharacters){
				if(newPlayerName == playableCharacter.name){
					newPlayer = GameObject.Find(newPlayerName).transform;
					if(playerSwitch.routine == 'a')
						newPlayer.position = player.transform.position;
					else if(playerSwitch.routine == 'b'){
						player.transform.position = newPlayer.position;
						camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
					}
					//(Transform) Instantiate(playableCharacter, player.transform.position, player.transform.rotation);
					newPlayer.name = newPlayerName;
					playerSwitch.instantSwitch(newPlayer);
					break;
				}
			}
		}
	}

	public void setSpawnDirection(bool isRight) {
		if (isRight) {
			exitInRight = false;
		} else
			exitInRight = true;	
	}

	public void setSwitchRoutine(string routine){ 
		/*
		 * routine 'a' - sets the position of the new player to the player holder
		 * routine 'b' - sets the position of the player holder to the new player
		*/
		char[] myRoutine = routine.ToCharArray();
		PlayerSwitch playerSwitch = FindObjectOfType<PlayerSwitch>();
		playerSwitch.routine = myRoutine[0];
	}

	public void savePlayerPosition(){
		playerPos.Save();
	}

	public void clearHeldItem(){
		gameManager.currentHeldItem = null;
	}

	public void _setObjectReference(string objectName, Vector3 position, bool destroyed){
		setObjectReference(gameManager.currentScene, objectName, position, destroyed);
	}

	public void setObjectReference(string sceneName, string objectName, Vector3 position, bool destroyed){
		ObjectDataReference[] objectDataRefs;
		SceneObjects tempSceneObject;
		GameObject objectRefContainer;
		ObjectDataReference objectRef;
		bool sceneFound = false;
		bool objectFound = false;

		foreach(SceneObjects sceneObject in gameManager.sceneObjects){
			if (sceneName == sceneObject.name) {
				Debug.Log ("Scene found in the list");
				objectDataRefs = sceneObject.GetComponentsInChildren<ObjectDataReference> ();
				foreach (ObjectDataReference objectDataRef in objectDataRefs) {
					if (objectName == objectDataRef.Name.Replace ("(Ref)", "")) {
						objectDataRef.Init (position, destroyed);
						objectFound = true;
						break;
					} else
						objectFound = false;
				}
				if(!objectFound){
					objectRefContainer = new GameObject ("(Ref)" + objectName);
					objectRef = objectRefContainer.AddComponent<ObjectDataReference> ();
					objectRef.transform.SetParent (sceneObject.transform);
					objectRef.Init (position,destroyed);
					sceneObject.sceneObjectData.Add (objectRef);
				}
				sceneFound = true;
				break;
			}
		}
		if(!sceneFound){
			Debug.Log("Scene not found in the list.");
			GameObject container = new GameObject(sceneName);
			container.transform.SetParent(gameManager.sceneObjsHolder);
			tempSceneObject = container.AddComponent<SceneObjects>();
			tempSceneObject.Name = sceneName;
			gameManager.sceneObjects.Add(tempSceneObject);
			Debug.Log("'" + tempSceneObject.Name + "' added to the scene list.");

			objectRefContainer = new GameObject ("(Ref)" + objectName);
			objectRef = objectRefContainer.AddComponent<ObjectDataReference> ();
			objectRef.transform.SetParent (tempSceneObject.transform);
			objectRef.Init (position,destroyed);
			tempSceneObject.sceneObjectData.Add (objectRef);
		}
	}

	public void _removeObjectDataReference(string objectName){
		removeObjectReference(gameManager.currentScene, objectName);
	}

	public void removeObjectReference(string sceneName, string objectName){
		List <ObjectDataReference> objectDataRefs;
		foreach(SceneObjects sceneObject in gameManager.sceneObjects){
			if(sceneName == sceneObject.name){
				objectDataRefs = sceneObject.GetComponentsInChildren<ObjectDataReference>().ToList();
				foreach(ObjectDataReference objectDataRef in objectDataRefs){
					if(objectName == objectDataRef.Name.Replace("(Ref)", "")){
						sceneObject.sceneObjectData.Remove(objectDataRef);
						Destroy(objectDataRef.gameObject);
						break;
					}
				}
				break;
			}
		}
	}

	public void setCharData(CharacterData characterData, string state){
		characterData.saveThis = true;
		characterData.Init(state);
	}

	public void setCharData(CharacterData characterData, string state, Sprite itemHeld){
		characterData.saveThis = true;
		characterData.Init(state, itemHeld);
	}

	public void resetCharData(CharacterData characterData){
		gameManager.resetCharData(characterData);
	}

	public void resetCharsData(CharacterData[] charactersData){
		gameManager.resetCharsData(charactersData);
	}
}
