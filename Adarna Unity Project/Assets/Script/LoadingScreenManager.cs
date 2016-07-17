using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {

	private ScreenFader screenFader; 
	public UIFader loadingScreen;

	void Awake(){
		screenFader = FindObjectOfType<ScreenFader> ();
	}

	public void loadScene(string sceneName){
		StartCoroutine (startLoadingScene (sceneName));
	}

	IEnumerator startLoadingScene(string sceneName){
		float fadeTime = screenFader.BeginFade(1);

		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		if(levelManager != null){
			levelManager.onLevelExit ();
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

		fadeTime = screenFader.BeginFade (1);

		yield return new WaitForSeconds (fadeTime);

		loadingScreen.canvasGroup.alpha = 0f;
		loadingScreen.canvasGroup.blocksRaycasts = false;
		screenFader.BeginFade (-1);

	}
}
