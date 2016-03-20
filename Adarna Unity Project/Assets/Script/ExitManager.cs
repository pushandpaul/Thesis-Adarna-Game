using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	private ScreenFader screenFader;

	void Start(){
		screenFader = FindObjectOfType<ScreenFader>();
	}

	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			LevelManager.isDoor = false;
			LevelManager.exitInRight = isRight;
			StartCoroutine(ChangeLevel());
		}
	}
	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
