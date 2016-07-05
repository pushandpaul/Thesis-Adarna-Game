using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;
	public bool isOpen = true;

	private LevelManager levelManager;
	private GameManager gameManager;
	//public FollowTarget followers;

	void Awake(){
		levelManager = FindObjectOfType<LevelManager>();
		gameManager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter2D (Collider2D other){
		FollowerManager followerManager = FindObjectOfType<FollowerManager>();
		MoveCharacter moveCharacter = FindObjectOfType<MoveCharacter>();
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

		if(levelLoader == null){
			levelLoader = this.gameObject.AddComponent<LevelLoader>();
			//levelLoader.Init();
		}

		Vector3 playerPositionWhenClosed = new Vector3();
		string playerFlipDirection = "";

		if(other.tag == "Player"){
			if(isOpen){
				LevelManager.isDoor = false;
				LevelManager.exitInRight = isRight;
				levelLoader.launchScene(nextLocation);
			}
			else{
				Debug.Log("Exit closed!");
				//Add narrative triggers.
				if(isRight){
					playerPositionWhenClosed = new Vector3(transform.position.x - 7f, other.transform.position.y, other.transform.position.z);
					playerFlipDirection = "l";
				}
				else{
					playerPositionWhenClosed = new Vector3(transform.position.x + 7f, other.transform.position.y, other.transform.position.z);
					playerFlipDirection = "r";
				}

				if(moveCharacter != null){
					moveCharacter.flipCharacter(other.transform, playerFlipDirection);
					moveCharacter.moveCharacter(other.transform, playerPositionWhenClosed, 3f);
				}
				else
					Debug.Log("Please add 'Movers' prefab in the scene.");
			}
		}
	}

	public void setIsOpen(bool isOpen){
		this.isOpen = isOpen;
	}
}
