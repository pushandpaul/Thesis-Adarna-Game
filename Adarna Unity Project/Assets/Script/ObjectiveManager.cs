using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {
	private static ObjectiveManager instance = null;

	public Objective currentObjective;
	public int currentObjectiveIndex;
	private Objective[] allObjectives;
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
		//destroyList = new List<GameObject>();
		allObjectives = this.GetComponentsInChildren<Objective>();
		textBox = this.GetComponent<TextBoxManager>();
		//objectivePanelFader.FadeIn(fadeDelay);
		for(int i = 0; i < allObjectives.Length; i++)
			allObjectives[i].objectiveIndex = i;

		currentObjectiveIndex = currentObjective.objectiveIndex;

		if(currentObjective.Description != "")
			displayCurrentObjective();

		this.printCurrentObjective();
	}

	public void printCurrentObjective(){
		Debug.Log("This is the current objective " + currentObjective.Name);
	}

	public void displayCurrentObjective(){
		textBox.setText(currentObjective.Description);
	}
}
