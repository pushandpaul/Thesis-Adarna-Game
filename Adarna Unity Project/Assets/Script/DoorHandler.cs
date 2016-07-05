using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour {

	public int thisDoorIndex;
	public string nextLocation;
	public bool doorPlacementUp = true;
	public bool isOpen = true;
	public bool closedMessageDisplayed = false;

	private KeyCode openDoorButton;

	//public bool defaultSpawnLeft = true;
	private bool playerInZone;
	public bool waitForPress;

	private LevelManager levelManager;
	private GameManager gameManager;
	private LevelLoader levelLoader;

	void Awake(){
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
		levelLoader = FindObjectOfType<LevelLoader>();

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
		if(!isOpen){
			if(!closedMessageDisplayed && playerInZone){
				Debug.Log("Door closed.");
				//Add Narrative.
				closedMessageDisplayed = true;
			}
			return;
		}

		if(waitForPress){
			if(Input.GetKeyDown(openDoorButton) && playerInZone){
				//LevelManager.exitInRight = defaultSpawnLeft;
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				levelLoader.launchScene(nextLocation);
			}
		}
		else{
			if(playerInZone){
				//LevelManager.exitInRight = defaultSpawnLeft;
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				levelLoader.launchScene(nextLocation);
			}
		}
	}

	public void setIsOpen(bool isOpen){
		this.isOpen = isOpen;
	}
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			Debug.Log("Press " + openDoorButton);
			playerInZone = true;
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			Debug.Log("Door Left");
			playerInZone = false;
			closedMessageDisplayed = false;
		}
	}
}
