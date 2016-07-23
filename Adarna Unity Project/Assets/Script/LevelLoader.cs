using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public enum LevelSelect{
		InitialScene = 0,
		KwartoNiHaringFernando = 1,
		CastleThroneRoom = 2,
		CastleWalkway = 3,
		CastleBridge = 4,
		CastleBridgeMiddle = 5,
		CastleBridgeToForest = 6,
		Forest = 7,
		ForestPedrasPlatas = 8,
		BahayNiErmitanyo = 9,
		BahayNiErmitanyoInterior = 10,
		GreenToDarkForest = 11,
		//Minigame
		MinigameDayap = 12,
		DarkForest = 13,
		KwartoNiAdarna = 14,
		KwartoNiDonJuan = 15,
		Castle2ndFloor = 16,
		DarkToGreenForest = 17,
		DownhillForest = 18,
		BerbanyaToArmenya = 19,
		ArmenyaForestNiJuan = 20,
		ArmenyaFallsBridge = 21,
		ForestToMountain = 22,
		Mountaintop = 23,
		MountainToForest = 24,
		ArmenyaBalon = 25,
		//Minigame
		MinigameBalon = 26,
		ArmenyaIlalimNgBalon = 27,
		ArmenyaCastleWalkway = 28,
		ArmenyaHallway = 29,
		ArmenyaBalcony = 30,
		//Minigame
		MinigameBattle = 31,
		ArmenyaBatis = 32,
		ForestToArmenya = 33,
		ReynoBahayNiErmits = 34,
		ReynoGenericForest = 35,
		ReynoBanyoNgPrinsesa = 36,
		ReynoAgilaDropoff = 37,
		ReynoBeforeBahayNiErmits2 = 38,
		ReynoBahayNiErmitanyo2 = 39,
		//Minigame
		ReynoMaze = 40,
		//Minigame
		MinigameAgila = 41,
		ReynoCastleThrone = 42,
		ReynoCastleHallway = 43,
		ReynGardenHallway = 44,
		ReynoPrinsesaHallway = 45,
		ReynoKwartoNgHari = 46,
		ReynoBalcony = 47,
		ReynoMazeEntrance = 48,
		ReynoLabasNgPalasyo = 49,
		ReynoTrigo = 50,
		Assessment = 51,
		//UI
		ChapterSelect = 52,
		MainMenu = 53,
		Intro = 54,
		Prayer = 55,
	}

	public LevelSelect Levels;
	private ScreenFader screenFader;
	private LoadingScreenManager loadingScreenManager;
	//private GameManager gameManager;
	//private LevelManager levelManager;

	public bool launchOnStart;
	public static string sceneToLoad;

	void Awake () {
		Init();
		if(launchOnStart)
			launchScene();
		loadingScreenManager = FindObjectOfType<LoadingScreenManager> ();
	}
	
	public void launchScene(){
		saveBeforeUnload();
		StartCoroutine(fadeLevelByList());
	}
		
	public void launchScene(string sceneName){
		sceneToLoad = sceneName;
		saveBeforeUnload();
		loadingScreenManager.loadScene (sceneName);
		//StartCoroutine(fadeLevelByName(sceneName));
	}

	public void discreteLaunchScene(string sceneName){
		saveBeforeUnload ();
		loadingScreenManager.loadScene (sceneName);
	}
		
	public void launchBattleScene(BattleSetup.EnemyType enemyType, BattleSetup.Stage stage){
		GameManager gameManager = FindObjectOfType<GameManager>();
		LevelManager levelManager = FindObjectOfType<LevelManager>();

		gameManager.setBattleInit (enemyType, stage, 0);

		if(levelManager != null)
			levelManager.savePlayerPosition();

		launchScene("(Minigame) Battle");
	}

	public void launchBattleScene(BattleSetup.EnemyType enemyType, BattleSetup.Stage stage, int enemyBaseHP){
		GameManager gameManager = FindObjectOfType<GameManager>();
		LevelManager levelManager = FindObjectOfType<LevelManager>();

		gameManager.setBattleInit (enemyType, stage, enemyBaseHP);

		if(levelManager != null)
			levelManager.savePlayerPosition();

		launchScene("(Minigame) Battle");
	}

	public void reloadScene(){
		GameManager gameManager = FindObjectOfType<GameManager>();
		saveBeforeUnload ();
		StartCoroutine(fadeLevelByName(gameManager.currentScene));
	}

	public void launchPrevScene(){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		launchScene (gameManager.prevScene);
	}

	private void saveBeforeUnload(){
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		if(levelManager != null && levelManager.tag != "Ignore"){
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

	public void Init(){
		screenFader = FindObjectOfType<ScreenFader>();
	}

	void OnLevelWasLoaded(){
		//sceneToLoad = "";
	}
}
