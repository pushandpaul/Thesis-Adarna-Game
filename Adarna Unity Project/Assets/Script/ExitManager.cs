using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	private ScreenFader screenFader;

	//public SceneStateHandler sceneStateHandler;

	void Start(){
		//sceneStateHandler = FindObjectOfType<SceneStateHandler>();
		screenFader = FindObjectOfType<ScreenFader>();

	}

	void OnTriggerEnter2D (Collider2D other){
		//sceneStateHandler.saveCoordinates();
		if(other.tag == "Player"){
			LevelManager.isDoor = false;
			LevelManager.exitInRight = isRight;
			StartCoroutine(ChangeLevel());
			//screenFader.endScene(nextLocation);
			//SceneManager.LoadScene(nextLocation);
		}
	}

	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
