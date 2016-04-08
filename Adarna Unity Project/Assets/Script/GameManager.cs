using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <SceneObjects> sceneObjects;
	public SceneObjects currentSceneObj;
	public string currentScene = "";

	//for future use
	public Sprite[] heldItem;
	public Sprite currentHeldItem;
	public string playerIdleState;

	private LevelManager levelManager;

	void Awake () {
		//playerIdleState = "Idle";
		DontDestroyOnLoad(this);
		sceneObjects = new List<SceneObjects>();
	}


	public void updateSceneList(){
		bool found = false;
		//if()

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
			container.transform.SetParent(this.transform);
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
			/*foreach(ObjectDataReference objectDataRef in currentSceneObj.sceneObjectData){
				if(objectDataRef.Name == objectData[i].Name){
					found = true;
					Debug.Log("Game object '" + objectData[i].Name + "' found.");
					objectDataRef.Name = objectData[i].Name;
					objectDataRef.coordinates = objectData[i].transform.position;
					break;
				}
				found = false;
			}
			if(!found){
				Debug.Log("Game object '" + objectData[i].Name + "' not found.");
				GameObject container = new GameObject(objectData[i].Name);
				container.transform.SetParent(currentSceneObj.transform);
				ObjectDataReference tempData = container.AddComponent<ObjectDataReference>();
				tempData.Name = objectData[i].Name;
				tempData.coordinates = objectData[i].transform.position;
				currentSceneObj.sceneObjectData.Add(tempData);
				Debug.Log("'" + tempData.Name + "' is added to game data references list.");
			}*/
			searchData(objectData[i], 's');
		}
	}

	public void loadCoordinates(ObjectData[] objectData){
		bool found = false;
		for(int i = 0; i < objectData.Length; i++){
			/*foreach(ObjectDataReference objectDataRef in currentSceneObj.sceneObjectData){
				if(objectDataRef.Name == objectData[i].Name){
					found = true;
					objectData[i].transform.position = objectDataRef.coordinates;
					Debug.Log("'" + objectData[i].Name + "' position is loaded.");
					break;
				}
				found = false;
			}
			if(found = false)
				Debug.Log("'" + objectData[i].Name+ "' is not found.");*/
			searchData(objectData[i], 'l');
		}

	}

	public bool searchData(ObjectData objectData, char command /*s - save; l - load; d- destroy; f - normal search*/){
		bool found = false;

		foreach(ObjectDataReference objectDataRef in currentSceneObj.sceneObjectData){
			if(objectDataRef.Name == objectData.Name){
				found = true;

				switch(command){

				case 's': 
					Debug.Log("Save Command: Game object '" + objectData.Name + "' found.");
					objectDataRef.Name = objectData.Name;
					objectDataRef.coordinates = objectData.transform.position;
					objectDataRef.destroyed = objectData.destroyed;
					Debug.Log("'" + objectData.Name + "' position is saved.");
					break;
				case 'l':
					if(objectDataRef.destroyed)
						Destroy(objectData.gameObject);
					else{
						objectData.transform.position = objectDataRef.coordinates;
						Debug.Log("'" + objectData.Name + "' position is loaded.");
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
				GameObject container = new GameObject(objectData.Name);
				container.transform.SetParent(currentSceneObj.transform);
				ObjectDataReference tempData = container.AddComponent<ObjectDataReference>();
				tempData.Name = objectData.Name;
				tempData.coordinates = objectData.transform.position;
				tempData.destroyed = objectData.destroyed;
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

}
