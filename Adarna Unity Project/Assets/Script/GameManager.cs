﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <SceneObjects> sceneObjects;
	public SceneObjects currentSceneObj;
	public string currentScene = "";
	public char timeOfDay = 'd'; //d - day, n - night

	//for future use
	public Sprite[] heldItem;
	public Sprite currentHeldItem;
	public string playerIdleState;

	private LevelManager levelManager;
	public List <GameObject> Followers;

	void Awake () {
		//playerIdleState = "Idle";
		DontDestroyOnLoad(this);
		sceneObjects = new List<SceneObjects>();
		Followers = new List<GameObject>();
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
			searchData(objectData[i], 's');
		}
	}

	public void loadCoordinates(ObjectData[] objectData){
		bool found = false;
		for(int i = 0; i < objectData.Length; i++){
			searchData(objectData[i], 'l');
		}

	}

	public bool searchData(ObjectData objectData, char command /*s - save; l - load; f - normal search*/){
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

	public void findFollowers(FollowTarget[] follower){
		Followers.Clear();
		for(int i = 0; i < follower.Length; i++){
			DontDestroyOnLoad(follower[i].gameObject);
			Followers.Add(follower[i].gameObject);
			}
	}

	public void removeFollowers(){
		foreach(GameObject myFollower in Followers){
			Destroy(myFollower);
		}
	}
}
