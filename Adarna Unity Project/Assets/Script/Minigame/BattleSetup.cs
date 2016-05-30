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
		public float yPosition;
	}

	[System.Serializable]
	public class BattleStagePrefab{
		public Stage stage;
		public GameObject prefab;
	}

	public BoxCollider2D smallCameraBounds;
	public BoxCollider2D mediumCameraBounds;
	public BoxCollider2D largeCameraBounds;

	public Transform playerBattlePosition;
	public Transform enemyBattlePosition;

	public EnemyPrefab[] enemyPrefabs;
	public BattleStagePrefab[] battleStagePrefabs;

	private BattleStateMachine battleStateMachine;
	private GameManager gameManager;

	void Awake(){		
		gameManager = FindObjectOfType<GameManager>();
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		if(gameManager != null)
			Init(gameManager.battleEnemyType, gameManager.battleStage);
		//Init(EnemyType.Higante, Stage.ArmenyaCastle);

	}
	public void Init(EnemyType enemyType, Stage stage){

		GameObject enemyContainer;
		Vector3 enemyPosition = Vector3.zero;

		foreach(EnemyPrefab enemyPrefab in enemyPrefabs){
			if(enemyType.ToString() == enemyPrefab.type.ToString()){
				//Instantiate enemy
				enemyPosition = new Vector3(enemyBattlePosition.position.x, enemyPrefab.yPosition, enemyBattlePosition.position.z);
				enemyContainer = (GameObject) Instantiate(enemyPrefab.prefab, enemyPosition, Quaternion.identity);
				battleStateMachine.enemy = enemyContainer.GetComponent<BattleEnemy>();

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
