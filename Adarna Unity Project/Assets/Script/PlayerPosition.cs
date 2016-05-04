using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPosition : MonoBehaviour {

	private CameraController camera;
	private PlayerController player;

	[System.Serializable]
	class PositionRef{
		public string sceneName;
		public Vector3 playerPos;
		public Vector3 cameraPos;
	}

	private List <PositionRef> positionRefs;

	void Start(){
		positionRefs = new List<PositionRef>();
	}

	public void Save(){
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		string currentScene = FindObjectOfType<LevelManager>().sceneName;
		PositionRef tempRef = new PositionRef();
		bool found = false;
		if(positionRefs.Count > 0){
			foreach(PositionRef positionRef in positionRefs){
				if(positionRef.sceneName == currentScene){
					positionRef.playerPos = player.transform.position;
					positionRef.cameraPos = camera.transform.position;
					Debug.Log("Scene exists in the list. Save position for scene '" + currentScene + "'.");
					found = true;
					break;
				}
			}
		}

		if(!found){
			tempRef.sceneName = currentScene;
			tempRef.playerPos = player.transform.position;
			tempRef.cameraPos = camera.transform.position;
			Debug.Log("Scene does not exist in the list. Save position for scene '" + currentScene + "' will be created.");
			positionRefs.Add(tempRef);
		}
	}

	public bool Load(){
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		string currentScene = FindObjectOfType<LevelManager>().sceneName;
		bool found = false;

		if(positionRefs.Count > 0){
			foreach(PositionRef positionRef in positionRefs){
				if(positionRef.sceneName == currentScene){
					player.transform.position = positionRef.playerPos;
					camera.transform.position = positionRef.cameraPos;
					positionRefs.Remove(positionRef);
					found = true;
					Debug.Log("Saved position for scene '" + currentScene + "' is loaded");
					break;
				}
			}
		}
		return found;
	}
}
