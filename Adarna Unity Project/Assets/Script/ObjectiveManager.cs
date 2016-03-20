using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {
	private static ObjectiveManager instance = null;

	public Objective currentObjective;
	public int currentObjectiveIndex;
	private Objective[] allObjectives;

	//public List<GameObject> destroyList;

	void Awake(){
		/*if(FindObjectsOfType<ObjectiveManager>().Length > 1){
			Destroy(this.gameObject);
		}
		else if(FindObjectsOfType<ObjectiveManager>().Length == 1){
			DontDestroyOnLoad(this.gameObject);
		}*/

		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy(gameObject);
		
		DontDestroyOnLoad(gameObject);
			
	}

	void Start () {
		//destroyList = new List<GameObject>();
		allObjectives = this.GetComponentsInChildren<Objective>();


		for(int i = 0; i < allObjectives.Length; i++)
			allObjectives[i].objectiveIndex = i;

		currentObjectiveIndex = currentObjective.objectiveIndex;
		this.printCurrentObjective();
	}

	public void printCurrentObjective(){
		Debug.Log("This is the current objective " + currentObjective.Name);
	}

	public void destroyGameObjects(){
		
	}
}
