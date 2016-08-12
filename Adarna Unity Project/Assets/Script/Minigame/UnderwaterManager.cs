using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Fungus;

public class UnderwaterManager : MonoBehaviour {

	public Underwater_CollectSingsing toCollectPrefab;
	private Underwater_CollectSingsing toCollect;

	public static Vector3 toCollectPosition;
	public static float timeLimit;
	public static bool hasTimeLimit;

	public Flowchart flowchart;

	private BetterTimer timer;
	private GenericMinigameManger miniGameManager;
	private bool endedMinigame;

	void Awake(){
		miniGameManager = GetComponent<GenericMinigameManger>();
		timer = GetComponent<BetterTimer>();
	}

	void Start(){
		if(miniGameManager != null){
			StartCoroutine(waitToStart());
		}
		else
			Debug.Log("no minigame manager, minigame cannot start.");
		/*else{
			Init();
		}*/
	}

	void Init(){
		toCollect = (Underwater_CollectSingsing) Instantiate(toCollectPrefab, toCollectPosition,Quaternion.identity);
		if(hasTimeLimit){
			flowchart.ExecuteBlock("Intro with Time");
		}
		else
			flowchart.ExecuteBlock("Intro without Time");

	}

	public void officialStart(){
		StartCoroutine(waitToBeCollected());

	}

	void collected(){
		flowchart.ExecuteBlock("Collected");
	}

	void end(){
		Debug.Log("This is the end");
		endedMinigame = true;
		flowchart.ExecuteBlock("End");
	}


	void lose(){
		Debug.Log("You Lose.");
		flowchart.ExecuteBlock("Lose");
	}

	public void enableExitTrigger(bool enable){
		this.GetComponent<Collider2D>().enabled = enable;
	}

	void OnTriggerEnter2D(Collider2D other){
		end();
		other.enabled = false;
	}

	IEnumerator waitToStart(){
		while(!miniGameManager.checkCanStart()){
			yield return null;
		}

		Init();
	}

	IEnumerator subscribeToTimer(){
		timer.startTimer(timeLimit);
		while(timer.onGoing){
			if(toCollect.collected){
				collected();
			}
			if(endedMinigame){
				timer.onGoing = false;
			}
			yield return null;
		}
		if(!endedMinigame){
			lose();
		}
	}

	IEnumerator waitToBeCollected(){
		if(hasTimeLimit){
			StartCoroutine(subscribeToTimer());
		}
		else{
			while(!toCollect.collected){
				yield return null;
			}
			collected();
		}
	}

}
