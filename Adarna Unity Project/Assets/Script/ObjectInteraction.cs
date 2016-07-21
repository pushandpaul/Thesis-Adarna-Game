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

	private bool messageSent = false;
	private InteractPrompt interactionPrompt;
	// Use this for initialization

	void Awake(){

		this.GetComponent<Collider2D>().enabled = false;
		interactionPrompt = FindObjectOfType<InteractPrompt>();
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

	void OnLevelWasLoaded(){
		//this.GetComponent<Collider2D>().enabled = true;
		StartCoroutine(waitBeforeEnabling());
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
		if(!messageSent){
			if(anObjective){
				objectiveMapper.checkIfCurrent_object();
			}
			if(message != ""){
				Debug.Log("Send message: " + message);
				startDialogue(message);
			}
			messageSent = true;
		}
	}

	void startDialogue(string toSend){
		if(flowchart != null){
			//flowchart.SendFungusMessage(toSend);	
			flowchart.ExecuteBlock(toSend);
		}
		//objectiveMapper.textBox.disableTextBox();
	}

	void OnTriggerEnter2D (Collider2D other){
		
		//triggered = true;
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
			messageSent = false;
			if(pressToInteract){
				if(!anObjective || objectiveMapper.checkIfCurrent()){
					interactionPrompt.show(InteractPrompt.keyToInteract.E, false, transform);
				}
				waitForPress = false;
			}
		}
	}


	IEnumerator waitBeforeEnabling(){
		//yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(0.5f);
		this.GetComponent<Collider2D>().enabled = true;

	}
	/*IEnumerator checkIfInDialogue(){
		Debug.Log("Waiting until dialogue is over.");
		yield return new WaitWhile(DialogueController.inDialogue);
		Debug.Log("Dialogue is over. Checking if this is the current objective.");
		checkIfCurrent();
	}*/
}
