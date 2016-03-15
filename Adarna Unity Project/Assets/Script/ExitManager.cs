using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	//public SceneStateHandler sceneStateHandler;

	void Start(){
		//sceneStateHandler = FindObjectOfType<SceneStateHandler>();
	}

	void OnTriggerEnter2D (Collider2D other){
		//sceneStateHandler.saveCoordinates();
		if(other.tag == "Player"){
			LevelManager.isDoor = false;
			LevelManager.exitInRight = isRight;
			SceneManager.LoadScene(nextLocation);
		}
	}
}
