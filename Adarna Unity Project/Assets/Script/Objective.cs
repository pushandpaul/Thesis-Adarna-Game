using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Fungus;

public class Objective : MonoBehaviour {

	public enum ObjectiveType{
		Reach = 0,
		Talk = 1,
		Misc = 2,
		Dummy = 3,
	}
	public enum ObjectiveStatus{
		Pending = 0,
		Achieved = 1,
	}

	public enum ActionOnReach{
		StartDialogue = 0,
		MarkAsAchieved = 1,
		DisplayToTextBox = 2,
		LoadScene = 3,
		LoadNextPart = 4,
		LoadAssessment = 5,
		//IfPlayAnimation = 3,
	}

	public string Name;
	[Multiline(10)]
	public string Description;
	public LevelLoader.LevelSelect objectiveLocation;

	public ObjectiveType Kind;
	public ObjectiveStatus Status;
	public ActionOnReach[] OnReach;

	public Objective nextObjective;
	public int objectiveIndex;

	public ObjectiveManager manager {get; set;}
	private TextBoxManager textBox;
	public bool textBoxDisplayed = false;
	private UIFader uiFader;

	void Start(){
		manager = GetComponentInParent<ObjectiveManager>();
		textBox = manager.GetComponent<TextBoxManager>();
	}
	public void onReach(){
		if(this.OnReach.Contains(ActionOnReach.MarkAsAchieved)){
			this.Status = ObjectiveStatus.Achieved;
			Debug.Log("Achieved Objective: " + this.name);
		}

		if(this.OnReach.Contains(ActionOnReach.StartDialogue)){
			startDialogue();
		}

		if (this.OnReach.Contains (ActionOnReach.LoadNextPart)) {
			manager.nextPart ();
		}

		if(this.OnReach.Contains(ActionOnReach.LoadAssessment)){
			loadAssessment();
		}

		if(nextObjective != null){
			
			manager.currentObjective = this.nextObjective;
			manager.currentObjectiveIndex = this.nextObjective.objectiveIndex;
			if(!this.OnReach.Contains(ActionOnReach.StartDialogue) && manager.currentObjective.OnReach.Contains(ActionOnReach.DisplayToTextBox)){
				manager.currentObjective.displayToTextBox();
			}
		}
	}

	private void startDialogue(){
		Debug.Log("Dialogue Started");
	}

	public void displayToTextBox(){
		
		textBox.setText("<color=#ffed0b>Layunin:</color> " + manager.currentObjective.Description);
	}

	private void loadAssessment(){
		//AssessmentManager.assessmentNumber = this.objectiveIndex;
		StartCoroutine(startLoadAssesssment());
	}


	IEnumerator startLoadAssesssment(){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();
		AssessmentManager.assessmentNumber = manager.currentPartIndex;
		while(!DialogueController.inDialogue){
			yield return null;
		}

		while(DialogueController.inDialogue){
			yield return null;
		}

		yield return new WaitForSeconds(1);

		if(levelLoader != null){
			levelLoader.launchScene("() Assessment");
		}
	}
}
