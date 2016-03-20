using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour {

	public int thisDoorIndex;
	public string nextLocation;

	private bool playerInZone;
	public bool waitForPress;

	private ScreenFader screenFader;
	// Use this for initialization
	void Start(){
		screenFader = FindObjectOfType<ScreenFader>();
	}

	void Update () {
		if(waitForPress){
			if(Input.GetKeyDown(KeyCode.W) && playerInZone){
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				StartCoroutine(ChangeLevel());
				//SceneManager.LoadScene(nextLocation);
			}
		}
		else{
			if(playerInZone){
				LevelManager.isDoor = true;
				LevelManager.doorIndex = thisDoorIndex;
				StartCoroutine(ChangeLevel());
				//SceneManager.LoadScene(nextLocation);
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

	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
