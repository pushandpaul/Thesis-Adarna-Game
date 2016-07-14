using UnityEngine;
using System.Collections;
using Fungus;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour {
	private bool waitForPress;
	public bool pressToInteract;

	public Flowchart flowchart;

	public PlayerController player;

	public int panelFadeDelay;

	private ObjectiveMapper objectiveMapper;
	public bool anObjective;
	//public bool isAchieved;

	public string message;
	public string origMessage;

	private InteractPrompt interactionPrompt;
	// Use this for initialization

	void Awake(){
		interactionPrompt = FindObjectOfType<InteractPrompt>();
	}

	void Start () {

		if(message == ""){
			message = this.name;
		}

		origMessage = message;
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		if(objectiveMapper != null)
			anObjective = true;
		else{
			anObjective = false;
		}

		foreach(Flowchart _flowchart in FindObjectsOfType<Flowchart>()){
			if(_flowchart.tag != "Global Flowchart"){
				flowchart = _flowchart;
			}
		}

		//flowchart = FindObjectOfType<Flowchart>();
		player = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		if(pressToInteract && waitForPress && Input.GetKeyDown(KeyCode.E)){
			Debug.Log ("Interacted with Object " + gameObject.name);
			interactionPrompt.show(InteractPrompt.keyToInteract.E, false, transform);
			checkIfCurrent();
		}
	}

	private void checkIfCurrent(){
		if(anObjective){
			objectiveMapper.checkIfCurrent_object();
		}
		if(message != ""){
			Debug.Log(message);
			startDialogue(message);

		}
	}

	void startDialogue(string toSend){
		if(flowchart != null){
			flowchart.SendFungusMessage(toSend);	
		}
		//objectiveMapper.textBox.disableTextBox();
	}

	void OnTriggerEnter2D (Collider2D other){
		//Debug.Log("Collided with " + gameObject.name);
		if(other.tag == "Player"){
			if(pressToInteract){
				if(!anObjective || objectiveMapper.checkIfCurrent()){
					interactionPrompt.show(InteractPrompt.keyToInteract.E, true, transform);
				}
				Debug.Log ("Interact");
				waitForPress = true;
			}
			else
				checkIfCurrent();
		}
	}

	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			if(pressToInteract){
				if(!anObjective || objectiveMapper.checkIfCurrent()){
					interactionPrompt.show(InteractPrompt.keyToInteract.E, false, transform);
				}
				waitForPress = false;
			}
		}
	}

	/*IEnumerator checkIfInDialogue(){
		Debug.Log("Waiting until dialogue is over.");
		yield return new WaitWhile(DialogueController.inDialogue);
		Debug.Log("Dialogue is over. Checking if this is the current objective.");
		checkIfCurrent();
	}*/
}
