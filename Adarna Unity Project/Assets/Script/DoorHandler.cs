using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour {

	public int thisDoorIndex;
	public string nextLocation;

	private bool playerInZone;
	public bool waitForPress;
	// Use this for initialization
	void Update () {
		if(waitForPress){
			if(Input.GetKeyDown(KeyCode.W) && playerInZone){
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				SceneManager.LoadScene(nextLocation);
			}
		}
		else{
			if(playerInZone){
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				SceneManager.LoadScene(nextLocation);
			}
		}
	}
	
	// Update is called once per frame
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
}
