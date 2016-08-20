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
		RDLCStables,
	}

	[System.Serializable]
	public class EnemyPrefab{
		public EnemyType type;
		public GameObject prefab;
		public float yPosition;
		public Stats_BattleEnemy stats;
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

	public GameObject enemyAvatar;
	public EnemyPrefab[] enemyPrefabs;
	public BattleStagePrefab[] battleStagePrefabs;

	private BattleStateMachine battleStateMachine;
	private GameManager gameManager;

	void Awake(){		
		gameManager = FindObjectOfType<GameManager>();
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		if(gameManager != null)
			Init(gameManager.battleData.enemyType, gameManager.battleData.stage, gameManager.battleData.enemyBaseHP);

		//Init(EnemyType.Serpyente, Stage.ArmenyaCastle, 0);
	}

	public void Init(EnemyType enemyType, Stage stage, int enemyBaseHP){

		Vector3 enemyPosition = Vector3.zero;
		CameraController camera = FindObjectOfType<CameraController> ();
		float cameraSize = 0f;

		BattleEnemy enemy = battleStateMachine.enemy;

		foreach(EnemyPrefab enemyPrefab in enemyPrefabs){
			if(enemyType.ToString() == enemyPrefab.type.ToString()){
				Debug.Log("Found enemy type: " + enemyPrefab.type.ToString());
				//Instantiate enemy
				enemyPosition = new Vector3(enemyBattlePosition.position.x, enemyPrefab.yPosition, enemyBattlePosition.position.z);
				enemyAvatar = (GameObject) Instantiate(enemyPrefab.prefab, enemyPosition, Quaternion.identity);

				enemy.stats = enemyPrefab.stats;
				if (enemyBaseHP > 0)
					enemy.stats.baseHP = enemyBaseHP;
				enemy.enabled = true;
				enemy.anim = enemyAvatar.GetComponentInChildren<Animator> ();

				switch(enemy.stats.size){
				case (Stats_BattleEnemy.Size.Small): 
					cameraSize = 5f;
					break;
				case (Stats_BattleEnemy.Size.Medium):
					cameraSize = 6f;
					break;
				case (Stats_BattleEnemy.Size.Large):
					cameraSize = 7f;
					break;
				case (Stats_BattleEnemy.Size.ExtraLarge):
					cameraSize = 8f;
					break;
				}

				camera.camera.orthographicSize = cameraSize;
				camera.initialCamSize = cameraSize;

				break;
			}
		}

		foreach(BattleStagePrefab battleStagePrefab in battleStagePrefabs){
			//Instantiate Stage
			if(stage.ToString() == battleStagePrefab.stage.ToString()){
				Instantiate(battleStagePrefab.prefab, Vector3.zero, Quaternion.identity);
				Debug.Log("Found stage: " + battleStagePrefab.stage.ToString());
				break;
			}
		}
		battleStateMachine.Begin();
	}
}
