﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorAndExitController : MonoBehaviour {
	[System.Serializable]
	public class ExitsInScene{
		public string Name;
		public List <ExitData> exits;
	}

	[System.Serializable]
	public class ExitData{
		public string Name;
		public bool isOpen = true;
		public bool isDoor;

		public ExitData(string Name, bool isDoor){
			this.Name = Name;
			this.isDoor = isDoor;
		}

		public ExitData(string Name, bool isOpen, bool isDoor){
			this.Name = Name;
			this.isOpen = isOpen;
			this.isDoor = isDoor;
		}
	}

	[System.Serializable]
	public class ActualExit{
		public GameObject exit;
		public bool isDoor;

		public ActualExit(GameObject exit, bool isDoor){
			this.exit = exit;
			this.isDoor = isDoor;
		}
	}

	public List <ExitsInScene> exitsInScenes;
	public ExitsInScene currentExitsInScene;
	private LevelManager levelManager;

	private ExitManager[] currentExits;
	private DoorHandler[] currentDoors;
	private List<ActualExit> actualExits;

	void Start () {
		Init();
	}

	public void SetExitAccess(string exitName, bool isOpen, bool isDoor){
		ExitData foundExitData;
		ExitData tempExitData;
		foundExitData = FindReturnExitData(exitName, currentExitsInScene);
		if(foundExitData == null){
			tempExitData = new ExitData(exitName, isOpen, isDoor);
			currentExitsInScene.exits.Add(tempExitData);
		}
		else{
			tempExitData = foundExitData;
			tempExitData.isOpen = isOpen;
		}

		FindAndSetExit(tempExitData);
	}

	public void SetExitAccess(string sceneName, string exitName, bool isOpen, bool isDoor){
		ExitData foundExitData;
		ExitData tempExitData;
		ExitsInScene foundScene = FindScene(sceneName);
		foundExitData =	FindReturnExitData(exitName, foundScene);

		if(foundExitData == null){
			tempExitData = new ExitData(exitName, isOpen, isDoor);
			foundScene.exits.Add(tempExitData);
		}
		else{
			foundExitData.isOpen = isOpen;
		}
	}

	ExitsInScene FindScene(string sceneName){
		bool found = false;
		ExitsInScene foundExitsInScene = new ExitsInScene();

		foreach(ExitsInScene exitInScene in exitsInScenes){
			if(exitInScene.Name == sceneName){
				found = true;
				foundExitsInScene = exitInScene;
				break;
			}
		}

		if(!found){
			foundExitsInScene.Name = sceneName;
			foundExitsInScene.exits = new List<ExitData>();
			exitsInScenes.Add(foundExitsInScene);
		}

		return foundExitsInScene;
	}

	void SetupExits(){
		
		List <ExitData> tempExits = new List<ExitData>();
		ExitData tempExitData;

		foreach(ExitManager currentExit in currentExits){
			tempExits.Add(new ExitData(currentExit.name, false));
		}

		foreach(DoorHandler currentDoor in currentDoors){
			tempExits.Add(new ExitData(currentDoor.name, true));
		}

		foreach(ExitData _tempExit in tempExits){
			tempExitData = FindReturnExitData(_tempExit.Name, currentExitsInScene);
			if(tempExitData != null){
				FindAndSetExit(tempExitData);
			}
			else{
				currentExitsInScene.exits.Add(_tempExit);
			}
		}
	}

	void FindAndSetExit(ExitData toFind){
		foreach(ActualExit actualExit in actualExits){
			if(toFind.Name == actualExit.exit.name){
				if(!actualExit.isDoor){
					actualExit.exit.GetComponent<ExitManager>().isOpen = toFind.isOpen;
					Debug.Log("Door is open? " + actualExit.exit.GetComponent<ExitManager>().isOpen);
				}
				else{
					actualExit.exit.GetComponent<DoorHandler>().isOpen = toFind.isOpen;
					Debug.Log("Exit is open? " + actualExit.exit.GetComponent<DoorHandler>().isOpen);
				}
				break;
			}
		}
	}
		
	ExitData FindReturnExitData(string toFind, ExitsInScene toWhichScene){
		foreach(ExitData exit in toWhichScene.exits){
			if(toFind == exit.Name){
				return exit;
				break;
			}
		}
		return null;
	}

	public void Init(){
		levelManager = FindObjectOfType<LevelManager>();
		currentExits = FindObjectsOfType<ExitManager>();
		currentDoors = FindObjectsOfType <DoorHandler>();
		actualExits = new List<ActualExit>();

		foreach(ExitManager currentExit in currentExits){
			actualExits.Add(new ActualExit(currentExit.gameObject, false));
		}

		foreach(DoorHandler currentDoor in currentDoors){
			actualExits.Add(new ActualExit(currentDoor.gameObject, true));
		}

		if(levelManager != null){
			currentExitsInScene = FindScene(levelManager.sceneName);
			SetupExits();
		}
	}
}
