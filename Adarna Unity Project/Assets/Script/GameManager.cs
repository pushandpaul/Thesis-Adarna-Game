using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <SceneObjects> sceneObjects;
	public Transform sceneObjsHolder;
	public SceneObjects currentSceneObj;
	public string currentScene = "";

	public char timeOfDay = 'd'; //d - day, n - night

	//public Sprite[] heldItem;
	public Sprite currentHeldItem; 
	public string playerIdleState;
	public float playerSpeed = 5f;

	private LevelManager levelManager;
	public List <FollowTarget> Followers;
	public List <string> FollowerNames;

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

	public List <SavedCharData> characters;

	public Transform[] playableCharacters;
	public string currentCharacterName;
	public string defaultCharacterName = "Don Juan";

	public BattleSetup.EnemyType battleEnemyType;
	public BattleSetup.Stage battleStage;

	void Awake () {
		//playerIdleState = "Idle";
		Debug.Log(Animator.StringToHash("(Don Pedro) Carry Item Idle"));
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
	}
		
	public void updateSceneList(){
		bool found = false;

		foreach(SceneObjects sceneObject in sceneObjects){
			if(currentScene == sceneObject.Name){
				found = true;
				Debug.Log("Current scene found in the list");
				currentSceneObj = sceneObject;
				break;
			}
			found = false;
		}

		if(!found){
			Debug.Log("Current scene not found in the list.");
			GameObject container = new GameObject(currentScene);
			container.transform.SetParent(this.sceneObjsHolder);
			currentSceneObj = container.AddComponent<SceneObjects>();
			currentSceneObj.Name = currentScene;
			sceneObjects.Add(currentSceneObj);
			Debug.Log("'" + currentSceneObj.Name + "' added to the scene list.");
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
			Debug.Log("Found object data reference" + objectDataRef.name);
			if(objectDataRef.Name.Replace("(Ref)", "") == objectData.Name){
				found = true;

				switch(command){

				case 's': 
					Debug.Log("Save Command: Game object '" + objectData.Name + "' found.");
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
					
					Debug.Log("'" + objectData.Name + "' position is saved.");
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
						Debug.Log("'" + objectData.Name + "' position is loaded.");
						if(objectDataRef.parentName != "" && objectDataRef.parentName != null && objectDataRef.parentName != "NA"){
							if(objectData.transform.parent.name != objectDataRef.parentName)
								objectData.transform.parent = GameObject.Find(objectDataRef.parentName).transform;
							Debug.Log("'" + objectData.Name + "' parent has found.");
						}
						/*else{
							objectData.transform.parent = null;
							Debug.Log("'" + objectData.Name + "' has no parent.");
						}*/
					}
					break;
				case 'f':
					Debug.Log("Search Command: Game object '" + objectData.Name + "' found.");
					break;
				}
				break;
			}
		}
			
		if(!found){
			switch(command){
			case 's':
				Debug.Log("Save Command: Game object '" + objectData.Name + "' not found.");
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
				Debug.Log("'" + tempData.Name + "' is added to game data references list.");
				break;
			case 'l':
				Debug.Log("Load Command: Game object '" + objectData.Name + "' not found.");
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
			Debug.Log(charData.name);
			if(charData.allowSave){
				for(int i = 0; i < characters.Count; i++){
					if(charData.name == characters[i].Name){
						Debug.Log("Character data for '" + charData.name + "' found in the list.");
						Debug.Log("Saving character data for '" + charData.name + "'.");
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
							itemHeld = charData.item.getItem();
						}
								
						characters[i] = new SavedCharData(charData.name, tempHashID, itemHeld);
						found = true;
						break;
					}
				}

				if(!found){
					Debug.Log("No character data for '" + charData.name + "' found in the list. Saved character data will be created.");
					Debug.Log("Saving character data for '" + charData.name + "'.");
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
						itemHeld = charData.item.getItem();
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
					Debug.Log("Character '" + charData.name + "' is found in the character data list.");
					//found = true;
					if(character.stateHashID != 0){
						Debug.Log("Character '" + charData.name + "' animation ID '" + character.stateHashID + "' will be played.");
						charData.anim.Play(character.stateHashID);
					}
					else
						Debug.Log("Character '" + charData.name + "' animator component does not exist.");
					if(character.heldItem != null)
						charData.item.setItem(character.heldItem);
					break;
				}
				else
					Debug.Log("Character may be the player or data not saved.");
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

	/*public void findFollowers(FollowTarget[] follower){
		Followers.Clear();
		for(int i = 0; i < follower.Length; i++){
			DontDestroyOnLoad(follower[i].gameObject);
			Followers.Add(follower[i].gameObject);
			}
	}

	public void removeFollowers(){
		foreach(FollowTarget myFollower in Followers){
			Destroy(myFollower);
		}
	}

	public bool removeFollower(string Name){
		foreach(FollowTarget myFollower in Followers){
			if(myFollower.name == Name){
				myFollower.GetComponent<FollowTarget>().enabled = false;
				Followers.Remove(myFollower);
				Destroy(myFollower);
				return true;
				break;
			}
		}
		return false;
	}*/
}
