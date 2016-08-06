using UnityEngine;
using System.Collections;
using Fungus;

public class MoveMountainManager : MonoBehaviour {

	public GameObject generalFX;

	public MoveMountain_Draggable currentToDrag;

	public GenericMinigameManger miniGameManager;
	public Flowchart flowchart;
	private bool started;
	private ObjectiveMapper objectiveMapper;
	
	void Awake(){
		miniGameManager = FindObjectOfType<GenericMinigameManger>();
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
	}

	void Start(){
		if(miniGameManager != null){
			StartCoroutine(waitToStart());
		}
	}

	void Update () {

		if(!started){
			return;
		}

		if(currentToDrag != null){
			if(currentToDrag.isMoving){
				generalFX.SetActive(true);
			}
			else{
				generalFX.SetActive(false);
			}
		}
	}

	void Init(){
		flowchart.ExecuteBlock("Intro");
	}

	public void officialStart(){
		started = true;
	}

	public void achieved(string toCallBlock){
		flowchart.ExecuteBlock(toCallBlock);
	}

	public void end(){
		Debug.Log("Minigame is ended.");

		if(objectiveMapper != null){
			objectiveMapper.checkIfCurrent_misc();
		}
	}

	public void wrong(string toCallBlock){
		flowchart.ExecuteBlock(toCallBlock);
	}

	public void changeCurrentToDrag(MoveMountain_Draggable toDrag){
		toDrag.allowDrag = true;
		currentToDrag = toDrag;
	}

	IEnumerator waitToStart(){
		while(!miniGameManager.checkCanStart()){
			yield return null;
		}

		Init();
	}
}
