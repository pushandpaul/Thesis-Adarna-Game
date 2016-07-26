using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Fungus;

public class DoorHandler : MonoBehaviour {

	public int thisDoorIndex;
	public string nextLocation;
	public bool doorPlacementUp = true;
	public bool isOpen = true;
	public bool closedMessageDisplayed = false;
	public bool isActualDoor = true;

	private KeyCode openDoorButton;

	//public bool defaultSpawnLeft = true;
	private bool playerInZone;
	public bool waitForPress;

	private LevelManager levelManager;
	private LevelLoader levelLoader;

	private Flowchart globalFlowchart;
	private DoorAndExitController controller;

	private InteractPrompt interactionPrompt;

	void Awake(){
		levelManager = FindObjectOfType<LevelManager>();
		levelLoader = FindObjectOfType<LevelLoader>();
		GameObject flowchartHolder = GameObject.FindWithTag ("Global Flowchart");
		controller = FindObjectOfType<DoorAndExitController>();
		globalFlowchart = flowchartHolder.GetComponent<Flowchart> ();
		interactionPrompt = FindObjectOfType<InteractPrompt>();

		if(levelLoader == null){
			levelLoader = this.gameObject.AddComponent<LevelLoader>();
			levelLoader.Init();
		}

		if(doorPlacementUp)
			openDoorButton = KeyCode.W;
		else
			openDoorButton = KeyCode.S;

	}

	void Update () {
		if(waitForPress){
			if(Input.GetKeyDown(openDoorButton) && playerInZone){
				if(isOpen){
					showInteractionprompt(false);
					launchNextScene();
				}
				else {
					displayDialogue();
				}
				//LevelManager.exitInRight = defaultSpawnLeft;
			}
		}
	}

	void launchNextScene(){
		LevelManager.isDoor = true;
		LevelManager.doorIndex = thisDoorIndex;
		levelLoader.launchScene(nextLocation);
	}

	public void setIsOpen(bool isOpen){
		this.isOpen = isOpen;
	}

	void displayDialogue(){
		/*if(isActualDoor){
			globalFlowchart.SendFungusMessage ("Door " + Random.Range(1,3));
		}
		else{
			globalFlowchart.SendFungusMessage ("Exit " + Random.Range(1,4));
		}*/
		globalFlowchart.SendFungusMessage ("Exit " + Random.Range(1,4));
	}

	void showInteractionprompt(bool show){
		if(openDoorButton == KeyCode.W){
			interactionPrompt.show(InteractPrompt.keyToInteract.W, show, transform);
		}
		else if(openDoorButton == KeyCode.S){
			interactionPrompt.show(InteractPrompt.keyToInteract.S, show, transform);
		}

	}
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			//Debug.Log("Press " + openDoorButton);
			playerInZone = true;
			if(!waitForPress){
				if(isOpen){
					launchNextScene();
				}
				else{
					if(!isActualDoor){
						controller.movePlayerAway(other.transform);
						//StartCoroutine (waitForReverse(other.transform));
					}
					displayDialogue();
				}
			}
			else{
				if(isOpen){
					showInteractionprompt(true);
				}
				else
					showInteractionprompt(false);
			}
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			//Debug.Log("Door Left");
			playerInZone = false;
			closedMessageDisplayed = false;

			if(waitForPress){
				showInteractionprompt(false);
			}
		}
	}

	IEnumerator waitForReverse(Transform playerHolder){
		PlayerController player = playerHolder.GetComponent<PlayerController> ();
		bool inDoor = true;

		player.canMove = false;

		while(inDoor){
			if(playerHolder.localScale.x > 0){
				if(Input.GetKeyDown(KeyCode.A)){
					player.canMove = true;
					inDoor = true;
				}
			}
			else if(playerHolder.localScale.x < 0){
				if(Input.GetKeyDown(KeyCode.D)){
					player.canMove = true;
					inDoor = true;
				}
			}
			yield return null;
		}
	}
}
