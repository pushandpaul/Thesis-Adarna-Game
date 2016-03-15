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
	}
	public enum ObjectiveStatus{
		Pending = 0,
		Achieved = 1,
	}

	public enum ActionOnReach{
		StartDialogue = 0,
		MarkAsAchieved = 1,
	}

	public string Name;
	[Multiline(10)]
	public string Description;
	public ObjectiveType Kind;
	public ObjectiveStatus Status;
	public ActionOnReach[] OnReach;

	public Objective nextObjective;
	public int objectiveIndex;

	//private Flowchart flowchart;
	//public GameObject target;

	public ObjectiveManager manager {get; set;}

	void Start () {
		//Collider2D thisCOllider = this.GetComponent<CircleCollider2D>();
		manager = FindObjectOfType<ObjectiveManager>();
		//flowchart = null;
		//Debug.Log("" + this.manager.currentObjective.Name);

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
			manager.currentObjectiveIndex = this.nextObjective.objectiveIndex;
		}
	}

	private void startDialogue(){
		Debug.Log("Dialogue Started");
		//flowchart = FindObjectOfType<Flowchart>();
		//flowchart.ExecuteBlock(this.name);
	}
	/*private void onReach(){
		if(this.OnReach.Contains(ActionOnReach.MarkAsAchieved)){
			this.Status = ObjectiveStatus.Achieved;
			Debug.Log("Achieved Objective: " + this.Name);
		}
			
		if(this.OnReach.Contains(ActionOnReach.StartDialogue))
			this.startDialogue();
		if(nextObjective != null){
			manager.currentObjective = this.nextObjective;
			Debug.Log("This is the last Objective");
		}
			
		manager.printCurrentObjective();
	}

	private void startDialogue(){
		Debug.Log("Dialogue Started for " + this.Name);
	}
		
	void OnTriggerEnter2D (Collider2D other){
		Debug.Log("Triggered Objective: " + this.name);

		if(other.tag == "Player" && this.manager.currentObjective.name == this.name){
			Debug.Log("Collided with Player");
			onReach();
		}
	}*/

	/*----------Promosed Solution----------------

	if(other.tag == "Player" && this.ObjectiveIndex == ObjectiveManager.currentObjectiveIndex)
		ObjectiveManager.currentObjective.OnReach();

		*/
		
}
