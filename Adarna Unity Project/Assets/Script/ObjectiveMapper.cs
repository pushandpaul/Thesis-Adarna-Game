using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class ObjectiveMapper : MonoBehaviour {

	[System.Serializable]
	public class HeldObjective{
		public int part;
		public int[] objectives;
	}

	public HeldObjective[] partObjective;
	private ObjectiveManager objectiveManager;
	//public int [] objectiveIndex;
	//public int [] objectivePartIndex;

	public bool isNPC;
	public bool isObject;

	private NPCInteraction npc;
	private ObjectInteraction objectiveHolder;

	void Awake(){
		objectiveManager = FindObjectOfType<ObjectiveManager>();
	}
	void Start () {
		
		if(isNPC)
			npc = this.GetComponent<NPCInteraction>();
		else if(isObject)
			objectiveHolder = this.GetComponent<ObjectInteraction>();
	}

	public bool checkIfCurrent() {
		bool found = false;
		for(int i = 0; i < partObjective.Length; i++){
			if(partObjective[i].part == objectiveManager.currentPartIndex){
				foreach(int objectiveIndex in partObjective[i].objectives){
					if(objectiveIndex == objectiveManager.currentObjectiveIndex){
						found = true;
						return true;
						break;
					}
				}
			}
			if(found)
				break;
		}
		return false;
	}

	public void checkIfCurrent_npc(){
		bool found = false;
		for(int i = 0; i < partObjective.Length; i++){
			if(partObjective[i].part == objectiveManager.currentPartIndex){
				foreach(int objectiveIndex in partObjective[i].objectives){
					if(objectiveIndex == objectiveManager.currentObjectiveIndex){
						npc.message = objectiveManager.currentObjective.Name;
						Debug.Log("Sendmessage " + npc.message);
						objectiveManager.currentObjective.onReach();
						found = true;
						break;
					}
					else{
						found = false;
					}
				}
			}
			if(found)
				break;
		}
		if(!found)
			npc.message = npc.name;
			
	}

	public void checkIfCurrent_object(){
		bool found = false;
		for(int i = 0; i < partObjective.Length; i++){
			if(partObjective[i].part == objectiveManager.currentPartIndex){
				foreach(int objectiveIndex in partObjective[i].objectives){
					if(objectiveIndex == objectiveManager.currentObjectiveIndex){
						objectiveHolder.message = objectiveManager.currentObjective.Name;
						Debug.Log("Sendmessage " + objectiveHolder.message);
						objectiveManager.currentObjective.onReach();
						found = true;
						break;
					}
					else{
						found = false;
					}
				}
			}
			if(found)
				break;	
		}
		if(!found)
			objectiveHolder.message = "";
	}

	public void checkIfCurrent_misc(){
		Debug.Log("Triggered misc objective");
		UIFader uiFader = objectiveManager.objectivePanelFader;

		if(checkIfCurrent()){
			Debug.Log("This is the current objective");
			objectiveManager.currentObjective.onReach();

			if(objectiveManager.currentObjective.OnReach.Contains(Objective.ActionOnReach.DisplayToTextBox))
				uiFader.FadeIn(objectiveManager.fadeDelay, objectiveManager.panelFaderSpeed, true);
		}
	}
}
