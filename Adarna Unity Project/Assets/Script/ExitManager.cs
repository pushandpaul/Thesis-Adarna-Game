using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	private LevelManager levelManager;
	private GameManager gameManager;
	private ScreenFader screenFader;
	//public FollowTarget followers;

	void Start(){
		screenFader = FindObjectOfType<ScreenFader>();
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter2D (Collider2D other){
		FollowerManager followerManager = FindObjectOfType<FollowerManager>();
		if(other.tag == "Player"){
			LevelManager.isDoor = false;
			LevelManager.exitInRight = isRight;
			if(levelManager != null)
				levelManager.onLevelExit();
			StartCoroutine(ChangeLevel());
		}

	}
	IEnumerator ChangeLevel(){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(nextLocation);
	}
}
