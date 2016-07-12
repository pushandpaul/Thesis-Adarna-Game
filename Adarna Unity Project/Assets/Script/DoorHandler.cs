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

	private KeyCode openDoorButton;

	//public bool defaultSpawnLeft = true;
	private bool playerInZone;
	public bool waitForPress;

	private LevelManager levelManager;
	private GameManager gameManager;
	private LevelLoader levelLoader;

	private Flowchart globalFlowchart;

	void Awake(){
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
		levelLoader = FindObjectOfType<LevelLoader>();
		GameObject flowchartHolder = GameObject.FindWithTag ("Global Flowchart");
		globalFlowchart = flowchartHolder.GetComponent<Flowchart> ();

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
					LevelManager.isDoor = true;
					LevelManager.doorIndex = thisDoorIndex;
					levelLoader.launchScene(nextLocation);
				}
				else {
					globalFlowchart.SendFungusMessage ("Door " + Random.Range(1,3));
				}
				//LevelManager.exitInRight = defaultSpawnLeft;
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
			if(!isOpen && !waitForPress){
				globalFlowchart.SendFungusMessage ("Door " + Random.Range(1,3));
				StartCoroutine (waitForReverse(other.transform));
			}
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			Debug.Log("Door Left");
			playerInZone = false;
			closedMessageDisplayed = false;
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
