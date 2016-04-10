using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public enum LevelSelect{
		KwartoNiHaringFernando = 0,
		CastleThroneRoom = 1,
		CastleWalkway = 2,
		CastleBridge = 3,
		CastleBridgeMiddle = 4,
		CastleBridgeToForest = 5,
		Forest = 6,
		ForestPedrasPlatas = 7,
		BahayNiErmitanyo = 8,
		BahayNiErmitanyoInterior = 9,
		GreenToDarkForest = 10,
		//Minigames
		MinigameDayap = 11,
	}

	public LevelSelect Levels;
	private ScreenFader screenFader;
	private GameManager gameManager;
	private LevelManager levelManager;

	public bool launchOnStart;
	// Use this for initialization
	void Start () {
		screenFader = FindObjectOfType<ScreenFader>();
		gameManager = FindObjectOfType<GameManager>();
		levelManager = FindObjectOfType<LevelManager>();
		if(launchOnStart)
			launchScene();
	}
	
	public void launchScene(){
		if(levelManager != null){
			if(FindObjectsOfType<ObjectData>().Length > 0)
				gameManager.saveCoordinates(FindObjectsOfType<ObjectData>());
			if(FindObjectsOfType<FollowTarget>().Length > 0){
				FollowTarget[] followers = FindObjectsOfType<FollowTarget>();
				gameManager.findFollowers(followers);
			}
		}
		StartCoroutine(fadeLevel());
	}

	IEnumerator fadeLevel(){
		int levelIndex = 0;
		levelIndex = (int)this.Levels;
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Debug.Log("" + levelIndex);
		SceneManager.LoadScene(levelIndex);
	}
}
