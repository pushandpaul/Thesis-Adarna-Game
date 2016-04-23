using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour {

	public int thisDoorIndex;
	public string nextLocation;

	//public bool defaultSpawnLeft = true;
	private bool playerInZone;
	public bool waitForPress;

	private ScreenFader screenFader;
	private LevelManager levelManager;
	private GameManager gameManager;
	// Use this for initialization
	void Start(){
		screenFader = FindObjectOfType<ScreenFader>();
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
	}

	void Update () {
		if(waitForPress){
			if(Input.GetKeyDown(KeyCode.W) && playerInZone){
				//LevelManager.exitInRight = defaultSpawnLeft;
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				if(FindObjectsOfType<ObjectData>().Length > 0)
					gameManager.saveCoordinates(FindObjectsOfType<ObjectData>());
				StartCoroutine(ChangeLevel());
				//SceneManager.LoadScene(nextLocation);
			}
		}
		else{
			if(playerInZone){
				//LevelManager.exitInRight = defaultSpawnLeft;
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				if(FindObjectsOfType<ObjectData>().Length > 0)
					gameManager.saveCoordinates(FindObjectsOfType<ObjectData>());
				FollowerManager followerManager = FindObjectOfType<FollowerManager>();
				followerManager.updateFollowerList();
				StartCoroutine(ChangeLevel());
				//SceneManager.LoadScene(nextLocation);
			}
		}
	}
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			Debug.Log("Press W");
			playerInZone = true;
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			Debug.Log("Door Left");
			playerInZone = false;
		}
	}

	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
