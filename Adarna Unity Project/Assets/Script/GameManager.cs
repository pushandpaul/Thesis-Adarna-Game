using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	public int latestPartIndex = 0;
	public bool watchedIntro = false;
	private ObjectiveManager objectiveManager;

	public List <SceneObjects> sceneObjects;
	public Transform sceneObjsHolder;
	public SceneObjects currentSceneObj;
	public string currentScene = "";
	public string prevScene;

	public char timeOfDay = 'd'; //d - day, n - night

	//public Sprite[] heldItem;
	public Sprite currentHeldItem; 
	public int initPlayerIdleStateHash;
	public int playerIdleState;
	public string playerIdleState_string;
	public float playerSpeed = 5f;

	public LevelManager tempLevelManager;
	public List <FollowTarget> Followers;
	public List <string> FollowerNames;

	public bool spawnIsRight;

	[System.Serializable]
	public class SavedCharData{
		public string Name;
		public string state;
		public int stateHashID;
		public Sprite heldItem;

		public SavedCharData(string Name, int stateHashID, Sprite heldItem){
			this.Name = Name;
			this.stateHashID = stateHashID;
			this.heldItem = heldItem;
		}
	}
		
	public struct BattleData{
		public BattleSetup.EnemyType enemyType;
		public BattleSetup.Stage stage;
		public int enemyBaseHP;
	}

	public List <SavedCharData> characters;

	public Transform[] playableCharacters;
	public string currentCharacterName;
	public string defaultCharacterName = "Don Juan";

	public BattleData battleData;

	public bool isPaused;
	public Camera blurredCam;

	public bool pauseRan = false;

	public UIFader mainHUD;
	public Sprite[] heldItems;
	public MySaveGame mySaveGame;
	public InitSaveGame initSaveGame;

	public List <GameObject> HUDs;
	public GameObject pauseMenu;
	public GameObject pauseButton;
	private bool pauseInControl;
	public TutorialManager tutorialManager;

	public static bool inGame;

	void Awake () {
		HUDs = new List<GameObject>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		initPlayerIdleStateHash = Animator.StringToHash(playerIdleState_string);
		//Debug.Log(Animator.StringToHash("(Don Pedro) Carry Item Idle"));
		DontDestroyOnLoad(this);
		sceneObjects = new List<SceneObjects>();
		//Followers = new List<FollowTarget>();
		FollowerNames = new List<string>();
		if(characters.Count == 0)
			characters = new List<SavedCharData>();
		foreach(FollowTarget follower in Followers){
			FollowerNames.Add(follower.name);
		}

		foreach(SavedCharData character in characters){
			if(character.state != null && character.state != ""){
				character.stateHashID = Animator.StringToHash(character.state);
			}
		}

		//resetCharData("Don Pedro");
		//SaveGameSystem.DeleteSaveGame("MySaveGame");
		LevelManager.exitInRight = !spawnIsRight;
		//LevelManager.setSpawnDirection (spawnIsRight);
	}

	void Start(){
		loadInitData();

		//loadPartData(0);
	}

	public void updateSceneList(){
		updateSceneList(currentScene, true);
	}

	private void updateSceneList(string newScene, bool setAsCurrent){
		bool found = false;

		foreach(SceneObjects sceneObject in sceneObjects){
			if(newScene == sceneObject.Name){
				found = true;
				//Debug.Log("Current scene found in the list");
				if(setAsCurrent){
					currentSceneObj = sceneObject;
				}

				break;
			}
			found = false;
		}

		if(!found){
			//Debug.Log("Current scene not found in the list.");
			GameObject container = new GameObject(newScene);
			container.transform.SetParent(this.sceneObjsHolder);
			SceneObjects tempSceneObj = container.AddComponent<SceneObjects>();
			tempSceneObj.Name = newScene;
			sceneObjects.Add(tempSceneObj);
			if(setAsCurrent)
				currentSceneObj = tempSceneObj;
			//Debug.Log("'" + currentSceneObj.Name + "' added to the scene list.");
		}

		foreach(SceneObjects sceneObject in sceneObjects){
			Debug.Log("'" + sceneObject.Name +"'");
		}
	}

	public void saveCoordinates(ObjectData[] objectData){
		bool found = false;

		for(int i = 0; i < objectData.Length; i++){
			if(objectData[i].GetComponent<FollowTarget>() != null){
				if(objectData[i].GetComponent<FollowTarget>().enabled)
					return;
			}
			searchObjectData(objectData[i], 's');
		}
	}

	public void loadCoordinates(ObjectData[] objectData){
		bool found = false;
		for(int i = 0; i < objectData.Length; i++){
			searchObjectData(objectData[i], 'l');
		}

	}

	public bool searchObjectData(ObjectData objectData, char command /*s - save; l - load; f - normal search*/){
		bool found = false;
		
		foreach(ObjectDataReference objectDataRef in currentSceneObj.sceneObjectData){
			//Debug.Log("Found object data reference" + objectDataRef.name);
			if(objectDataRef.Name.Replace("(Ref)", "") == objectData.Name){
				found = true;

				switch(command){

				case 's': 
					//Debug.Log("Save Command: Game object '" + objectData.Name + "' found.");
					objectDataRef.Name = objectData.Name;
					objectDataRef.coordinates = objectData.transform.position;
					if(objectData.persistLookDirection){
						if(objectData.transform.localScale.x > 0)
							objectDataRef.lookRight = true;
						else if(objectData.transform.localScale.x < 0)
							objectDataRef.lookRight = false;
					}
					objectDataRef.destroyed = objectData.destroyed;

					if(objectData.transform.parent != null)
						objectDataRef.parentName = objectData.transform.parent.name;
					else
						objectDataRef.parentName = "";
					
					//Debug.Log("'" + objectData.Name + "' position is saved.");
					break;
				case 'l':
					if(objectDataRef.destroyed)
						Destroy(objectData.gameObject);
					else{
						objectData.transform.position = objectDataRef.coordinates;
						if(objectData.persistLookDirection){
							Vector3 tempScale = objectData.transform.localScale;
							if(objectDataRef.lookRight)
								objectData.transform.localScale = new Vector3(Mathf.Abs(tempScale.x), tempScale.y, tempScale.z);
							else
								objectData.transform.localScale = new Vector3(-Mathf.Abs(tempScale.x), tempScale.y, tempScale.z);
						}
						//Debug.Log("'" + objectData.Name + "' position is loaded.");
						if(objectDataRef.parentName != "" && objectDataRef.parentName != null && objectDataRef.parentName != "NA"){
							if(objectData.transform.parent.name != objectDataRef.parentName)
								objectData.transform.parent = GameObject.Find(objectDataRef.parentName).transform;
							//Debug.Log("'" + objectData.Name + "' parent has found.");
						}
						/*else{
							objectData.transform.parent = null;
							Debug.Log("'" + objectData.Name + "' has no parent.");
						}*/
					}
					break;
				case 'f':
					//Debug.Log("Search Command: Game object '" + objectData.Name + "' found.");
					break;
				}
				break;
			}
		}
			
		if(!found){
			switch(command){
			case 's':
				//Debug.Log("Save Command: Game object '" + objectData.Name + "' not found.");
				GameObject container = new GameObject();
				container.transform.SetParent(currentSceneObj.transform);
				ObjectDataReference tempData = container.AddComponent<ObjectDataReference>();
				tempData.Name = "(Ref)" + objectData.Name;
				container.name = tempData.Name;
				tempData.coordinates = objectData.transform.position;
				//tempData.scale = objectData.transform.localScale;
				tempData.destroyed = objectData.destroyed;

				if(objectData.transform.parent != null)
					tempData.parentName = objectData.transform.parent.name;
				else
					tempData.parentName = "";

				currentSceneObj.sceneObjectData.Add(tempData);
				//Debug.Log("'" + tempData.Name + "' is added to game data references list.");
				break;
			case 'l':
				//Debug.Log("Load Command: Game object '" + objectData.Name + "' not found.");
				break;
			}
		}
	
		return found;
	}

	public void saveCharData(CharacterData[] characterData){
		int tempHashID = 0;
		bool found = false;
		Sprite itemHeld = new Sprite();
		foreach(CharacterData charData in characterData){
			found = false;
			//Debug.Log(charData.name);
			if(charData.allowSave){
				for(int i = 0; i < characters.Count; i++){
					if(charData.name == characters[i].Name){
						//Debug.Log("Character data for '" + charData.name + "' found in the list.");
						//Debug.Log("Saving character data for '" + charData.name + "'.");
						if(charData.saveThis){
							if(charData.anim != null)
								tempHashID = charData.stateHashID;
							else
								tempHashID = 0;
							itemHeld = charData.heldItem;
						}
								
						else{
							if(charData.anim != null)
								tempHashID = charData.anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
							else
								tempHashID = 0;
							if(charData.item != null){
								itemHeld = charData.item.getItem();
							}
							else 
								itemHeld = null;
						}
								
						characters[i] = new SavedCharData(charData.name, tempHashID, itemHeld);
						found = true;
						break;
					}
				}

				if(!found){
					//Debug.Log("No character data for '" + charData.name + "' found in the list. Saved character data will be created.");
					//Debug.Log("Saving character data for '" + charData.name + "'.");
					if(charData.saveThis){
						if(charData.anim != null)
							tempHashID = charData.stateHashID;
						else
							tempHashID = 0;
						itemHeld = charData.heldItem;
					}

					else{
						if(charData.anim != null)
							tempHashID = charData.anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
						else
							tempHashID = 0;
						if(charData.item != null){
							itemHeld = charData.item.getItem();
						}
						else 
							itemHeld = null;
					}
					characters.Add(new SavedCharData(charData.name, tempHashID, itemHeld));
				}
			}
		}
	}

	public void loadCharData(CharacterData[] characterData){
		bool found = false;

		foreach(CharacterData charData in characterData){
			//found = false;
			foreach(SavedCharData character in characters){
				if(charData.name == character.Name && charData.name != currentCharacterName){
					//Debug.Log("Character '" + charData.name + "' is found in the character data list.");
					//found = true;
					if(character.stateHashID != 0){
						//Debug.Log("Character '" + charData.name + "' animation ID '" + character.stateHashID + "' will be played.");
						charData.anim.Play(character.stateHashID);
					}
					//else
						//Debug.Log("Character '" + charData.name + "' animator component does not exist.");
					if(character.heldItem != null)
						charData.item.setItem(character.heldItem);
					break;
				}
				//else
					//Debug.Log("Character may be the player or data not saved.");
			}
		}
	}

	public void resetCharData(CharacterData characterData){
		foreach(SavedCharData character in characters){
			if(characterData.name == character.Name){
				characters.Remove(character);
				characterData.allowSave = false;
				//characterData.enabled = false;
				//Destroy(characterData);
				break;
			}
		}
	}

	public void resetCharsData(CharacterData[] charactersData){
		foreach(CharacterData charData in charactersData){
			resetCharData(charData);
		}
	}

	public void setBattleInit(BattleSetup.EnemyType enemyType, BattleSetup.Stage stage, int enemyBaseHP){
		battleData.enemyType = enemyType;
		battleData.stage = stage;
		battleData.enemyBaseHP = enemyBaseHP;
	}

	Sprite searchSpriteInList(string spriteName){
		foreach(Sprite sprite in heldItems){
			if(sprite.name == spriteName){
				return sprite;
			}		
		}
		return null;
	} 
	public void feedDataAndSave(){
		List<Save_CharData> save_charData = new List<Save_CharData>();
		List<Save_SceneObjects> save_sceneObjects = new List<Save_SceneObjects>();
		List<Save_ObjectData> save_objectsData = new List<Save_ObjectData>();

		int currentPartIndex = objectiveManager.currentPartIndex;

		string spriteName = "";

		if(currentHeldItem != null){
			spriteName = currentHeldItem.name;
		}
		else
			spriteName = "";

		Save_PlayerData save_playerData = new Save_PlayerData(currentCharacterName, playerIdleState, spriteName);
		Save_LocationSetup save_locationSetup = new Save_LocationSetup(LevelLoader.sceneToLoad, LevelManager.exitInRight, LevelManager.isDoor, LevelManager.doorIndex);

		foreach(SavedCharData character in characters){
			if(character.heldItem != null){
				spriteName = character.heldItem.name;
			}
			else
				spriteName = "";

			save_charData.Add(new Save_CharData(character.Name, character.stateHashID, spriteName));
		}

		foreach(SceneObjects sceneObject in sceneObjects){
			save_objectsData = new List<Save_ObjectData>();
			foreach(ObjectDataReference objectData in sceneObject.sceneObjectData){
				save_objectsData.Add(new Save_ObjectData(objectData.Name, objectData.coordinates.x, objectData.coordinates.y, 
					objectData.coordinates.z, objectData.parentName, objectData.destroyed));
			}
			save_sceneObjects.Add(new Save_SceneObjects(sceneObject.Name, save_objectsData));

			foreach(Save_SceneObjects save_sceneObject in save_sceneObjects){
				foreach(Save_ObjectData temp in save_sceneObject.objectsData){
					Debug.Log(temp.Name);
				}
			}
		}
			
		if(currentHeldItem != null){
			spriteName = currentHeldItem.name;
		}
		else
			spriteName = "";
		
		SaveGameSystem.SaveGame(new MySaveGame(currentPartIndex, save_playerData, save_locationSetup, save_charData, save_sceneObjects, FollowerNames), "MySaveGame_Part_" + currentPartIndex);

		if(objectiveManager.currentPartIndex > latestPartIndex){
			latestPartIndex = objectiveManager.currentPartIndex;
		}
		SaveGameSystem.SaveGame(new InitSaveGame(latestPartIndex, watchedIntro), "InitSaveGame");

	}

	public void loadPartData(int part){
		Vector3 temp = Vector3.zero;
		mySaveGame = SaveGameSystem.LoadGame("MySaveGame_Part_" + part) as MySaveGame;
		//SaveGameSystem.DeleteSaveGame("MySaveGame");

		if(mySaveGame != null){
			
			objectiveManager.currentPartIndex = mySaveGame.partIndex;
			objectiveManager.currentObjective = null;
			objectiveManager.setPartObjectives();
			objectiveManager.Init();
			currentCharacterName = mySaveGame.playerData.Name;
			playerIdleState = mySaveGame.playerData.stateHashID;
			currentHeldItem = searchSpriteInList(mySaveGame.playerData.heldItemName);
			FollowerNames = mySaveGame.followers;
			LevelManager.exitInRight = mySaveGame.locationSetup.isRight;
			LevelManager.isDoor = mySaveGame.locationSetup.isDoor;
			LevelManager.doorIndex = mySaveGame.locationSetup.doorIndex;

			foreach(Save_CharData charData in mySaveGame.charData){
				characters.Add(new SavedCharData(charData.Name, charData.stateHashID, searchSpriteInList(charData.heldSpriteName)));
			}

			foreach(Save_SceneObjects sceneObject in mySaveGame.sceneObjects){
				foreach(Save_ObjectData objectData in sceneObject.objectsData){
					temp = new Vector3(objectData.positionX, objectData.positionY, objectData.positionZ);
					tempLevelManager.setObjectReference(sceneObject.sceneName, objectData.Name, temp, objectData.isDestroyed);
				}
			}

			FindObjectOfType<LevelLoader>().launchScene(mySaveGame.locationSetup.sceneToLoad);
		}
	}

	public void loadInitData(){
		initSaveGame = SaveGameSystem.LoadGame("InitSaveGame") as InitSaveGame;
		if(initSaveGame != null){
			latestPartIndex = initSaveGame.latestPartIndex;
			watchedIntro = initSaveGame.watchedIntro;
		}
	}

	public void deleteAllSavedData(){
		SaveGameSystem.DeleteSaveGame("InitSaveGame");
		for(int i = 0; i <= latestPartIndex; i++){
			SaveGameSystem.DeleteSaveGame("MySaveGame_Part_" + i);
		}
		initTutorial();
	}

	public void initTutorial(){
		foreach(SceneObjects sceneObject in sceneObjects){
			foreach(ObjectDataReference objectDataRef in sceneObject.sceneObjectData){
				Destroy(objectDataRef.gameObject);
			}
			sceneObject.sceneObjectData.Clear();
			Destroy(sceneObject.gameObject);
		}

		sceneObjects.Clear();
		characters.Clear();

		latestPartIndex = 0;
		currentCharacterName = "Don Pedro";
		LevelManager.isDoor = false;
		LevelManager.doorIndex = 0;
		LevelManager.exitInRight = true;
		objectiveManager.currentPartIndex = 0;
		objectiveManager.currentObjectiveIndex = 0;
		objectiveManager.setPartObjectives();
		objectiveManager.Init();
		FindObjectOfType<LevelLoader>().launchScene("Kwarto ni Haring Fernando");
	}

	public void pause(bool isPaused){
		this.isPaused = isPaused;
		PlayerController player = FindObjectOfType<PlayerController> ();

		if(isPaused){
			hideHUDs(false);
			Time.timeScale = 0f;
		}
		else{
			hideHUDs (true);
			Time.timeScale = 1f;
		}
			
		if(player != null){
			if(isPaused){
				if(!player.canMove && !player.canJump){
					pauseInControl = false;
				}
				else{
					player.canMove = false;
					player.canJump = false;
					pauseInControl = true;
				}
			}
			else{
				if(pauseInControl){
					player.canMove = true;
					player.canJump = true;
				}
			}
		}
	}

	public void setHUD(GameObject HUD, bool enable){
		HUD.SetActive(enable);
	}


	public void setHUDs(bool enable){
		if(HUDs == null || HUDs.Count == 0){
			HUDs = GameObject.FindGameObjectsWithTag ("HUD").ToList();
		}
		foreach(GameObject HUD in HUDs){
			setHUD (HUD, enable);
		}
	}

	public void hideHUD(CanvasGroup HUD, bool show){
		if (show)
			HUD.alpha = 1f;
		else
			HUD.alpha = 0f;
		HUD.interactable = show;
	}

	public void hideHUDs(bool show){
		if(HUDs == null || HUDs.Count == 0){
			HUDs = GameObject.FindGameObjectsWithTag ("HUD").ToList();
		}
		foreach(GameObject HUD in HUDs){
			hideHUD (HUD.GetComponent<CanvasGroup> (), show);
		}
	}

	public void setPauseMenu(bool enable){
		pauseMenu.GetComponent<PauseMenu> ().enabled = enable;
	}

	public void close(){
		Application.Quit();
	}
}
