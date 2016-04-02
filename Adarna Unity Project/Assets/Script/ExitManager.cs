using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	private LevelManager levelManager;
	private GameManager gameManager;
	private ScreenFader screenFader;


	void Start(){
		screenFader = FindObjectOfType<ScreenFader>();
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			LevelManager.isDoor = false;
			LevelManager.exitInRight = isRight;
			if(FindObjectsOfType<ObjectData>().Length > 0)
				gameManager.saveCoordinates(FindObjectsOfType<ObjectData>());
			StartCoroutine(ChangeLevel());
		}
	}
	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
