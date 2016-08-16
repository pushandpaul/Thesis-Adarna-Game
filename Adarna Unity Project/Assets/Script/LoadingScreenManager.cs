using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {

	private ScreenFader screenFader; 
	public UIFader loadingScreen;
	private PauseMenu pauseMenu;

	public bool usedLoadingScreen = false;
	public bool officiallyLoaded = false;

	void Awake(){
		screenFader = FindObjectOfType<ScreenFader> ();
		pauseMenu = FindObjectOfType<PauseMenu>();
	}

	public void loadScene(string sceneName){
		//May subject to change
		StopAllCoroutines();
	
		officiallyLoaded = false;
		usedLoadingScreen = true;
		StartCoroutine (startLoadingScene (sceneName));
	}

	IEnumerator startLoadingScene(string sceneName){
		float fadeTime = screenFader.BeginFade(1);
		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		bool inControl = false;

		if(levelManager != null){
			levelManager.onLevelExit ();
		}

		if(pauseMenu.enabled){
			pauseMenu.enabled = false;
			inControl = true;
		}

		yield return new WaitForSeconds(fadeTime);
		screenFader.BeginFade (-1);
		loadingScreen.canvasGroup.alpha = 1f;
		loadingScreen.canvasGroup.blocksRaycasts = true;
		yield return new WaitForSeconds (fadeTime); 

		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
		while (!async.isDone) {
			Debug.Log ("Loading");
			yield return null;
		}
		Debug.Log ("Finished loading!");

		screenFader.alpha = 0f;
		fadeTime = screenFader.BeginFade (1);

		yield return new WaitForSeconds (fadeTime);
		loadingScreen.canvasGroup.alpha = 0f;
		loadingScreen.canvasGroup.blocksRaycasts = false;
		fadeTime = screenFader.BeginFade (-1);

		yield return new WaitForSeconds(fadeTime - (fadeTime * 0.2f));
		if(inControl){
			pauseMenu.enabled = true;
		}
		officiallyLoaded = true;
	}
}
