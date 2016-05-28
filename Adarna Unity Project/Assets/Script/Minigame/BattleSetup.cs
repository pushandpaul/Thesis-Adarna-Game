using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleSetup : MonoBehaviour {

	public enum EnemyType{
		Higante,
		Serpyente,
	}

	public enum Stage{
		ArmenyaCastle,
	}

	[System.Serializable]
	public class EnemyPrefab{
		public EnemyType type;
		public GameObject prefab;
		public Vector3 position;
	}

	[System.Serializable]
	public class BattleStagePrefab{
		public Stage stage;
		public GameObject prefab;
	}

	public BoxCollider2D smallCameraBounds;
	public BoxCollider2D mediumCameraBounds;
	public BoxCollider2D largeCameraBounds;

	public EnemyPrefab[] enemyPrefabs;
	public BattleStagePrefab[] battleStagePrefabs;

	private BattleStateMachine battleStateMachine;
	private CameraController camera;
	private GameManager gameManager;

	void Awake(){		
		gameManager = FindObjectOfType<GameManager>();
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		if(gameManager != null)
			Init(gameManager.battleEnemyType, gameManager.battleStage);

	}
	public void Init(EnemyType enemyType, Stage stage){
		foreach(EnemyPrefab enemyPrefab in enemyPrefabs){
			if(enemyType.ToString() == enemyPrefab.type.ToString()){
				//Instantiate enemy
				Debug.Log("Found enemy type: " + enemyPrefab.type.ToString());
				break;
			}
		}

		foreach(BattleStagePrefab battleStagePrefab in battleStagePrefabs){
			if(stage.ToString() == battleStagePrefab.stage.ToString()){
				Instantiate(battleStagePrefab.prefab, Vector3.zero, Quaternion.identity);
				Debug.Log("Found stage: " + battleStagePrefab.stage.ToString());
				break;
			}
		}
		battleStateMachine.Begin();
	}
}
