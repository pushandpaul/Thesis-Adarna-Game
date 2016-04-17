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

	void Start () {
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		if(isNPC)
			npc = this.GetComponent<NPCInteraction>();
		else if(isObject)
			objectiveHolder = this.GetComponent<ObjectInteraction>();
	}

	public void checkIfCurrent_npc(){
		bool found = false;
		/*for(int i = 0; i < objectiveIndex.Length; i++){
			Debug.Log("Entered Objective Checker Loop");
			if(objectiveManager.currentPartIndex == objectivePartIndex[i] && objectiveManager.currentObjectiveIndex == objectiveIndex[i]){
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
				
		}*/

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
						npc.message = npc.name;
						found = false;
					}
				}
			}
			if(found)
				break;
		}
	}
		
	public bool checkIfCurrent() {
		bool found = false;
		/*for (int i = 0; i < objectiveIndex.Length; i++) {
			if (objectiveManager.currentPartIndex == objectivePartIndex[i] && objectiveManager.currentObjectiveIndex == objectiveIndex [i]) {
				return true;
				break;
			} 	
		}*/

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

	public void checkIfCurrent_object(){
		bool found = false;
		/*for(int i = 0; i < objectiveIndex.Length; i++){
			Debug.Log("Entered Objective Checker Loop");
			if(objectiveManager.currentPartIndex == objectivePartIndex[i] && objectiveManager.currentObjectiveIndex == objectiveIndex[i]){
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
		}*/

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
						objectiveHolder.message = "";
						found = false;
					}
				}
			}
			if(found)
				break;
		}

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
