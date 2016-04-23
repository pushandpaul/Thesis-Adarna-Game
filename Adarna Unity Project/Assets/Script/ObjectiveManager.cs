using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {
	private static ObjectiveManager instance = null;

	public GameObject[] objectiveParts;
	public int currentPartIndex;
	private GameObject currentPart;

	private Objective[] Objectives;
	public Objective currentObjective;
	public int currentObjectiveIndex;
	private TextBoxManager textBox;

	public UIFader objectivePanelFader;
	public int fadeDelay = 10;
	public float panelFaderSpeed = 0.6f;

	//public List<GameObject> destroyList;

	void Awake(){
		if(FindObjectsOfType<ObjectiveManager>().Length > 1){
			Destroy(this.gameObject);
		}
		else if(FindObjectsOfType<ObjectiveManager>().Length == 1){
			DontDestroyOnLoad(this.gameObject);
		}

		/*if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
			
		else if(instance != this)
			Destroy(gameObject);
		*/
	}

	void Start () {
		textBox = this.GetComponent<TextBoxManager>();
		setPartObjectives();
	
		if(currentObjective != null){
			if(currentObjective.Description != "")
				displayCurrentObjective();

			this.printCurrentObjective();
		}

	}

	public void printCurrentObjective(){
		Debug.Log("This is the current objective " + currentObjective.Name);
	}

	public void displayCurrentObjective(){
		textBox.setText(currentObjective.Description);
	}

	public void setPartObjectives(){
		currentPart = objectiveParts[currentPartIndex];
		Objectives = currentPart.GetComponentsInChildren<Objective>();
		bool found = false;

		if(Objectives.Length > 0){
			Debug.Log("There are objectives in Part " + currentPartIndex + ".");
			for(int i = 0; i < Objectives.Length; i++){
				Objectives[i].objectiveIndex = i;
				if(currentObjective == Objectives[i]){
					currentObjectiveIndex = currentObjective.objectiveIndex;
					Debug.Log("Current objective found in the current part.");
					found = true;
				}
			}
			if(!found){
				Debug.Log("Current objective not found in the current part.");
				currentObjective = Objectives[0];
				currentObjective.objectiveIndex = 0;
				currentObjectiveIndex = currentObjective.objectiveIndex;
			}
		}
		else{
			currentObjective = null;
			Debug.Log("There are no objectives in Part " + currentPartIndex + ".");
		}
	}

	public void nextPart(){
		currentPartIndex++;
		setPartObjectives();

		if(currentObjective != null){
			if(currentObjective.Description != "")
				displayCurrentObjective();

			this.printCurrentObjective();
		}
	}
}
