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
	}

	public string Name;
	[Multiline(10)]
	public string Description;

	public ObjectiveType Kind;
	public ObjectiveStatus Status;
	public ActionOnReach[] OnReach;

	public Objective nextObjective;
	public int objectiveIndex;

	public ObjectiveManager manager {get; set;}
	private TextBoxManager textBox;

	void Start(){
		manager = FindObjectOfType<ObjectiveManager>();
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
		if(nextObjective != null){
			manager.currentObjective = this.nextObjective;
			if(nextObjective.OnReach.Contains(ActionOnReach.DisplayToTextBox)){
				displayToTextbox();
			}

			manager.currentObjectiveIndex = this.nextObjective.objectiveIndex;
		}
	}

	private void startDialogue(){
		Debug.Log("Dialogue Started");
	}

	private void displayToTextbox(){
		Debug.Log("Display this to Text Box");
		textBox.setText(nextObjective.Description);
	}
}
