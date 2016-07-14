using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Fungus;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;
	public bool isOpen = true;

	private LevelManager levelManager;
	private GameManager gameManager;
	private Flowchart globalFlowchart;
	private DoorAndExitController controller;
	//public FollowTarget followers;

	void Awake(){
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
		GameObject flowchartHolder = GameObject.FindWithTag ("Global Flowchart");
		controller = FindObjectOfType<DoorAndExitController>();
		globalFlowchart = flowchartHolder.GetComponent<Flowchart> ();
	}

	void OnTriggerEnter2D (Collider2D other){
		FollowerManager followerManager = FindObjectOfType<FollowerManager>();
		MoveCharacter moveCharacter = FindObjectOfType<MoveCharacter>();
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

		if(levelLoader == null){
			levelLoader = this.gameObject.AddComponent<LevelLoader>();
			//levelLoader.Init();
		}

		Vector3 playerPositionWhenClosed = new Vector3();
		string playerFlipDirection = "";

		if(other.tag == "Player"){
			if(isOpen){
				LevelManager.isDoor = false;
				LevelManager.exitInRight = isRight;
				levelLoader.launchScene(nextLocation);
			}
			else{
				globalFlowchart.SendFungusMessage ("Exit " + Random.Range(1,4));
				StopAllCoroutines();
				controller.movePlayerAway(other.transform);
				//StartCoroutine (waitForReverse(other.transform));
			}
		}
	}

	public void setIsOpen(bool isOpen){
		this.isOpen = isOpen;
	}

	IEnumerator waitForReverse(Transform playerHolder){
		PlayerController player = playerHolder.GetComponent<PlayerController> ();
		bool inExit = true;

		player.canMove = false;

		while(inExit){
			if(playerHolder.localScale.x > 0){
				if(Input.GetKeyDown(KeyCode.A)){
					player.canMove = true;
					inExit = true;
				}
			}
			else if(playerHolder.localScale.x < 0){
				if(Input.GetKeyDown(KeyCode.D)){
					player.canMove = true;
					inExit = true;
				}
			}
			yield return null;
		}
	}
}
