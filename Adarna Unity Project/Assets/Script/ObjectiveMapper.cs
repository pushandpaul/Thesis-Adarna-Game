﻿using UnityEngine;
using System.Collections;

public class ObjectiveMapper : MonoBehaviour {

	private ObjectiveManager objectiveManager;
	//public TextBoxManager textBox;
	public int [] objectiveIndex;


	public bool isNPC;
	public bool isObject;

	private NPCInteraction npc;
	private ObjectInteraction objectiveHolder;

	//private bool found;
	// Use this for initialization
	void Start () {
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		//textBox = objectiveManager.GetComponent<TextBoxManager>();
		if(isNPC)
			npc = this.GetComponent<NPCInteraction>();
		else if(isObject)
			objectiveHolder = this.GetComponent<ObjectInteraction>();
	}

	public void checkIfCurrent_npc(){
		for(int i = 0; i < objectiveIndex.Length; i++){
			Debug.Log("Entered Objective Checker Loop");
			if(objectiveManager.currentObjectiveIndex == objectiveIndex[i]){
				npc.message = objectiveManager.currentObjective.Name;
				Debug.Log("Sendmessage " + npc.message);
				objectiveManager.currentObjective.onReach();
				break;
				//objectiveHolder.isAchieved = false;
			}
			else{
				npc.message = npc.name;
				//npc.pressToInteract = true;
				//found = false;
			}
				
		}
	}
		
	public bool checkIfCurrent() {
		for (int i = 0; i < objectiveIndex.Length; i++) {
			if (objectiveManager.currentObjectiveIndex == objectiveIndex [i]) {
				return true;
				break;
			} 	
		}
		return false;
	}

	public void checkIfCurrent_object(){
		for(int i = 0; i < objectiveIndex.Length; i++){
			Debug.Log("Entered Objective Checker Loop");
			if(objectiveManager.currentObjectiveIndex == objectiveIndex[i]){
				objectiveHolder.message = objectiveManager.currentObjective.Name;
				Debug.Log("Sendmessage " + objectiveHolder.message);
				objectiveManager.currentObjective.onReach();
				//found = true;
				//objectiveHolder.isAchieved = true;
				break;
			}
			else{
				objectiveHolder.message = "";
			}

		}
		
	}

	public void checkIfCurrent_misc(){
		Debug.Log("Triggered misc objective");
	}
	// Update is called once per frame
}
