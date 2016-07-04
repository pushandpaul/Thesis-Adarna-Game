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
		Mountaintop = 22,
		MountainToForest = 23,
		ArmenyaBalon = 24,
		//Minigame
		MinigameBalon = 25,
		ArmenyaIlalimNgBalon = 26,
		ArmenyaCastleWalkway = 27,
		ArmenyaHallway = 28,
		ArmenyaBalcony = 29,
		//Minigame
		MinigameBattle = 30,
		ArmenyaBatis = 31,
		ForestToArmenya = 32,
		ReynoBahayNiErmits = 33,
		ReynoGenericForest = 34,
		ReynoBanyoNgPrinsesa = 35,
		ReynoAgilaDropoff = 36,
		ReynoBeforeBahayNiErmits2 = 37,
		ReynoBahayNiErmitanyo2 = 38,
		//Minigame
		ReynoMaze = 39,
		//Minigame
		MinigameAgila = 40,
		ReynoCastleThrone = 41,
		ReynoCastleHallway = 42,
		ReynGardenHallway = 43,
		ReynoPrinsesaHallway = 44,
		ReynoKwartoNgHari = 45,
		ReynoBalcony = 46,
		ReynoMazeEntrance = 47,
		ReynoLabasNgPalasyo = 48,
		ReynoTrigo = 49,
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
