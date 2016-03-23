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
		BahayNiErmitanyoInsecure = 9,
		GreenToDarkForest = 10,
	}

	public LevelSelect Levels;

	public string nextScene;
	// Use this for initialization
	void Start () {
		int levelIndex = 0;
		levelIndex = (int)this.Levels;
		Debug.Log("" + levelIndex);
		SceneManager.LoadScene(levelIndex);
	}
	
	// Update is called once per frame
	void Update () {
		/*if(nextScene != null)
			SceneManager.LoadScene(nextScene);*/
	}
}
