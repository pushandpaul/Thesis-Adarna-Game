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
		//Minigame
		MinigameDayap = 11,
		DarkForest = 12,
		KwartoNiAdarna = 13,
		KwartoNiDonJuan = 14,
		Castle2ndFloor = 15,
		DarkToGreenForest = 16,
		DownhillForest = 17,
		BerbanyaToArmenya = 18,
		ArmenyaForestNiJuan = 19,
		ArmenyaFallsBridge = 20,
		ForestToMountain = 21,
		MountaintTop = 22,
	}

	public LevelSelect Levels;
	private ScreenFader screenFader;
	//private GameManager gameManager;
	//private LevelManager levelManager;

	public bool launchOnStart;
	// Use this for initialization
	void Start () {
		screenFader = FindObjectOfType<ScreenFader>();
		if(launchOnStart)
			launchScene();
	}
	
	public void launchScene(){
		saveBeforeUnload();
		StartCoroutine(fadeLevelByList());
	}

	public void launchScene(string sceneName){
		saveBeforeUnload();
		StartCoroutine(fadeLevelByName(sceneName));
	}

	public void reloadScene(){
		GameManager gameManager = FindObjectOfType<GameManager>();
		saveBeforeUnload ();
		StartCoroutine(fadeLevelByName(gameManager.currentScene));
	}

	private void saveBeforeUnload(){
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		if(levelManager != null){
			levelManager.onLevelExit();
		}
	}

	IEnumerator fadeLevelByList(){
		int levelIndex = 0;
		levelIndex = (int)this.Levels;
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		//Debug.Log("" + levelIndex);
		SceneManager.LoadScene(levelIndex);
	}

	IEnumerator fadeLevelByName(string sceneName){
		float fadeTime = screenFader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(sceneName);
	}
}
